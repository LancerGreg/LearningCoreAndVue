<template>
  <div class="messanger-container">
    <div class="window-container">
      <div class="vac-card-window">
        <div class="vac-chat-container">
          <div v-show="chatOptions.expandChat" class="vac-rooms-container vac-app-border-r">
            <div class="vac-box-search">
              <div class="vac-icon-search">
                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24"><path id="vac-icon-search" d="M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z"></path></svg>
              </div>
              <input v-model.lazy="chatOptions.searchChat" type="search" placeholder="Search" autocomplete="off" class="vac-input">
              <div @click="openAddNewChat" class="vac-svg-button vac-add-icon">
                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24"><path id="vac-icon-add" d="M17,13H13V17H11V13H7V11H11V7H13V11H17M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z"></path></svg>
              </div>
            </div>
            <div id="rooms-list" class="vac-room-list">
              <div v-if="addNewChat.display" id="add-new-room" class="vac-room-item">
                <div class="vac-add-new-room-form">
                  <input v-model="addNewChat.chatName" type="text" placeholder="Chat name">
                  <button @click="createNewChat" type="submit" :disabled="addNewChat.chatName == ''">Create Chat</button>
                  <button @click="closeAddNewChat" class="button-cancel">Cancel</button>
                </div>
              </div>
              <div v-for="dataChat in listChats" :key="dataChat.chat.id" id="ZBRmBRMngRGSM1k0fYdR" @click="openChat(dataChat)" class="vac-room-item">
                <div class="vac-room-container">
                  <div class="vac-avatar">
                  </div>
                  <div class="vac-name-container vac-text-ellipsis">
                    <div class="vac-title-container">
                      <div class="vac-room-name vac-text-ellipsis">{{ dataChat.chat.Name }}</div>
                      <div class="vac-text-date">{{ dataChat.lastMessage.dateSend }}</div>
                    </div>
                    <div class="vac-text-last">
                      <div class="vac-format-message-wrapper vac-text-ellipsis">
                        <div class="vac-text-ellipsis">
                          <div class="vac-format-container">
                            <span class="vac-text-ellipsis">
                              <span>{{ dataChat.lastMessage.text }}</span>
                            </span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="vac-col-messages">
            <div class="vac-room-header vac-app-border-b">
              <div class="vac-room-wrapper">
                <div @click="expandChat" class="vac-svg-button vac-toggle-button" v-bind:class="{ 'vac-rotate-icon': !chatOptions.expandChat }">
                  <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24">
                    <path id="vac-icon-toggle" d="M5,13L9,17L7.6,18.42L1.18,12L7.6,5.58L9,7L5,11H21V13H5M21,6V8H11V6H21M21,16V18H11V16H21Z"></path>
                  </svg>
                </div>
                <div class="vac-info-wrapper">
                  <div class="vac-avatar"></div>
                  <div class="vac-text-ellipsis">
                    <div class="vac-room-name vac-text-ellipsis"> ,mk,mkknk </div>
                    <div class="vac-room-info vac-text-ellipsis"></div>
                  </div>
                </div>
                <div v-click-outside="closeChatMethods" @click="openChatMethods" class="vac-svg-button vac-room-options">
                  <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24">
                    <path id="vac-icon-menu" d="M12,16A2,2 0 0,1 14,18A2,2 0 0,1 12,20A2,2 0 0,1 10,18A2,2 0 0,1 12,16M12,10A2,2 0 0,1 14,12A2,2 0 0,1 12,14A2,2 0 0,1 10,12A2,2 0 0,1 12,10M12,4A2,2 0 0,1 14,6A2,2 0 0,1 12,8A2,2 0 0,1 10,6A2,2 0 0,1 12,4Z"></path>
                  </svg>
                </div>
                <div v-if="chatOptions.chatMethods" class="vac-menu-options">
                  <div class="vac-menu-list">
                    <div>
                      <div @click="showInviteUserMenu" class="vac-menu-item">Invite User</div>
                    </div>
                    <div>
                      <div @click="showDeleteUserMenu" class="vac-menu-item">Remove User</div>
                    </div>
                    <div>
                      <div @click="deleteChat" class="vac-menu-item">Delete Chat</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="vac-container-scroll">
              <div class="vac-messages-container">
                <div>
                  <div>
                    <div class="vac-text-started"> Conversation started on: 17 October 2021 </div>
                  </div>
                  <div class="infinite-loading-container">
                    <div class="infinite-status-prompt">
                      <div class="vac-loader-wrapper vac-container-top">
                        <div id="vac-circle"></div>
                      </div>
                    </div>
                  </div>
                  <span>
                    <div>
                      <div id="f0eLbI3T3EU8tR3bgxy9" class="vac-message-wrapper">
                        <div class="vac-message-box vac-offset-current">
                          <div class="vac-message-container vac-message-container-offset">
                            <div class="vac-message-card vac-message-current">
                              <div class="vac-format-message-wrapper">
                                <div class="">
                                  <div class="vac-format-container">
                                    <span class="">
                                      <span>hello</span>
                                    </span>
                                  </div>
                                </div>
                              </div>
                              <div class="vac-text-timestamp">
                                <span>08:29</span>
                                <span>
                                  <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24" class="vac-icon-check">
                                    <path id="vac-icon-double-checkmark-seen" d="M18 7l-1.41-1.41-6.34 6.34 1.41 1.41L18 7zm4.24-1.41L11.66 16.17 7.48 12l-1.41 1.41L11.66 19l12-12-1.42-1.41zM.41 13.41L6 19l1.41-1.41L1.83 12 .41 13.41z"></path>
                                  </svg>
                                </span>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div>
                      <div id="yv9tAFPn9NCdjvb1t2cj" class="vac-message-wrapper">
                        <div class="vac-message-box">
                          <div class="vac-message-container vac-message-container-offset">
                            <div class="vac-message-card">
                              <div class="vac-format-message-wrapper">
                                <div class="">
                                  <div class="vac-format-container">
                                    <span class="">
                                      <span>hii</span>
                                    </span>
                                  </div>
                                </div>
                              </div>
                              <div class="vac-text-timestamp">
                                <span>09:54</span>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </span>
                </div>
              </div>
            </div>
            <div class="vac-box-footer">
              <textarea v-model="messageOptions.textMessage" placeholder="Type message" class="vac-textarea" style="min-height: 20px; padding-left: 12px; height: 20px;"></textarea>
              <div class="vac-icon-textarea">
                <div @click="sendMessage" class="vac-svg-button" v-bind:class="{ 'vac-send-disabled': messageOptions.textMessage == '' }">
                  <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="24" height="24" viewBox="0 0 24 24">
                    <path v-bind:id="[messageOptions.textMessage == '' ? 'vac-icon-send-disabled' : 'vac-icon-send']" d="M2,21L23,12L2,3V10L17,12L2,14V21Z"></path>
                  </svg>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
      <!-- Exapmle used web socket
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
            <input v-model="form.chatId" class="message-input" type="text" placeholder="Type a chad id here">
            <input v-model="form.textMessage" class="message-input" type="text" placeholder="Type a message here">
            <button class="dark-bg text-white submit-button" @click.prevent="sendMessage">Submit</button>
          </div>
        </div>
      </section> -->
  </div>
