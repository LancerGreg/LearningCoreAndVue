<template>
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Sign In</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <form>
        <v-layout column>
          <v-flex>
            <v-text-field
              v-model="SignInUser.Email"
              name="email"
              label="Email"
              id="email"
              type="email"
              required></v-text-field>
          </v-flex>
          <v-flex>
            <v-text-field
              v-model="SignInUser.Password"
              name="password"
              label="Password"
              id="password"
              type="password"
              required></v-text-field>
          </v-flex>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap" mt-5>
            <v-btn color="primary" @click="toSignIn">Sign In</v-btn>
            <OnForggotPassword />
            <OnSignUp />
          </v-flex>
        </v-layout>
      </form>
    </v-flex>
  </v-layout>
</v-container>
</template>

<script>
import store from '../../store'
import axios from 'axios'
import OnForggotPassword from '../account/buttons/onForggotPassword.vue'
import OnSignUp from '../account/buttons/onSignUp.vue'

export default {
  data: () => {
      return {
        SignInUser: {
          Email: "",
          Password: ""
        }
      }
  },
  methods: {
    toSignIn () {
      axios.post(store.getters.URLS.API_URL + "auth/sign_in", {
        Email: this.SignInUser.Email,
        Password: this.SignInUser.Password,
        RememberMe: false,
        ReturnUrl: ""
      })
      .then((response) => {
        response;
        store.dispatch('SET_USERISAUTHORITED');
        store.getters.USERISAUTHORITED;
      }).catch(error => {
        error.response.data.forEach(element => {
          alert(element.Code + "\n" + element.Description)
        });
      });
    }
  },
  components: {
    OnForggotPassword,
    OnSignUp
  }
}
</script>
