<template>
<v-card id="lateral">
    <v-tabs v-model="tabs" align-with-title>
      <v-tab class="v-bar-title" disabled>
        Find friend by:
      </v-tab>
      <v-tab href="#byName">
        Name
      </v-tab>
      <v-tab href="#byEmail">
        Email
      </v-tab>
      <v-tab href="#byPhone">
        Phone
      </v-tab>
      <v-tabs-slider color="pink"></v-tabs-slider>
    </v-tabs>
    <v-fab-transition>
      <div :key="addingMethod.method" class="mt-2 mb-2">
        <AddByName v-show="addingMethod.method === 'AddByName'"/>
        <AddByEmail v-show="addingMethod.method === 'AddByEmail'"/>
        <AddByPhone v-show="addingMethod.method === 'AddByPhone'"/>
      </div>
    </v-fab-transition>
  </v-card>
</template>

<script>
import AddByName from '../friendship/addNewFriend/addByName.vue'
import AddByEmail from '../friendship/addNewFriend/addByEmail.vue'
import AddByPhone from '../friendship/addNewFriend/addByPhone.vue'

export default {
  name: 'Add_new_friend',
  data: () => {
    return {
      fab: false,
      hidden: false,
      tabs: null,
      addingMethods: ['byName', 'byEmail', 'byPhone']
    }
  },
  components: {
    AddByName,
    AddByEmail,
    AddByPhone
  },
  computed: {
    addingMethod () {
      switch (this.tabs) {
        case 'byName': return { method: "AddByName" }
        case 'byEmail': return { method: "AddByEmail" }
        case 'byPhone': return { method: "AddByPhone" }
        default: return {}
      }
    },
  },
}
</script>

<style scoped>
  #lateral .v-btn--example {
    bottom: 0;
    position: absolute;
    margin: 0 0 16px 16px;
  }
  
  .v-tabs {
    border-bottom: 2px solid aliceblue;
  }

  .v-tab {
    font-size: 24px;
  }

  .v-bar-title {
    color: rgba(0, 0, 0, 1) !important;
  }
</style>