</template>
 
<script>
import router from "../router"
import store from "../store"
import axios from 'axios'

export default { 
  data() {
    return {
      listChats: [],
      chatOptions: {
        searchChat: "",
        chatMethods: false,
        expandChat: true,
      },
      addNewChat: {
        display: false,
        chatName: "",
      },
      messageOptions: {
        textMessage: "",
      }
    }
  },
  methods: {
    // TODO: open selected chat
    openChat(dataChat) { },

    // TODO: create a request to create a new chat 
    // after creating put a new chat at the top of the chat list 
    createNewChat() { },

    openAddNewChat() { this.addNewChat.display = true },
    closeAddNewChat() { this.addNewChat.display = false },
    openChatMethods() { this.chatOptions.chatMethods = true },
    closeChatMethods() { this.chatOptions.chatMethods = false },

    expandChat() { this.chatOptions.expandChat = !this.chatOptions.expandChat },

    // TODO: create menu bar with search user and with function of invite new user to the chat
    showInviteUserMenu(dataChat) { },

    // TODO: create menu bar with search user and with function of delete from this chat
    showDeleteUserMenu(dataChat) { },

    // TODO: create a request to delete this chat
    // after deleting remove from this chat list 
    deleteChat(dataChat) { },

    // TODO: send message to server
    // after sending put a new message at the bottom of the message list 
    // implemented WebSocket with sending
    sendMessage() { },

    async getChatsList() {
      await axios.get(store.getters.URLS.API_URL + "chat/get_chats_by_current_user")
      .then((response) => {
        this.listChats = response.data
      }).catch(() => {
        router.push({ name: "Error_500"})
      });
    },
  },
  async mounted() {
    await this.getChatsList();
  }
}

