<template>
<div class="loader">
  <Loader :loaderPerams="loaderPerams" />
</div>
</template>

<script>
import store from '../../store'
import axios from 'axios'
import router from '../../router'
import Loader from "../loader/loader.vue"

export default {
  data() {
    return {
      loaderPerams: {
        size: 100,
        color: "#1976d2",
        width: 10
      },
    }
  },
  methods: {
    confirmEmail() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      let email = params.get("email");
      let token = params.get("token");
      if (email === null || email === "" || token === null || token === "") {
        router.push({ name: "Account"})
      } else {
        axios.post(store.getters.URLS.API_URL + "auth/confirm_email?email=" + email + "&token=" + token).then(() => {
          router.push({ name: "Account"})
        }).catch(() => {
          router.push({ name: "Account"})
        });
      }
    }
  },
  beforeMount(){
    this.confirmEmail()
  },
  components: {
    Loader,
  }
}
</script>

<style scoped>
.loader {
  position: absolute;
  left: calc(50vw - 50px);
  top: calc(50vh - 50px); 
}
</style>