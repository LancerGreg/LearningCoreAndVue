<template>
<v-container fluid>
  <v-layout row wrap>
    <v-flex xs12 class="text-xs-center" mt-5>
      <h1>Sign In</h1>
    </v-flex>
    <v-flex xs12 sm6 offset-sm3 mt-3>
      <form>
        <v-layout column>
          <v-flex>
            <v-text-field
              name="email"
              label="Email"
              id="email"
              type="email"
              required></v-text-field>
          </v-flex>
          <v-flex>
            <v-text-field
              name="password"
              label="Password"
              id="password"
              type="password"
              required></v-text-field>
          </v-flex>
          <v-flex class="text-xs-center justify-content-between flex-flow-wrap" mt-5>
            <v-btn color="primary" type="submit" @click="toSignIn">Sign In</v-btn>
            <OnForggotPassword />
            <OnSignUp />
          </v-flex>
        </v-layout>
      </form>
    </v-flex>
  </v-layout>
</v-container>
</template>

<script>
import OnForggotPassword from '../account/buttons/onForggotPassword.vue'
import OnSignUp from '../account/buttons/onSignUp.vue'

export default {
  data: () => {
      return {
      }
  },
  methods: {
    toSignIn () {

    }
  },
  components: {
    OnForggotPassword,
    OnSignUp
  }
}
</script>