// Exapmle used web socket

// import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

// export default { 
//   data() {
//     return {
//       messages: [],
//       form: {
//         chatId: '',
//         textMessage: ''
//       }
//     }
//   },
//   methods: {
//     async loadMessages() {
//       try {
//         let response = await axios.get(store.getters.URLS.API_URL + "messages/getMessages")
//         console.log(response.data.messages)
//         this.messages = response.data.messages
//       } catch (err) {
//         window.alert(err)
//         console.log(err)
//       }
//     },
//     initSignalR() {
//       this.connection = new HubConnectionBuilder()
//       .withUrl(window.location.origin + '/signalr-hub')
//       .configureLogging(LogLevel.Information)
//       .build();

//       this.connection.onclose(() => {
//         this.connectToSignalR();
//       })
//       this.connectToSignalR();
//       this.connection.on('RefreshMessage', (data) => {
//         this.messages.push({ text: data.value.text, date: data.value.date })
//       })
//       this.connection.on('OnConnectedAsync', (data) => {
//         this.messages.push({ text: data, date: (new Date()).toLocaleString() })
//       })
//       this.connection.on('OnDisconnectedAsync', (data) => {
//         this.messages.push({ text: data, date: (new Date()).toLocaleString() })
//       })
//     },
//     connectToSignalR() {
//       this.connection.start().catch(err => {
//         console.error('Failed to connect with hub', err)
//         return new Promise((resolve, reject) =>
//           setTimeout(() => this.connectToSignalR().then(resolve).catch(reject), 5000))
//       })
//     },
//     closeConnectionSR() {
//       if (!this.connection) return;
//       this.connection.off('RefreshEvent');
//       this.connection = null;
//     },
//     async sendMessage() {
//       await axios.post(store.getters.URLS.API_URL + "chat/send_message?chatId=" + this.form.chatId + "&textMessage=" + this.form.textMessage)
//       .then(request => {
//         console.log(request)
//         if (request.status == 200) {
//           this.form.chatId = '';
//           this.form.textMessage = '';
//         }
//       });
//     }
//   },
//   created() {
//     this.initSignalR()
//   }
// }
</script>
<style>
.container {
  padding: 0;
}

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

