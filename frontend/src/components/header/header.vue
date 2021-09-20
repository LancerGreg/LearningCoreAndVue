<template>
  <v-card class="mx-auto overflow-hidden" height="100%">
    <v-app-bar color="deep-purple" dark>
      <v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>

      <v-toolbar-title>Friendes's Tree</v-toolbar-title>

      <v-list-item v-if="accountState === 0" @click="exit" class="d-flex d-flex-none right=100 ml-auto ">
        <v-list-item-title>Sign out</v-list-item-title>
        <v-list-item-icon>
          <v-icon>exit_to_app</v-icon>
        </v-list-item-icon>
      </v-list-item>  
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" absolute temporary>
      <v-list nav dense>
        <v-list-item-group v-model="group" active-class="deep-purple--text text--accent-4">
            <v-toolbar-title class="navigation-drawer-title d-flex text-xs-center justify-center">Friendes's Tree</v-toolbar-title>  
            <v-list-item v-for="item in menuItems" :key="item.title" :to="item.path">
              <v-list-item-icon>
                <v-icon>{{ item.icon }}</v-icon>
              </v-list-item-icon>
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item>  
            <v-list-group v-show="accountState === 0" prepend-icon="people" value="true">
              <template v-slot:activator>
                <v-list-item>
                  <v-list-item-title>Friends</v-list-item-title>
                </v-list-item>
              </template>
              <v-list-item v-for="(item, i) in friendMenu" :key="i" :to="item.path" class="pl-16">
                <v-list-item-title v-text="item.title"></v-list-item-title>
                <v-list-item-icon v-if="item.porpInvite === null">
                  <v-icon v-text="item.icon"></v-icon>
                </v-list-item-icon>
                <v-list-item-icon v-else>
                  <v-icon color="green" v-text="item.icon"></v-icon>
                </v-list-item-icon>
              </v-list-item>
            </v-list-group>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>
  </v-card>
</template>

<script>
import store from "../../store/"
import axios from 'axios'
import router from '../../router'
import func from 'vue-editor-bridge'

export default {
  data: () => ({
    drawer: false,
    group: null,
    menuItems: [
      { title: 'Home', path: '/', icon: 'home' },
      { title: 'Account', path: '/account', icon: 'mdi-account' },
    ],
    friendMenu: [
      { title: "Friendes's Tree", path: "/friendship/friendess_tree", icon: 'account_tree', porpInvite: null },
      { title: "Add new friend", path: "/friendship/add_new_friend", icon: 'person_add', porpInvite: null },
      { title: "Invite of freindship", path: "/friendship/invite_of_freindship", icon: 'group_add', porpInvite: function () { getNewInvites() } },
    ],
  }),
  computed: {
    accountState () {
      return store.getters.ACCOUNTSTATE;
    },
  },
  methods: {
    exit() {
      axios.post(store.getters.URLS.API_URL + "auth/logout")
        .then(() => {
          router.go(store.getters.URLS.API_URL + "account")
        })
    },
    getNewInvites() {
      store.dispatch('SET_NEWINVITE');
      console.log(store.getters.NEWINVITE);
      return store.getters.NEWINVITE;
    },
  },
}
</script>

<style scoped>
    .v-card {
        position: absolute;
        max-width: none;
        width: 100%;
    }

    .v-sheet.v-card {
        border-radius: 0;
    }

    .navigation-drawer-title {
        -webkit-box-shadow: 0px 5px 2px 0px rgba(34, 60, 80, 0.25);
        -moz-box-shadow: 0px 5px 2px 0px rgba(34, 60, 80, 0.25);
        box-shadow: 0px 5px 2px 0px rgba(34, 60, 80, 0.25);
    }

    .d-flex-none {
      flex: none !important; 
    }
</style>