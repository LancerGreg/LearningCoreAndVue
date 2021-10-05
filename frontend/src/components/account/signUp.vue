<template>
  <v-container fluid>
    <v-layout row wrap>
      <v-flex xs12 class="text-xs-center" mt-5>
        <h1>Sign Up</h1>
      </v-flex>
      <v-flex xs12 sm6 offset-sm3 mt-3>
        <form>
          <v-layout column>
            <v-flex>
              <v-text-field
                v-model="SignUpUser.Email"
                name="email"
                label="Email"
                id="email"
                type="email"
                required></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field
                v-model="SignUpUser.Password"
                name="password"
                label="Password"
                id="password"
                type="password"
                required></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field
                v-model="SignUpUser.PasswordConfirm"
                name="confirmPassword"
                label="Confirm Password"
                id="confirmPassword"
                type="password"
                required
                ></v-text-field>
            </v-flex>
            <v-flex class="text-xs-center justify-content-between flex-flow-wrap" mt-5>
              <Loader v-if="loader" :loaderPerams="loaderPerams" />
              <v-btn v-else color="primary" @click="toSignUn">Sign Up</v-btn>
              <OnSignIn />
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
import router from '../../router'
import Loader from "../loader/loader.vue"

import OnSignIn from '../account/buttons/onSignIn.vue'

export default {
  data: () => {
      return {
        SignUpUser: {
          Email: "",
          Password: "",
          PasswordConfirm: ""
        },
        loader: false,
        loaderPerams: {
          size: 40,
          color: "#1976d2",
          width: 5
        },
      }
  },
  methods: {
    toSignUn() {
      if (this.SignUpUser.Password == this.SignUpUser.PasswordConfirm) {
        this.loader = true
        axios.post(store.getters.URLS.API_URL + "auth/sign_up", {
          email: this.SignUpUser.Email,
          password: this.SignUpUser.Password,
          passwordConfirm: this.SignUpUser.PasswordConfirm
        })
        .then(() => {
          alert("Thank you for sign in\n\nCheck your email and confirm registration");
          router.go(store.getters.URLS.API_URL + "account")
        }).catch(error => {
          error.response.data.forEach(element => {
            alert(element.Code + "\n" + element.Description)
          });
        }).finally(() => this.loader = false);
      } else {
        alert("Not correct passwrod" + "\n" + "Your password and confirmation password do not match.")
      }
    }
  },
  components: {
    OnSignIn,
    Loader
  }
}
</script>