<style scoped>
* {
  --chat-color: #0a0a0a;
  --chat-bg-color-input: #fff;
  --chat-color-spinner: #333;
  --chat-color-placeholder: #9ca6af;
  --chat-color-caret: #1976d2;
  --chat-border-style: 1px solid #e1e4e8;
  --chat-bg-scroll-icon: #fff;
  --chat-container-border: none;
  --chat-container-border-radius: 4px;
  --chat-container-box-shadow: 0px 1px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12);
  --chat-header-bg-color: #fff;
  --chat-header-color-name: #0a0a0a;
  --chat-header-color-info: #9ca6af;
  --chat-footer-bg-color: #f8f9fa;
  --chat-border-style-input: 1px solid #e1e4e8;
  --chat-border-color-input-selected: #1976d2;
  --chat-footer-bg-color-reply: #e5e5e6;
  --chat-footer-bg-color-tag-active: #e5e5e6;
  --chat-footer-bg-color-tag: #f8f9fa;
  --chat-content-bg-color: #f8f9fa;
  --chat-sidemenu-bg-color: #fff;
  --chat-sidemenu-bg-color-hover: #f6f6f6;
  --chat-sidemenu-bg-color-active: #e5effa;
  --chat-sidemenu-color-active: #1976d2;
  --chat-sidemenu-border-color-search: #e1e5e8;
  --chat-dropdown-bg-color: #fff;
  --chat-dropdown-bg-color-hover: #f6f6f6;
  --chat-message-bg-color: #fff;
  --chat-message-bg-color-me: #ccf2cf;
  --chat-message-color-started: #9ca6af;
  --chat-message-bg-color-deleted: #dadfe2;
  --chat-message-color-deleted: #757e85;
  --chat-message-color-username: #9ca6af;
  --chat-message-color-timestamp: #828c94;
  --chat-message-bg-color-date: #e5effa;
  --chat-message-color-date: #505a62;
  --chat-message-bg-color-system: #e5effa;
  --chat-message-color-system: #505a62;
  --chat-message-color: #0a0a0a;
  --chat-message-bg-color-media: rgba(0, 0, 0, 0.15);
  --chat-message-bg-color-reply: rgba(0, 0, 0, 0.08);
  --chat-message-color-reply-username: #0a0a0a;
  --chat-message-color-reply-content: #6e6e6e;
  --chat-message-color-tag: #0d579c;
  --chat-message-bg-color-image: #ddd;
  --chat-message-color-new-messages: #1976d2;
  --chat-message-bg-color-scroll-counter: #0696c7;
  --chat-message-color-scroll-counter: #fff;
  --chat-message-bg-color-reaction: #eee;
  --chat-message-border-style-reaction: 1px solid #eee;
  --chat-message-bg-color-reaction-hover: #fff;
  --chat-message-border-style-reaction-hover: 1px solid #ddd;
  --chat-message-color-reaction-counter: #0a0a0a;
  --chat-message-bg-color-reaction-me: #cfecf5;
  --chat-message-border-style-reaction-me: 1px solid #3b98b8;
  --chat-message-bg-color-reaction-hover-me: #cfecf5;
  --chat-message-border-style-reaction-hover-me: 1px solid #3b98b8;
  --chat-message-color-reaction-counter-me: #0b59b3;
  --chat-message-bg-color-audio-record: #eb4034;
  --chat-message-bg-color-audio-line: rgba(0, 0, 0, 0.15);
  --chat-message-bg-color-audio-progress: #455247;
  --chat-message-bg-color-audio-progress-selector: #455247;
  --chat-message-color-file-extension: #757e85;
  --chat-markdown-bg: rgba(239, 239, 239, 0.7);
  --chat-markdown-border: rgba(212, 212, 212, 0.9);
  --chat-markdown-color: #e01e5a;
  --chat-markdown-color-multi: #0a0a0a;
  --chat-room-color-username: #0a0a0a;
  --chat-room-color-message: #67717a;
  --chat-room-color-timestamp: #a2aeb8;
  --chat-room-color-online: #4caf50;
  --chat-room-color-offline: #9ca6af;
  --chat-room-bg-color-badge: #0696c7;
  --chat-room-color-badge: #fff;
  --chat-emoji-bg-color: #fff;
  --chat-icon-color-search: #9ca6af;
  --chat-icon-color-add: #1976d2;
  --chat-icon-color-hover-add: #d21922;
  --chat-icon-color-toggle: #0a0a0a;
  --chat-icon-color-menu: #0a0a0a;
  --chat-icon-color-close: #9ca6af;
  --chat-icon-color-close-image: #fff;
  --chat-icon-color-file: #1976d2;
  --chat-icon-color-paperclip: #1976d2;
  --chat-icon-color-close-outline: #000;
  --chat-icon-color-send: #1976d2;
  --chat-icon-color-send-disabled: #9ca6af;
  --chat-icon-color-emoji: #1976d2;
  --chat-icon-color-emoji-reaction: rgba(0, 0, 0, 0.3);
  --chat-icon-color-document: #1976d2;
  --chat-icon-color-pencil: #9e9e9e;
  --chat-icon-color-checkmark: #9e9e9e;
  --chat-icon-color-checkmark-seen: #0696c7;
  --chat-icon-color-eye: #fff;
  --chat-icon-color-dropdown-message: #fff;
  --chat-icon-bg-dropdown-message: rgba(0, 0, 0, 0.25);
  --chat-icon-color-dropdown-room: #9e9e9e;
  --chat-icon-color-dropdown-scroll: #0a0a0a;
  --chat-icon-color-microphone: #1976d2;
  --chat-icon-color-audio-play: #455247;
  --chat-icon-color-audio-pause: #455247;
  --chat-icon-color-audio-cancel: #eb4034;
  --chat-icon-color-audio-confirm: #1ba65b;
}

