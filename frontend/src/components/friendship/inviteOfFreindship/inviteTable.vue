<template>
  <v-data-table :headers="headers" :items="invites" sort-by="WhenSend" sort-desc="WhenSend" class="elevation-1">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>New Invites</v-toolbar-title>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialogAccept" max-width="500px">
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeAccept">
                OK
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to denie this invite?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDenie">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="denieInviteConfirm">Yes</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:[`item.actions`]="{ item }">
      <v-icon style="background:green;" class="v-icon-action" color="white" @click="acceptInvite(item)">
        check
      </v-icon>
      <v-icon style="background:red;" color="white" @click="denieInvite(item)">
        close
      </v-icon>
    </template>
    <template v-slot:no-data>
      <Loader v-if="loader" :loaderPerams="loaderPerams" />
      <v-btn v-else color="primary" @click="initialize">
        Reset
      </v-btn>
    </template>
  </v-data-table>
</template>

<script>
import store from "../../../store"
import axios from 'axios'
import Loader from '../../loader/loader.vue'

  export default {
    components: {
        Loader
    },
    data: () => ({
      dialogAccept: false,
      dialogDelete: false,
      loader: false,
      loaderPerams: {
        size: 40,
        color: "#1976d2",
        width: 5
      },
      headers: [
        {
          text: 'Full Name',
          align: 'start',
          sortable: false,
          value: 'FullName',
        },
        { text: 'First Name', value: 'FirstName' },
        { text: 'Last Name', value: 'LastName' },
        { text: 'When Send', value: 'WhenSend' },
        { text: 'Accept Invite', value: 'actions', sortable: false },
      ],
      invites: [],
      editedIndex: -1,
    }),

    computed: {
      formTitle () {
        return 'Great! You have new friend!'
      },
    },

    watch: {
      dialogDelete (val) {
        val || this.closeDenie()
      },
    },

    created () {
      this.initialize()
    },

    methods: {
      initialize () {
        this.loader = true
        axios.get(store.getters.URLS.API_URL + "invite/get_not_decide_invites")
        .then((response) => {          
          this.invites = response.data
        }).catch(() => {
          alert("Error 500\n Serve not working")
        });
        this.loader = false
      },

      acceptInvite (item) {
        axios.post(store.getters.URLS.API_URL + "invite/confirm_invite?inviteId=" + item.InviteId + "&decide=1")
        .then(() => {
          this.editedIndex = this.invites.indexOf(item)
          this.dialogAccept = true        
        }).catch(() => {
          alert("Error 500\n Serve not working")
        });
      },

      denieInvite (item) {
        this.editedIndex = this.invites.indexOf(item)
        this.dialogDelete = true
      },

      denieInviteConfirm () {
        axios.post(store.getters.URLS.API_URL + "invite/confirm_invite?inviteId=" + this.invites[this.editedIndex].InviteId + "&decide=0")
        .then(() => {
          this.invites.splice(this.editedIndex, 1)
          this.closeDenie()
        }).catch(() => {
          this.closeDenie()
          alert("Error 500\n Serve not working")
        });
      },

      closeAccept () {
        this.invites.splice(this.editedIndex, 1)
        this.dialogAccept = false
        this.$nextTick(() => {
          this.editedIndex = -1
        })
      },

      closeDenie () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedIndex = -1
        })
      },
    },
  }
</script>

<style scoped>
.v-icon-action {
    margin: 0 10px 0 5px;
}
.v-icon {
    padding: 1px;
    border-radius:50%;
    box-shadow: 0 0 3px #777;
}
</style>