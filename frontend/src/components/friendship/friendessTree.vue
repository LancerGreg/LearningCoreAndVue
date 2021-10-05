<template>
<div class="graph-box">
  <div v-if="loader" class="loader">
     <Loader :loaderPerams="loaderPerams" />
  </div>
  <d3-network v-else :net-nodes="nodes" :net-links="links" :options="options" @node-click='toTarget' />
  
  <v-menu :close-on-content-click="false" origin="center center">
    <template v-slot:activator="{ on, attrs }">
      <v-btn v-bind="attrs" v-on="on" class="option-button">
        <v-img max-height="150" max-width="150" width="75px" src="../../assets/images/friendshipTree/Options.png" lazy-src="../../assets/images/friendshipTree/Options_lazy.png">
          <template v-slot:placeholder>
            <v-row class="fill-height ma-0" align="center" justify="center" >
              <v-progress-circular indeterminate color="grey lighten-5" ></v-progress-circular>
            </v-row>
          </template>
        </v-img>
      </v-btn>
    </template>

    <v-card class="v-card-option">
      <v-card-text>
        <v-row>
          <v-col class="pr-4">
            <v-checkbox v-model="menuOptions.optionFontSize" label="Show users name" color="success"></v-checkbox>
            <v-slider v-show="menuOptions.optionFontSize" class="option-input" v-model="menuOptions.fontSize" label="Text size" min="8" max="72" hide-details>
              <template v-slot:append>
                <v-text-field v-model="menuOptions.fontSize" class="mt-0 pt-0" hide-details single-line type="number" style="width: 60px"></v-text-field>
              </template>
            </v-slider>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="pr-4">
            <v-slider class="option-input" v-model="menuOptions.force" label="Map scaling" min="100" max="20000" hide-details>
              <template v-slot:append>
                <v-text-field v-model="menuOptions.force" class="mt-0 pt-0" hide-details single-line type="number" style="width: 60px"></v-text-field>
              </template>
            </v-slider>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="pr-4">
            <v-slider class="option-input" v-model="menuOptions.nodeSize" label="Node size" min="3" max="100" hide-details>
              <template v-slot:append>
                <v-text-field v-model="menuOptions.nodeSize" class="mt-0 pt-0" hide-details single-line type="number" style="width: 60px"></v-text-field>
              </template>
            </v-slider>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="pr-4">
            <v-slider class="option-input" v-model="menuOptions.linkWidth" label="Link width" min="3" max="15" hide-details>
              <template v-slot:append>
                <v-text-field v-model="menuOptions.linkWidth" class="mt-0 pt-0" hide-details single-line type="number" style="width: 60px"></v-text-field>
              </template>
            </v-slider>
          </v-col>
        </v-row>
        <v-divider></v-divider>
        <v-list>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title class="text-white-space-break-spaces">Friends Filter</v-list-item-title>
              <v-list-item-subtitle class="text-white-space-break-spaces">To order by, define the indicators and click "Request"</v-list-item-subtitle>
            </v-list-item-content>
            <v-list-item-action>
              <v-btn @click="filterFriends" >
                <v-list-item-title>Request</v-list-item-title>
              </v-btn>
            </v-list-item-action>
          </v-list-item>
        </v-list>
        <v-row>
          <v-col class="pr-4">
            <v-checkbox v-model="menuOptions.simplifiedLink" label="Simplified link" color="success"></v-checkbox>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="pr-4">
            <v-slider class="option-input" v-model="menuOptions.friendsRange" label="Friends range" min="1" max="8" hide-details>
              <template v-slot:append>
                <v-text-field v-model="menuOptions.friendsRange" class="mt-0 pt-0" hide-details single-line type="number" style="width: 60px"></v-text-field>
              </template>
            </v-slider>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-menu>

  <v-card v-if="displayUserInfo" class="v-card-user-info">
    <v-list>
      <v-list-item>
        <v-list-item-avatar>
          <img src="../../assets/images/avatars/EmtyAvatar_40x40.png" alt="Avatar">
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title class="text-white-space-break-spaces">{{ userInfo.fullName }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-subtitle class="text-white-space-break-spaces">Total count of nodes: {{ nodes.length }}</v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>        
        <v-list-item-action class="button-status">          
          <v-btn v-if="userInfo.thisUser" class="button-status text-center" dark>
            It's you
          </v-btn>
          <v-btn v-else-if="userInfo.isFriend" class="button-status button-friend text-center" dark>
            Friend
          </v-btn>
          <v-btn v-else-if="!userInfo.isFriend && userInfo.haveInvite" class="button-status button-wait text-center" dark>
            Invitation didn't confirm
          </v-btn>
          <v-btn v-else-if="!userInfo.haveInvite" class="button-status button-invite text-center" dark @click="showDialogInvite">
            Request for friendship
          </v-btn>
        </v-list-item-action>
      </v-list-item>
    </v-list>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn text @click="closeUserInfo">
        Close
      </v-btn>
    </v-card-actions>
  </v-card>

  <v-dialog v-model="dialogInvite" max-width="600px">
    <v-card>
      <v-card-title class="text-h5">Are you sure you want to send friendship invite?</v-card-title>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="closeDialogInvite">Cancel</v-btn>
        <v-btn color="blue darken-1" text @click="sendInvite">Yes</v-btn>
        <v-spacer></v-spacer>
      </v-card-actions>
    </v-card>
  </v-dialog>
</div>
</template>

<script>
import store from "../../store"
import axios from 'axios'
import Loader from "../loader/loader.vue"

// documentation - https://github.com/emiliorizzo/vue-d3-network
import D3Network from 'vue-d3-network'

export default {
  name: "Friendess_Tree",  
  components: {
    D3Network,
    Loader
  },
  data () {
    return {
      nodes: [],
      links: [],
      offset: {
        x: 0,
        y: 0
      },
      hints: true,
      displayUserInfo: false,
      dialogInvite: false,
      menuOptions: {
        force: 5000,
        fontSize: 18,
        optionFontSize: true,
        nodeSize: 20,
        linkWidth: 3,
        
        simplifiedLink: false,
        friendsRange: 1,
      },
      loader: false,
      loaderPerams: {
        size: 100,
        color: "#1976d2",
        width: 10
      },
    }
  },
  computed:{
    options(){
      return{
        force: this.menuOptions.force,
        nodeSize: this.menuOptions.nodeSize,
        linkWidth: this.menuOptions.linkWidth,
        fontSize: this.menuOptions.fontSize,
        nodeLabels: this.menuOptions.optionFontSize,
        offset: {
          x: this.offset.x,
          y: this.offset.y
        }
      }
    },
    userInfo(){
      return{
        id: "",
        nodeId: "",
        fullName: "",
        thisUser: false,
        isFriend: false,
        haveInvite: false,
      }
    }
  },
  async beforeMount() {
    this.loader = true
    await axios.get(store.getters.URLS.API_URL + "friend/get_graph_data?range=" + this.menuOptions.friendsRange + "&simplifiedLink=" + this.menuOptions.simplifiedLink)
    .then((response) => {
      this.nodes = response.data.Item1.map(function(user) {
        return { id: user.Id, name: user.FullName, _color: "#000", _labelClass: "node-label", _userInfo: { userId: user.Id, fullName: user.FullName, isFriend: user.IsFriend, haveInvite: user.HaveInvite } };
      });
      this.links = response.data.Item2.map(function(friendship) {
        return { sid: friendship.FirstUserId, tid: friendship.SecondUserId, _color: "rgba(145,145,145,0.25)" };
      });
    }).catch(() => {
      alert("Error 500\n Serve not working")
    }).finally(() => this.loader = false);
  },
  methods: {
    toTarget(event, node){
      // delay is needed in case pull by the node
      setTimeout(() => {
        this.highlightTarget(node)
        this.showUserInfo(node)
      }, 120);      
    },
    highlightTarget(node) {
      // the node is go to the center
      this.offset.x += window.innerWidth / 2 - node.x
      this.offset.y += window.innerHeight / 2 - node.y

      // change all colors to default 
      this.nodes.map(e => { e._color = "#000"; e._labelClass = ""; })
      this.links.map(e => e._color = "rgba(145,145,145,0.25)")

      // change color for selected node
      node._color = "#ff0"
      node._labelClass = "selected-node-title"

      // change color for friends node
      this.nodes.map(n => {
        this.links.map(l => {
          if ((l.sid === node.id && l.tid === n.id) || (l.sid === n.id && l.tid === node.id)) {
            n._color = "#f00"
            n._labelClass = "friend-node-title"
            l._color = "rgba(0, 207, 255, 0.25)"
          }
        })
      })
    },
    showUserInfo(node) {
      this.displayUserInfo = true
      this.userInfo.nodeId = node.id
      this.userInfo.id = node._userInfo.userId
      this.userInfo.thisUser = node.index == 0
      this.userInfo.fullName = node._userInfo.fullName
      this.userInfo.isFriend = node._userInfo.isFriend
      this.userInfo.haveInvite = node._userInfo.haveInvite
    },
    closeUserInfo() {
      this.displayUserInfo = false
    },
    showDialogInvite() {
      this.dialogInvite = true
    },
    closeDialogInvite() {
      this.dialogInvite = false
    },
    sendInvite() {
      axios.post(store.getters.URLS.API_URL + "invite/invite_request_by_id?userId=" + this.userInfo.id)
      .then(() => {
        this.closeDialogInvite()
        this.userInfo.haveInvite = true
        this.nodes = this.nodes.map(e => {
          if(e.id == this.userInfo.nodeId) e._userInfo.haveInvite = true; 
          return e;
        })
      }).catch(() => {
        this.closeDenie()
        alert("Error 500\n Serve not working")
      });
    },
    async filterFriends(){
      this.loader = true
      await axios.get(store.getters.URLS.API_URL + "friend/get_graph_data?range=" + this.menuOptions.friendsRange + "&simplifiedLink=" + this.menuOptions.simplifiedLink)
      .then((response) => {
        this.nodes = response.data.Item1.map(function(user) {
          return { id: user.Id, name: user.FullName, _color: "#000", _labelClass: "node-label", _userInfo: { userId: user.Id, fullName: user.FullName, isFriend: user.IsFriend, haveInvite: user.HaveInvite } };
        });
        this.links = response.data.Item2.map(function(friendship) {
          return { sid: friendship.FirstUserId, tid: friendship.SecondUserId, _color: "rgba(145,145,145,0.25)" };
        });
      }).catch(() => {
        alert("Error 500\n Serve not working")
      }).finally(() => this.loader = false);
    }
  }
}
</script>

<style src="vue-d3-network/dist/vue-d3-network.css"></style>

<style scoped>  
  .loader {
    position: absolute;
    left: calc(50vw - 50px);
    top: calc(50vh - 50px); 
  }

  .graph-box {
    bottom: 0;
    box-sizing: content-box;
    height: calc(100% - 10px);
    left: 0;
    max-width: 100%;
    position: absolute;
    top: 0;
    width: 100%;
  }

  .option-button {
    position: absolute;
    display: flex;
    left: 25px;
    top: 25px;
    width: unset !important;
    height: unset !important;
    padding: 5px !important;
  }

  .v-card-option {
    max-width: 500px;
  }

  .v-card-user-info {
    position: absolute;
    right: 25px;
    top: 25px;
    width: unset !important;
    height: unset !important;
    padding: 5px;
    border-radius: 5px;
    max-width: 500px;
    margin: 0 0 0 115px;
  }

  .button-status {
    margin-right: unset !important;
    padding: 0 !important;
    width: 100%;
  }

  .button-friend {
    background: #008000 !important;
  }

  .button-wait {
    background: #0000ff !important;
  }

  .button-invite {
    background: #ffa500 !important;
  }

  .text-white-space-break-spaces {
    white-space: break-spaces !important;
  }

  .v-input--hide-details > .v-input__control > .v-input__slot {
    display: block !important;
  }
</style>

<style>
  .node-label:hover {
    position: relative;
    z-index: 1;
  }
  .selected-node-title {
    fill: #ff0 !important;
    text-shadow: 1px 1px 2px #000;
  }
  .friend-node-title {
    fill: #f00 !important;
    text-shadow: 1px 1px 2px #000;
  }
  .node {
    position: relative;
    z-index: 2;
  }
  .option-input {
    display: flex !important;
  }
  @media screen and (max-width: 600px) {
    .option-input {
      display: block !important;
    }    
  }
</style>