.messanger-container {
  font-family: Quicksand,sans-serif;
  padding: 20px 30px 30px;
}

.window-container {
  width: 100%;
}

.vac-card-window {
  height: calc(100vh - 80px - 64px);
  width: 100%;
  display: block;
  max-width: 100%;
  background: var(--chat-content-bg-color);
  color: var(--chat-color);
  overflow-wrap: break-word;
  position: relative;
  white-space: normal;
  border: var(--chat-container-border);
  border-radius: var(--chat-container-border-radius);
  box-shadow: var(--chat-container-box-shadow);
  -webkit-tap-highlight-color: transparent;
}

.vac-chat-container {
  height: 100%;
  display: flex;
}

.vac-rooms-container {
  display: flex;
  flex-flow: column;
  flex: 0 0 25%;
  min-width: 260px;
  max-width: 500px;
  position: relative;
  background: var(--chat-sidemenu-bg-color);
  height: 100%;
  border-top-left-radius: var(--chat-container-border-radius);
  border-bottom-left-radius: var(--chat-container-border-radius);
}

.vac-app-border-r {
  border-right: var(--chat-border-style);
}

.vac-box-search {
  position: sticky;
  display: flex;
  align-items: center;
  height: 64px;
  padding: 0 15px;
}

.vac-icon-search {
  display: flex;
  position: absolute;
  left: 30px;
}

#vac-icon-search {
  fill: var(--chat-icon-color-search);
}

.vac-input {
  height: 38px;
  width: 100%;
  background: var(--chat-bg-color-input);
  color: var(--chat-color);
  font-size: 15px;
  outline: 0;
  caret-color: var(--chat-color-caret);
  padding: 10px 10px 10px 40px;
  border: 1px solid var(--chat-sidemenu-border-color-search);
  border-radius: 20px;
}

.vac-svg-button {
  max-height: 30px;
  display: flex;
  cursor: pointer;
  transition: all .2s;
}

.vac-svg-button:hover {
    transform: scale(1.1);
    opacity: .7;
}

.vac-add-icon {
  margin-left: auto;
  padding-left: 10px;
}

#vac-icon-add {
  fill: var(--chat-icon-color-add);
}

.vac-room-list {
  flex: 1;
  position: relative;
  max-width: 100%;
  cursor: pointer;
  padding: 0 10px 5px;
  overflow-y: auto;
}

.vac-room-item {
  border-radius: 8px;
  align-items: center;
  display: flex;
  flex: 1 1 100%;
  margin-bottom: 5px;
  padding: 0 14px;
  position: relative;
  min-height: 71px;
  transition: background-color .3s cubic-bezier(.25,.8,.5,1);
}

.vac-rooms-container .vac-room-item:hover {
  background: var(--chat-sidemenu-bg-color-hover);
}

.vac-rooms-container .vac-room-selected, .vac-rooms-container .vac-room-selected:hover {
  background: var(--chat-sidemenu-bg-color-active) !important;
}

.vac-rooms-container .vac-room-selected {
  color: var(--chat-sidemenu-color-active) !important;
}

#add-new-room {
  cursor: auto;
}

.vac-add-new-room-form {
  width: 100%;
  padding: 5px 0;
  display: flex;
  border-top: 1px solid black;
  border-bottom: 1px solid black;
}

input {
  padding: 5px;
  border-radius: 4px;
  border: 1px solid #d2d6da;
  outline: none;
  font-size: 14px;
  vertical-align: middle;
}

button {
  background: #1976d2;
  color: #fff;
  outline: none;
  cursor: pointer;
  border-radius: 4px;
  padding: 8px 12px;
  margin-left: 10px;
  border: none;
  font-size: 14px;
  transition: .3s;
  vertical-align: middle;
}

button:disabled {
  cursor: auto;
  background: #c6c9cc;
  opacity: .6;
}

