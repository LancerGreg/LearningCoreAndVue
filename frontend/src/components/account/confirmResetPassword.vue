<template>    
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Reset Passwrod</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <form>
        <v-layout column>
          <v-flex>
            <v-text-field
              v-model="ReserPassword.NewPassword"
              name="NewPassword"
              label="NewPassword"
              id="NewPassword"
              type="password"
              required></v-text-field>
          </v-flex>
          <v-flex>
            <v-text-field
              v-model="ReserPassword.ConfirmPassword"
              name="ConfirmPassword"
              label="ConfirmPassword"
              id="ConfirmPassword"
              type="password"
              required></v-text-field>
          </v-flex>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap" mt-5>
            <Loader v-if="loader" :loaderPerams="loaderPerams" />
            <v-btn v-else color="primary" @click="reserPassword">save</v-btn>
          </v-flex>
          <ResponseDialog ref="responseDialog"/>
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
import Loader from "../loader/loader.vue"
import ResponseDialog from "../responseDialog/responseDialog.vue"

export default {
  data: () => {
    return {
      ReserPassword: {
        NewPassword: "",
        ConfirmPassword: ""
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
    reserPassword() {
      if (this.ReserPassword.NewPassword !== this.ReserPassword.ConfirmPassword) {          
        alert("ERROR\nNewPassword not equals with ConfirmPassword")
      } else {
        this.loader = true
        let uri = window.location.search.substring(1); 
        let params = new URLSearchParams(uri);
        let email = params.get("email");
        let token = params.get("token");

        axios.post(store.getters.URLS.API_URL + "auth/confirm_reset_password?email=" + email + "&passwrod=" + this.ReserPassword.NewPassword + "&token=" + token)
        .then(() => {
          alert("Password change");
          axios.post(store.getters.URLS.API_URL + "auth/sign_in", {
            Email: email,
            Password: this.ReserPassword.NewPassword,
            RememberMe: false,
            ReturnUrl: ""
          })
          .then((response) => {
            console.log(response);
            store.dispatch('SET_USERISAUTHORITED');
            store.getters.USERISAUTHORITED;
            router.push({ name: "Account"})
          }).catch(error => {
            this.$refs.responseDialog.showErrorResponse(error)
          })
        }).catch(error => {
          this.$refs.responseDialog.showErrorResponse(error)
        }).finally(() => this.loader = false);
      }
    },
  },
  components: {
    Loader,
    ResponseDialog
  }
}
</script>
