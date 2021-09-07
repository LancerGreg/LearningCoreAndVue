<template>
<div>
  <v-img
    class="img-center"
    contain
    lazy-src="../assets/images/home/FriendshipTree(100x100).jpg"
    max-height="600"
    max-width="600"
    height="75vh"
    src="../assets/images/home/FriendshipTree(600x600).jpg"
    alt="FriendshipTree img"
  ></v-img>
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