.button-cancel {
  color: #a8aeb3;
  background: none;
  margin: 0 0 0 auto;
}

.vac-room-container {
  display: flex;
  flex: 1;
  align-items: center;
  width: 100%;
}

.vac-avatar {
  background: url(../assets/images/avatars/EmtyAvatar_40x40.png);
  background-size: cover;
  background-position: 50%;
  background-repeat: no-repeat;
  background-color: #ddd;
  height: 42px;
  width: 42px;
  min-height: 42px;
  min-width: 42px;
  margin-right: 15px;
  border-radius: 50%;
}

.vac-name-container {
  flex: 1;
}

.vac-text-ellipsis {
  width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.vac-title-container {
  display: flex;
  align-items: center;
  line-height: 25px;
}

.vac-room-name {
  flex: 1;
  color: var(--chat-room-color-username);
  font-weight: 500;
}

.vac-text-date {
  margin-left: 5px;
  font-size: 11px;
  color: var(--chat-room-color-timestamp);
}

.vac-text-last {
  display: flex;
  align-items: center;
  font-size: 12px;
  line-height: 19px;
  color: var(--chat-room-color-message);
}

.vac-room-options-container {
  display: flex;
  margin-left: auto;
}

.vac-list-room-options {
  height: 19px;
  width: 19px;
  align-items: center;
  margin-left: 5px;
}

#vac-icon-dropdown-room {
  fill: var(--chat-icon-color-dropdown-room);
}

.vac-menu-options {
  position: absolute;
  right: 10px;
  top: 20px;
  z-index: 9999;
  min-width: 150px;
  display: inline-block;
  border-radius: 4px;
  font-size: 14px;
  color: var(--chat-color);
  overflow-y: auto;
  overflow-x: hidden;
  contain: content;
  box-shadow: 0 2px 2px -4px rgba(0,0,0,.1),0 2px 2px 1px rgba(0,0,0,.12),0 1px 8px 1px rgba(0,0,0,.12);
}

.vac-menu-list {
  border-radius: 4px;
  display: block;
  cursor: pointer;
  background: var(--chat-dropdown-bg-color);
  padding: 6px 0;
}

.vac-menu-list :hover, .vac-menu-list :not(:hover) {
    transition: background-color .3s cubic-bezier(.25,.8,.5,1);
}

.vac-menu-list :hover {
    background: var(--chat-dropdown-bg-color-hover);
}

.vac-menu-item {
  align-items: center;
  display: flex;
  flex: 1 1 100%;
  min-height: 30px;
  padding: 5px 16px;
  position: relative;
  white-space: nowrap;
  line-height: 30px;
}

.vac-col-messages {
  position: relative;
  height: 100%;
  flex: 1;
  overflow: hidden;
  display: flex;
  flex-flow: column;
}

.vac-room-header {
  position: absolute;
  display: flex;
  align-items: center;
  height: 64px;
  width: 100%;
  z-index: 6;
  margin-right: 1px;
  background: var(--chat-header-bg-color);
  border-top-right-radius: var(--chat-container-border-radius);
}

.vac-app-border-b {
  border-bottom: var(--chat-border-style);
}

.vac-room-wrapper {
  display: flex;
  align-items: center;
  min-width: 0;
  height: 100%;
  width: 100%;
  padding: 0 16px;
}

.vac-toggle-button {
  margin-right: 15px;
}

.vac-room-header .vac-rotate-icon {
    transform: rotate(180deg) !important;
}

#vac-icon-toggle {
  fill: var(--chat-icon-color-toggle);
}

.vac-info-wrapper {
  display: flex;
  align-items: center;
  min-width: 0;
  width: 100%;
  height: 100%;
}

.vac-room-info {
  font-size: 13px;
  line-height: 18px;
  color: var(--chat-header-color-info);
}

.vac-room-options {
  margin-left: auto;
}

#vac-icon-menu {
  fill: var(--chat-icon-color-menu);
}

.vac-container-scroll {
  background: var(--chat-content-bg-color);
  flex: 1;
  overflow-y: auto;
  margin-right: 1px;
  margin-top: 60px;
  -webkit-overflow-scrolling: touch;
}

.vac-messages-container {
  padding: 0 5px 5px;
}

