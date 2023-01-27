<template>
  <v-app>
    <Header />
    <v-main>
    <v-container>
      <router-view></router-view>
    </v-container>
    </v-main>
  </v-app>
</template>

<script>
import Header from "../src/components/header/header.vue"

export default {
  name: 'App',
  components: {
    Header
  },
  data: () => ({
    
  }),
};
</script>

<style scoped>
main {
  padding: 65px 0 0 0 !important ;
}
</style>

<style>
.justify-content-between {
  display: flex;
  justify-content: space-between;
}
.flex-flow-wrap {    
  flex-flow: wrap;
  flex: 1 1 100px !important;
}
</style>
