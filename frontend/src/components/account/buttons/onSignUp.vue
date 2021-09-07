<template>
    <v-btn color="primary" type="button" @click="onSignUp">Sign Up</v-btn>
</template>

<script>
import store from '../../../store'

export default {
   methods: {
    onSignUp () {
      store.commit('SET_ACCOUNTSTATE', store.getters.ACCOUNTSTATES.OnSignUp);
    }
  }
}
</script>