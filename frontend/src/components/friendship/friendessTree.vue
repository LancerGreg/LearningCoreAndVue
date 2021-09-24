<template>
<div>
  <d3-network :net-nodes="nodes" :net-links="links" :options="options" />
</div>
</template>

<script>
import store from "../../store"
import axios from 'axios'

import D3Network from 'vue-d3-network'

export default {
  name: "Friendess_Tree",  
  components: {
    D3Network
  },
  data () {
    return {
      nodes: [],
      links: [],
      options:
      {
        force: 8000,
        nodeSize: 20,
        nodeLabels: true,
        linkWidth: 3,
        size:{ w:1980, h:1200},
        fontSize: 18,
      }
    }
  },
  async beforeMount() {
    await axios.get(store.getters.URLS.API_URL + "friend/get_graph_data?range=4")
    .then((response) => {
      this.nodes = response.data.Item1.map(function(user) {
        return { id: user.Id, name: user.FullName,  };
      });
      this.links = response.data.Item2.map(function(friendship) {
        return { sid: friendship.FirstUserId, tid: friendship.SecondUserId, _color: "rgba(145,145,145,0.25)" };
      });
    }).catch(() => {
      alert("Error 500\n Serve not working")
    });
  }
}
</script>

<style src="vue-d3-network/dist/vue-d3-network.css"></style>
