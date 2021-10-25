<template>
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Forgot password</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <validation-observer>
        <form @submit.prevent="reserPassword">
          <validation-provider v-slot="{ errors }" name="email" rules="required|email">
            <v-card-text>
              <v-text-field type="email" v-model="ReserPasswordRequest.Email" :error-messages="errors" label="Email" required></v-text-field>
            </v-card-text>
          </validation-provider>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap pl-4 pr-4" mt-5>
            <Loader v-if="loader" :loaderPerams="loaderPerams" />
            <v-btn v-else type="submit" color="primary">Send request ot reset password</v-btn>
            <OnSignIn />
          </v-flex>
          <ResponseDialog ref="responseDialog"/>
        </form>
      </validation-observer>
    </v-flex>
  </v-layout>
</v-container>
</template>

<script>
import store from '../../store'
import axios from 'axios'
import Loader from "../loader/loader.vue"
import ResponseDialog from "../responseDialog/responseDialog.vue"

import OnSignIn from '../account/buttons/onSignIn.vue'

import { required, email, min } from 'vee-validate/dist/rules'
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
extend('email', {
  ...email,
  message: 'Email must be valid',
})

export default {
  data: () => {
    return {
      ReserPasswordRequest: {
        Email: ""
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
      axios.post(store.getters.URLS.API_URL + "auth/forgot_password_request?email=" + this.ReserPasswordRequest.Email)
      .then(success => {
        this.$refs.responseDialog.showSuccessResponse(success)
      }).catch(error => {
        this.$refs.responseDialog.showErrorResponse(error)
      }).finally(() => this.loader = false);
    }
  },
  components: {
    OnSignIn,
    Loader,
    ResponseDialog,
    ValidationProvider,
    ValidationObserver,
  }
}
</script>
