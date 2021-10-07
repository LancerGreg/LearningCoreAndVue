<template>
  <v-data-table :headers="headers" :items="users" :loading="loading" class="elevation-1">
    <template v-slot:top> 
      <v-toolbar flat>
        <v-toolbar-title>{{ match }}</v-toolbar-title>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialogFriendship" max-width="600px">
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ dialogFriendshipTitle }}</span>
            </v-card-title>  
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDialogFriendship">
                OK
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogWait" max-width="600px">
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ dialogWaitTitle }}</span>
            </v-card-title>  
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDialogWait">
                OK
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogInvite" max-width="600px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to send friendship invite?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDialogInvite">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="sendInvite">Yes</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:[`item.actions`]="{ item }">
      <v-icon v-if="!item.IsFriend && !item.HaveInvite" style="background:orange;" class="v-icon-action" color="white" large @click="showInvite(item)">
        send
      </v-icon>
      <v-icon v-if="!item.IsFriend && item.HaveInvite" title="the request is pending " style="background:blue;" class="v-icon-action" color="white" large @click="showDialogWait()">
        done
      </v-icon>
      <v-icon v-if="item.IsFriend" title="they are your friend" style="background:green;" class="v-icon-action" color="white" large @click="showDialogFriendship()">
        done_all
      </v-icon>
    </template>
  </v-data-table>
</template>

<script>
import router from "../../../../router"
import store from '../../../../store'
import axios from 'axios'

export default {
  props: ['match', "search"],
  data() {
        return {
            totalUsers: 0,
            users: [],
            editedIndex: -1,
            loading: true,
            options: {},
            headers: [
                {
                    text: "Full Name",
                    align: "left",
                    sortable: false,
                    value: "FullName",
                    width: '30%'
                },
                { text: "First Name", value: "FirstName", sortable: false, width: '25%' },
                { text: "Last Name", value: "LastName", sortable: false, width: '25%' },
                { text: 'Send Invite', value: 'actions', sortable: false },
            ],
            dialogFriendship: false,
            dialogInvite: false,
            dialogWait: false
        };
    },
    watch: {
        params: {
            handler() {
                this.getDataFromApi().then(data => {
                    this.users = data.items;
                    this.totalUsers = data.total;
                });
            },
            deep: true
        }
    },
    async mounted() {
        await this.getDataFromApi().then(data => {
            this.users = data.items;
            this.totalUsers = data.total;
        });
    },

    computed: {
        params() {
            return {
                ...this.options,
                query: this.search
            };
        },
      dialogFriendshipTitle () {
        return 'You are already friends!'
      },
      dialogWaitTitle () {
        return 'Your friendship request has not yet been accepted.'
      },
    },

    methods: {
        async getDataFromApi() {
            this.loading = true;
            let search = this.search.trim().toLowerCase();
            let items = await this.getUsers(search);
            return new Promise((resolve) => {
                const {
                    sortBy,
                    descending,
                    page,
                    rowsPerPage
                } = this.options;

                if (search) {
                    items = items.filter(item => {
                        return Object.values(item)
                            .join(",")
                            .toLowerCase()
                    });
                }

                if (this.options.sortBy) {
                    items = items.sort((a, b) => {
                        const sortA = a[sortBy];
                        const sortB = b[sortBy];

                        if (descending) {
                            if (sortA < sortB) return 1;
                            if (sortA > sortB) return -1;
                            return 0;
                        } else {
                            if (sortA < sortB) return -1;
                            if (sortA > sortB) return 1;
                            return 0;
                        }
                    });
                }

                if (rowsPerPage > 0) {
                    items = items.slice(
                        (page - 1) * rowsPerPage,
                        page * rowsPerPage
                    );
                }
                
                const total = items.length;
                

                setTimeout(() => {
                    this.loading = false;
                    resolve({
                        items,
                        total
                    });
                }, 300);
            });
        },
        async getUsers(search) {
          if (search) {
            let firstName = /\S/.test(search) ? search.split(" ")[0] : search
            let lastName = /\S/.test(search) ? search.split(" ").slice(1).join(" ") : ""
            let users;
            await axios.get(store.getters.URLS.API_URL + "friend/get_user_by_name?firstName=" + firstName + "&lastName=" + lastName)
            .then((response) => {
              if (this.$props.match == "Best Match") {
                users = response.data.Item1
              }
              if (this.$props.match == "Other Match") {
                users = response.data.Item2
              }
            }).catch(() => {
              router.push({ name: "Error_500"})
              return []
            });
            return users;
          } else {
            return []
          }
        },
        closeDialogFriendship() {
            this.dialogFriendship = false;
        },
        showDialogFriendship() {
            this.dialogFriendship = true;
        },
        closeDialogWait() {
            this.dialogWait = false;
        },
        showDialogWait() {
            this.dialogWait = true;
        },
        showInvite (item) {
          this.editedIndex = this.users.indexOf(item)
          this.dialogInvite = true
        },
        closeDialogInvite() {
          this.editedIndex = -1
          this.dialogInvite = false
        },
        sendInvite() {
            axios.post(store.getters.URLS.API_URL + "invite/invite_request_by_id?userId=" + this.users[this.editedIndex].UserId)
            .then(() => {
              this.closeDialogInvite()
              this.getDataFromApi().then(data => {
                this.users = data.items;
                this.totalUsers = data.total;
              });
            }).catch(() => {
              this.closeDenie()
              router.push({ name: "Error_500"})
            });
        }
    }
}
</script>

<style scoped>
.v-icon-action {
    margin: 0 0 0 10px;
    padding: 2px;
}
</style>