<template>
    <v-btn color="primary" type="button" @click="onForggotPassword">Forggot password?</v-btn>
</template>

<script>
import store from '../../../store'

export default {
  methods: {
    onForggotPassword () {
      store.commit('SET_ACCOUNTSTATE', store.getters.ACCOUNTSTATES.OnForgotPassword);
    }
  }
}
</script>
