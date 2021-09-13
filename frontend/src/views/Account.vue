<template>
    <div>
        <SignIn v-if="accountState == this.accountStates.OnSignIn" :accountState='accountState' />
        <SignUp v-if="accountState == this.accountStates.OnSignUp" :accountState='accountState' />
        <ForggotPassword v-if="accountState == this.accountStates.OnForgotPassword" :accountState='accountState' />
        <UserCredentials v-if="accountState == this.accountStates.OnUserCredentials" :accountState='accountState' /> 
    </div>
</template>

<script>
import store from "../store/index"

import SignIn from "../components/account/signIn.vue"
import SignUp from "../components/account/signUp.vue"
import UserCredentials from "../components/account/userCredentials.vue"
import ForggotPassword from "../components/account/forgotPassword.vue"

export default {
  name: 'Account',
  computed: {
    accountState () {
        return store.getters.ACCOUNTSTATE;
    },
    accountStates () {
        return store.getters.ACCOUNTSTATES;
    }
  },
  data: () => {
      return {

      }
  },
  methods: {
  },
  mounted() {
    store.dispatch('SET_USERISAUTHORITED');
  },
  components: {
    SignIn,
    SignUp,
    UserCredentials,
    ForggotPassword
  },
}
</script>
