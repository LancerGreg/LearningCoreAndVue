<template>
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Forgot password</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <form>
        <v-layout column>
          <v-flex>
            <v-text-field
              v-model="ReserPasswordRequest.Email"
              name="email"
              label="Email"
              id="email"
              type="email"
              required></v-text-field>
          </v-flex>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap" mt-5>
            <v-btn color="primary" @click="reserPassword">Send request ot reset password</v-btn>
            <OnSignIn />
          </v-flex>
        </v-layout>
      </form>
    </v-flex>
  </v-layout>
</v-container>
</template>

<script>
import router from '../../router'
import store from '../../store'
import axios from 'axios'

import OnSignIn from '../account/buttons/onSignIn.vue'

export default {
  data: () => {
    return {
      ReserPasswordRequest: {
        Email: ""
      }
    }
  },
  methods: {
    reserPassword() {
      axios.post(store.getters.URLS.API_URL + "auth/forgot_password_request?email=" + this.ReserPasswordRequest.Email)
      .then(() => {
        alert("Forgot password request send to your email");
        router.push({ name: "Home"})
      }).catch(error => {
        console.log(error);
        alert("ERROR\nEmail not found or not confirmed")
      });
    }
  },
  components: {
    OnSignIn
  }
}
</script>
