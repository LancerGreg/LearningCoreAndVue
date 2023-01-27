<template>
<div>
  <v-img
    src="../assets/images/home/FriendshipTree.png"
    lazy-src="../assets/images/home/FriendshipTree_lazy.png"
    class="img-center"
    contain
    max-height="600"
    max-width="600"
    height="75vh"
  >
    <template dark v-slot:placeholder>
      <v-row class="fill-height ma-0" align="center" justify="center" >
        <v-progress-circular indeterminate color="grey lighten-1" ></v-progress-circular>
      </v-row>
    </template>
  </v-img>
</div>
</template>

<script>
export default {
  name: 'Home',
  components: {      
  },
}
</script>

<style scoped>
.img-center {
  display: flex;
  align-items: center;
  margin: 0 auto;
}
</style>