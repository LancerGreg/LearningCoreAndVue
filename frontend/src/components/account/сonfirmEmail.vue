<template>
    <div>
    </div>
</template>

<script>
import store from '../../store'
import axios from 'axios'
import router from '../../router'

export default {
  data() {
    return {
    }
  },
  methods: {
    confirmEmail() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      let email = params.get("email");
      let token = params.get("token");
      if (email === null || email === "" || token === null || token === "") {
        alert("ERROR\nBad request")
        router.push({ name: "Account"})
      } else {
        axios.post(store.getters.URLS.API_URL + "auth/confirm_email?email=" + email + "&token=" + token).then(() => {
        router.push({ name: "Account"})
        }).catch(error => {
          try {
            error.response.data.forEach(element => {
              alert(element.Code + "\n" + element.Description)
            });
          } catch {
            alert("ERROR\nBad request")
          }
          router.push({ name: "Account"})
        });
      }
    }
  },
 beforeMount(){
    this.confirmEmail()
 },
}
</script>