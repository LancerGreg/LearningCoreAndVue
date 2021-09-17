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
            <v-list-item v-show="!(item.title === 'Friends' && accountState !== 0)" v-for="item in menuItems" :key="item.title" :to="item.path">
              <v-list-item-icon>
                <v-icon>{{ item.icon }}</v-icon>
              </v-list-item-icon>
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item>  
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>
  </v-card>
</template>

<script>
import store from "../../store/"
import axios from 'axios'
import router from '../../router'

export default {
  data: () => ({
    drawer: false,
    group: null,
    menuItems: [
        { title: 'Home', path: '/', icon: 'home' },
        { title: 'Account', path: '/account', icon: 'mdi-account' },
        { title: 'Friends', path: '/friends', icon: 'people' },
    ],
  }),
  computed: {
   accountState () {
       return store.getters.ACCOUNTSTATE;
   },
  },
  methods: {
    exit () {
      axios.post(store.getters.URLS.API_URL + "account/logout")
        .then(() => {
          router.go(store.getters.URLS.API_URL + "account")
        })
    }
  }
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