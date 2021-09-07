<template>
    <v-btn color="primary" type="button" @click="onSignIn">Already Registered?</v-btn>
</template>

<script>
import store from '../../../store'

export default {
   methods: {    
    onSignIn () {
      store.commit('SET_ACCOUNTSTATE', store.getters.ACCOUNTSTATES.OnSignIn);
    }
  }
}
</script>