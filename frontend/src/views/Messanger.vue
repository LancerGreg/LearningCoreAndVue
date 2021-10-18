<template>
  <div>
      <h1>Simple chat</h1>
      <div v-if="!messages" class="text-center">
        <p><em>Loading...</em></p>
      </div>
      <template v-if="messages">
        <div v-for="(message, index) in messages" :key="index">
          <p><em>Message at {{ message.date }}:</em> '{{ message.text }}'</p>
        </div>
      </template>
      <section class="form">
        <div class="field">
          <div class="control">
            <input v-model="form.textMessage" class="message-input" type="text" placeholder="Type a message here">
            <button class="dark-bg text-white submit-button" @click.prevent="sendMessage">Submit</button>
          </div>
        </div>
      </section>
    </div>
</template>
 
<script>
import store from "../store"
import axios from 'axios'

import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default { 
  data() {
    return {
      messages: [],
      form: {
        textMessage: ''
      }
    }
  },
  methods: {
    async loadMessages() {
      try {
        let response = await axios.get(store.getters.URLS.API_URL + "messages/getMessages")
        console.log(response.data.messages)
        this.messages = response.data.messages
      } catch (err) {
        window.alert(err)
        console.log(err)
      }
    },
    initSignalR() {
      this.connection = new HubConnectionBuilder()
      .withUrl(window.location.origin + '/signalr-hub')
      .configureLogging(LogLevel.Information)
      .build();

      this.connection.onclose(() => {
        this.connectToSignalR();
      })
      this.connectToSignalR();
      this.connection.on('RefreshEvent', (data) => {
        this.messages.push({ text: data.value.text, date: data.value.date })
      })
      this.connection.on('OnConnectedAsync', (data) => {
        this.messages.push({ text: data, date: (new Date()).toLocaleString() })
      })
      this.connection.on('OnDisconnectedAsync', (data) => {
        this.messages.push({ text: data, date: (new Date()).toLocaleString() })
      })
    },
    connectToSignalR() {
      this.connection.start().catch(err => {
        console.error('Failed to connect with hub', err)
        return new Promise((resolve, reject) =>
          setTimeout(() => this.connectToSignalR().then(resolve).catch(reject), 5000))
      })
    },
    closeConnectionSR() {
      if (!this.connection) return;
      this.connection.off('RefreshEvent');
      this.connection = null;
    },
    async sendMessage() {
      await axios.post(store.getters.URLS.API_URL + "Messages/SendMessage", {
        text: this.form.textMessage
      })
      .then(request => {
        console.log(request)
        if (request.status == 200) {
          this.form.textMessage = '';
        }
      });
    }
  },
  created() {
    this.initSignalR()
  }
}
</script>
<style>  
.submit-button {
    padding: .5rem .75rem;
    margin-left: 10px;
    border: 1px solid #dee2e6;
    border-radius: 10px;
    width: 150px;
}
.message-input {
    padding: .5rem .75rem;
    color: #007bff;
    border: 1px solid #dee2e6;
    border-radius: 10px;
    width: 700px;
    height: 50px;
}
</style>