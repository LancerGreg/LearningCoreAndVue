<template>
  <v-row justify="space-around">
    <v-col cols="auto">
      <v-dialog v-model="response.showResponseDialog" transition="dialog-bottom-transition" max-width="600">
        <v-card>
          <v-toolbar v-bind:color="response.success ? '#1976d2' : '#f00'" dark>{{ response.code }}</v-toolbar>
          <v-card-text>
            <div class="v-response-text text-h4 pa-12">{{ response.description }}</div>
          </v-card-text>
          <v-card-text>
            <div class="v-response-text text-h6 pa-12">{{ response.message }}</div>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" text @click="closeResponseDialog">
              OK
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-col>
  </v-row>
</template>

<script>
export default {
    data() {
        return {
            response: {
                showResponseDialog: false,
                success: false,
                code: "InternalError",
                description: "An error occurred on the server. Please try again later or contact your administrator",
                message: "",
            }            
        }
    },
    methods: {
        closeResponseDialog() {
            this.response.showResponseDialog = false
        },
        showSuccessResponse(success) {
            this.response.showResponseDialog = true
            this.response.success = true
            this.response.code = success.data.Code
            this.response.description = success.data.Description
            this.response.message = success.data.Message
        },
        showErrorResponse(error) {
            this.response.showResponseDialog = true
            this.response.success = false
            this.response.code = error.response.data.Code
            this.response.description = error.response.data.Description
            this.response.message = error.response.data.Message
        },
    }
}
</script>

<style>
.v-response-text {
  word-break: break-word;
}
</style>