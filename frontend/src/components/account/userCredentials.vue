<template>
    <div>
    <v-container>
      <h2>Welcome <strong>{{userProfile.FirstName}}</strong>!</h2>      
      <v-card class="overflow-hidden" color="grey darken-1" dark>        
        <v-toolbar flat color="purple">
          <v-dialog v-model="dialogConfirmToken" max-width="500px">
            <v-card>
              <v-card-title>
                <span class="v-verification-text text-h5">To verify your phone number, enter the verification code that was sent to this number.</span>
              </v-card-title>
              <v-text-field class="pl-24 pr-24" color="white" label="Verify change phone number code" v-model="token"></v-text-field>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="confirmToken">
                  Check verify code
                </v-btn>
              </v-card-actions>
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
        <v-divider></v-divider>
        <v-card-text>
          <v-text-field :disabled="!isEditing" color="white" label="First Name" v-model="userProfile.FirstName"></v-text-field>
          <v-text-field :disabled="!isEditing" color="white" label="Last Name" v-model="userProfile.LastName"></v-text-field>
          <v-text-field :disabled="!isEditing" color="white" label="Phone" v-model="userProfile.Phone" type="phone"></v-text-field>
          <v-text-field :disabled="!isEditing" color="white" label="Password" v-model="userProfile.Password" type="password"></v-text-field>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn :disabled="!isEditing" color="success" @click="save">
            Save
          </v-btn>
        </v-card-actions>
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
    ResponseDialog
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