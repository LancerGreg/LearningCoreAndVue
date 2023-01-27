<template>
  <v-card id="inspire">
    <div class="px-2">
      <h2>Paste friend name to the <b>search</b></h2>
      <v-text-field label="search" v-model.lazy.trim="search"></v-text-field>
      <v-data-table :headers="headers" :items="users" :loading="loading" class="elevation-1">
        <template v-slot:top>
          <v-toolbar flat>
            <v-dialog v-model="dialogAlreadyAdd" max-width="600px">
              <v-card>
                <v-card-title>
                  <span class="text-h5">{{ dialogAlreadyAddTitle }}</span>
                </v-card-title>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="closeDialogAlreadyAdd">
                    OK
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
            <v-dialog v-model="dialogAddUser" max-width="600px">
              <v-card>
                <v-card-title class="text-h5">
                  Are you sure you want to send friendship
                  invite?
                </v-card-title>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="closeDialogAddUser">
                    Cancel
                  </v-btn>
                  <v-btn color="blue darken-1" text @click="addUser">Yes</v-btn>
                  <v-spacer></v-spacer>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-toolbar>
        </template>
        <template v-slot:[`item.actions`]="{ item }">
          <v-icon
            v-if="item.AlreadyInChat"
            title="User already at chat"
            style="background: green"
            class="v-icon-action"
            color="white"
            large
            @click="showDialogAlreadyAdd()"
          >
            done_all
          </v-icon>
          <v-icon
            v-else
            style="background: orange"
            class="v-icon-action"
            color="white"
            large
            @click="showDialogAddUser(item)"
          >
            send
          </v-icon>
        </template>
      </v-data-table>
    </div>
  </v-card>
</template>

<script>
import router from "../../router";
import store from "../../store";
import axios from "axios";

export default {
  props: ["chatId"],
  data() {
    return {
      search: "",
      totalUsers: 0,
      users: [],
      editedIndex: -1,
      loading: true,
      options: {},
      headers: [
        {
          text: "Full name",
          align: "left",
          sortable: false,
          value: "FullName",
        },
        { 
          text: "Add member", 
          value: "actions", 
          sortable: false 
        },
      ],
      dialogAlreadyAdd: false,
      dialogAddUser: false,
    };
  },
  watch: {
    params: {
      handler() {
        this.getDataFromApi().then((data) => {
          console.log("GETDATA");
          this.users = data.items;
          this.totalUsers = data.total;
        });
      },
      deep: true,
    },
  },
  async mounted() {
    await this.getDataFromApi().then((data) => {
      this.users = data.items;
      this.totalUsers = data.total;
    });
  },

  computed: {
    params() {
      return {
        ...this.options,
        query: this.search,
      };
    },
    dialogAlreadyAddTitle() {
      return "User is already at chat";
    },
  },

  methods: {
    async getDataFromApi() {
      this.loading = true;
      let search = this.search.trim().toLowerCase();
      let items = await this.getUsers(search);
      return new Promise((resolve) => {
        const { sortBy, descending, page, rowsPerPage } = this.options;

        if (search) {
          items = items.filter((item) => {
            return Object.values(item).join(",").toLowerCase();
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
          items = items.slice((page - 1) * rowsPerPage, page * rowsPerPage);
        }

        const total = items.length;

        setTimeout(() => {
          this.loading = false;
          resolve({
            items,
            total,
          });
        }, 300);
      });
    },
    async getUsers(search) {
      if (search) {
        let users;
        await axios
          .get(
            store.getters.URLS.API_URL +
              "chat/get_users_by_name?chatId=" +
              this.chatId +
              "&fullName=" +
              this.search
          )
          .then((response) => {
            users = response.data;
          })
          .catch(() => {
            router.push({ name: "Error_500" });
            return [];
          });
        return users;
      } else {
        return [];
      }
    },
    closeDialogAlreadyAdd() {
      this.dialogAlreadyAdd = false;
    },
    showDialogAlreadyAdd() {
      this.dialogAlreadyAdd = true;
    },
    showDialogAddUser(item) {
      this.editedIndex = this.users.indexOf(item);
      this.dialogAddUser = true;
    },
    closeDialogAddUser() {
      this.editedIndex = -1;
      this.dialogAddUser = false;
    },
    addUser() {
      axios
        .post(
          store.getters.URLS.API_URL +
            "chat/add_new_member?chatId=" +
            this.chatId +
            "&memberId=" +
            this.users[this.editedIndex].Id
        )
        .then(() => {
          this.users[this.editedIndex].AlreadyInChat = true;
          this.closeDialogAddUser();
        })
        .catch(() => {
          router.push({ name: "Error_500" });
        });
    },
  },
};
</script>

<style scoped>
.v-icon-action {
  margin: 0 0 0 10px;
  padding: 2px;
}
.v-data-footer {
  display: none !important;
}
</style>