.vac-text-started {
  font-size: 14px;
  color: var(--chat-message-color-started);
  font-style: italic;
  text-align: center;
  margin-top: 30px;
  margin-bottom: 20px;
}

.infinite-loading-container {
  clear: both;
  text-align: center;
}

.infinite-status-prompt {
  text-align: center;
}

.vac-loader-wrapper {
  padding: 21px;
}

.vac-loader-wrapper .vac-container-top{
  padding: 21px;
}

.vac-loader-wrapper.vac-container-top #vac-circle {
    height: 20px;
    width: 20px;
}

.vac-loader-wrapper #vac-circle {
    margin: auto;
    height: 28px;
    width: 28px;
    border: 3px solid rgba(0,0,0,.25);
    border-top: 3px var(--chat-color-spinner) solid;
    border-right: 3px var(--chat-color-spinner) solid;
    border-bottom: 3px var(--chat-color-spinner) solid;
    border-radius: 50%;
    -webkit-animation: vac-spin 1s linear infinite;
    animation: vac-spin 1s linear infinite;
}

@keyframes vac-spin {
  0% {
      transform: rotate(0deg);
  }
  100% {
      transform: rotate(359deg);
  }
}

.vac-col-messages .vac-box-footer {
  display: flex;
  position: relative;
  background: var(--chat-footer-bg-color);
  padding: 10px 8px 10px;
}

.vac-col-messages .vac-textarea {
    height: 20px;
    width: 100%;
    line-height: 20px;
    overflow: hidden;
    outline: 0;
    resize: none;
    border-radius: 20px;
    padding: 12px 16px;
    padding-left: 16px;
    box-sizing: content-box;
    font-size: 16px;
    background: var(--chat-bg-color-input);
    color: var(--chat-color);
    caret-color: var(--chat-color-caret);
    border: var(--chat-border-style-input);
}

.vac-col-messages .vac-icon-textarea {
    margin-left: 15px;
}

.vac-col-messages .vac-icon-textarea, .vac-col-messages .vac-icon-textarea-left {
    display: flex;
    align-items: center;
}

.vac-send-disabled {
    cursor: none !important;
    pointer-events: none !important;
    transform: none !important;
}

#vac-icon-send {
  fill: var(--chat-icon-color-send);
}

#vac-icon-send-disabled {
  fill: var(--chat-icon-color-send-disabled);
}

.vac-message-wrapper {
  font-family: inherit;
}

.vac-message-box {
  display: flex;
  flex: 0 0 50%;
  max-width: 50%;
  justify-content: flex-start;
  line-height: 1.4;
}

.vac-offset-current {
  margin-left: 50%;
  justify-content: flex-end;
}

.vac-message-container {
  position: relative;
  padding: 2px 10px;
  align-items: end;
  min-width: 100px;
  box-sizing: content-box;
}

.vac-message-current {
  background: var(--chat-message-bg-color-me) !important;
}

.vac-message-card {
  background: var(--chat-message-bg-color);
  color: var(--chat-message-color);
  border-radius: 8px;
  font-size: 14px;
  padding: 6px 9px 3px;
  white-space: pre-line;
  max-width: 100%;
  transition-property: box-shadow,opacity;
  transition: box-shadow .28s cubic-bezier(.4,0,.2,1);
  will-change: box-shadow;
  box-shadow: 0 1px 1px -1px rgba(0,0,0,.1),0 1px 1px -1px rgba(0,0,0,.11),0 1px 2px -1px rgba(0,0,0,.11);
}

.vac-format-container {
  display: inline;
}

.vac-text-timestamp {
  font-size: 10px;
  color: var(--chat-message-color-timestamp);
  text-align: right;
}

.vac-icon-check {
  height: 14px;
  width: 14px;
  vertical-align: middle;
  margin: -3px -3px 0 3px;
}



#vac-icon-double-checkmark-seen {
  fill: var(--chat-icon-color-checkmark-seen);
}

@media screen and (max-width: 900px) {
  .messanger-container {
    padding: 0;
  }

  .vac-col-messages {
    display: none;
  }
  
  .vac-rooms-container {
    flex: 0 0 100%;
    max-width: 100%;
  }
}
</style>