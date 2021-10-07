<template>
<div>
    <ResponseDialog ref="responseDialog"/>
    <div v-if="loader" class="loader">
       <Loader :loaderPerams="loaderPerams" />
    </div>
</div>
</template>

<script>
import store from '../store/'
import axios from 'axios'

import Loader from "../components/loader/loader.vue"
import ResponseDialog from "../components/responseDialog/responseDialog.vue"

export default {
    name: 'Error_500',
    data: () => {
        return {            
            loader: false,
            loaderPerams: {
              size: 100,
              color: "#1976d2",
              width: 10
            },
        }
    },
    components: {    
        ResponseDialog,
        Loader
    },
    created() {
        this.loader = true
        axios.get(store.getters.URLS.API_URL + "error/get_internal_error")
        .then().catch(error => {
          this.$refs.responseDialog.showErrorResponse(error)
        }).finally(() => this.loader = false);
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