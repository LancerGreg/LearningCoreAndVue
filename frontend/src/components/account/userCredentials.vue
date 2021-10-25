<template>
    <div>
    <v-container>
      <h2>Welcome <strong>{{userProfile.FirstName}}</strong>!</h2>      
      <v-card class="overflow-hidden" color="grey darken-1" dark>        
        <v-toolbar flat color="purple">
          <v-dialog v-model="dialogConfirmToken" max-width="500px">
            <v-card>
              <validation-observer>
                <form @submit.prevent="confirmToken">
                  <validation-provider v-slot="{ errors }" name="VerifyCode" rules="required">
                    <v-card-title>
                      <span class="v-verification-text text-h5">To verify your phone number, enter the verification code that was sent to this number.</span>
                      <v-text-field v-model="token" class="pl-24 pr-24" color="white" label="Verify change phone number code" :error-messages="errors" required></v-text-field>
                    </v-card-title>
                  </validation-provider>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn type="submit" color="blue darken-1" text>
                      Check verify code
                    </v-btn>
                  </v-card-actions>
                </form>
              </validation-observer>
            </v-card>
          </v-dialog>
          <v-icon>mdi-account</v-icon>
          <v-toolbar-title class="font-weight-light">
            User Profile
          </v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn color="purple darken-3" fab small @click="isEditing = !isEditing">
            <v-icon v-if="isEditing">
              mdi-close
            </v-icon>
            <v-icon v-else>
              mdi-pencil
            </v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text>
          <v-text-field :disabled="true" color="white" label="Email" v-model="userProfile.Email" type="email"></v-text-field>
        </v-card-text>    
        <validation-observer>
          <form @submit.prevent="save">
            <v-divider></v-divider>
            <validation-provider v-slot="{ errors }" name="FirstName" rules="max:64">
              <v-card-text>
                <v-text-field :disabled="!isEditing" color="white" v-model="userProfile.FirstName" :counter="64" :error-messages="errors" label="First Name"></v-text-field>
              </v-card-text>
            </validation-provider>
            <validation-provider v-slot="{ errors }" name="LastName" rules="max:64">
              <v-card-text>
                <v-text-field :disabled="!isEditing" color="white" v-model="userProfile.LastName" :counter="64" :error-messages="errors" label="Last Name"></v-text-field>
              </v-card-text>
            </validation-provider>
            <validation-provider v-slot="{ errors }" name="Phone" rules="min:10">
              <v-card-text>
                <v-text-field :disabled="!isEditing" color="white" type="phone" v-model="userProfile.Phone" :error-messages="errors" label="Phone number"></v-text-field>
              </v-card-text>
            </validation-provider>
            <validation-provider v-slot="{ errors }" name="Password" rules="min:8">
              <v-card-text>
                <v-text-field :disabled="!isEditing" color="white" type="password" v-model="userProfile.Password" :error-messages="errors" label="Password"></v-text-field>
              </v-card-text>
            </validation-provider>
            <v-divider></v-divider>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn type="submit" :disabled="!isEditing" color="success">
                Save
              </v-btn>
            </v-card-actions>
          </form>
        </validation-observer>
        <v-snackbar v-model="hasSaved" :timeout="2000" absolute bottom left>
          Your profile has been updated
        </v-snackbar>
      </v-card>
    </v-container>
    <ResponseDialog ref="responseDialog"/>
  </div>
</template>

<script>
import store from "../../store"
import axios from 'axios'
import ResponseDialog from "../responseDialog/responseDialog.vue"

import { required, min, max  } from 'vee-validate/dist/rules'
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
extend('max', {
  ...max,
  message: '{_field_} may not be greater than {length} characters',
})

export default {
  data: () => {
      return {
        token: "",
        dialogConfirmToken: false,
        hasSaved: false,
        isEditing: null,
        model: null,
      }
  },
  computed: {
    userProfile () {
        return store.getters.USERPROFILE;
    },
  },
  methods: {
    save () {
      axios.post(store.getters.URLS.API_URL + "account/update_user", {
        FirstName: this.userProfile.FirstName,
        LastName: this.userProfile.LastName,
        Phone: this.userProfile.Phone,
        Email: this.userProfile.Email,
        Password: this.userProfile.Password
      })
      .then(response => {
        if (response.data.Message === "ResetNumberPhoneTokenSend") {
          this.dialogConfirmToken = true
        } else {
          this.isEditing = !this.isEditing
          this.hasSaved = true
        }
      }).catch(error => {
        this.$refs.responseDialog.showErrorResponse(error)
      });
    },
    async confirmToken() {
      axios.post(store.getters.URLS.API_URL + "auth/confirm_reset_number_phone?token=" + this.token)
      .then(() => {
        this.dialogConfirmToken = false
        this.isEditing = !this.isEditing
        this.hasSaved = true
      }).catch(error => {
        this.$refs.responseDialog.showErrorResponse(error)
      });
    }
  },
  mounted() {
    store.dispatch('SET_USERPROFILE');
  },
  components: {
    ResponseDialog,
    ValidationProvider,
    ValidationObserver,
  }
}
</script>

<style scoped>
  h2 {
    margin-bottom: 20px;
  }
  .v-verification-text {
    word-break: break-word;
  }
</style>