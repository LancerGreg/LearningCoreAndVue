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
              <v-btn color="primary" @click="toSignUn">Sign Up</v-btn>
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
import OnSignIn from '../account/buttons/onSignIn.vue'

export default {
  data: () => {
      return {
        SignUpUser: {
          Email: "",
          Password: "",
          PasswordConfirm: ""
        }
      }
  },
  methods: {
    toSignUn() {
      axios.post(store.getters.URLS.API_URL + "Account/SignUp", {
        Email: this.SignUpUser.Email,
        Password: this.SignUpUser.Password,
        PasswordConfirm: this.SignUpUser.PasswordConfirm
      })
      .then((response) => {
          alert("User alredy exist")
        if(response == 409) {
          alert("User alredy exist")
        }
        if(response == 200) {
          alert("Success")
        }
      })
    }
  },
  components: {
    OnSignIn
  }
}
</script>
