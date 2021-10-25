<template>    
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Reset Passwrod</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <validation-observer>
        <form @submit.prevent="reserPassword">
            <validation-provider v-slot="{ errors }" name="password" rules="required|min:8">
              <v-card-text>
                <v-text-field type="password" v-model="ReserPassword.NewPassword" :error-messages="errors" label="Password" required></v-text-field>
              </v-card-text>
            </validation-provider>
            <validation-provider v-slot="{ errors }" name="confirmPassword" rules="required|min:8">
              <v-card-text>
                <v-text-field type="password" v-model="ReserPassword.ConfirmPassword" :error-messages="errors" label="Confirm password" required></v-text-field>
              </v-card-text>
            </validation-provider>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap pl-4 pr-4" mt-5>
            <Loader v-if="loader" :loaderPerams="loaderPerams" />
            <v-btn v-else type="submit" color="primary">save</v-btn>
          </v-flex>
          <ResponseDialog ref="responseDialog"/>
        </form>
      </validation-observer>
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

import { required, min } from 'vee-validate/dist/rules'
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from 'vee-validate'
setInteractionMode('eager')
extend('required', {
  ...required,
  message: '{_field_} can not be empty',
})
extend('min', {
  ...min,
  message: '{_field_} may be greater than {length} characters',
})

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
  components: {
    Loader,
    ResponseDialog,
    ValidationProvider,
    ValidationObserver,
  }
}
</script>
