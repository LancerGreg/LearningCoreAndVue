import Vue from 'vue'
import Vuex from 'vuex'
import Axios from 'axios';

Vue.use(Vuex)

const store = new Vuex.Store({
  state: () => ({
    urls: {
      API_URL: "https://localhost:44332/api/"
    },
    accountState: 1,
    accountStates: {
      OnUserCredentials: 0, 
      OnSignIn: 1, 
      OnSignUp: 2, 
      OnForgotPassword: 3
    },
    userIsAuthorized: false,
    userProfile: {
      FirstName: "",
      LastName: "",
      Phone: "",
      Email: "",
      Password: ""
    }
  }),
  getters: {
    ACCOUNTSTATE: (state) => {
        return state.accountState;
    },
    ACCOUNTSTATES: (state) => {
      return state.accountStates;
    },
    URLS: (state) => {
      return state.urls;
    },
    USERISAUTHORITED: (state) => {
      return state.userIsAuthorized;
    },
    USERPROFILE: (state) => {
      return state.userProfile;
    }
  },
  mutations: {
    SET_ACCOUNTSTATE: (state, payload) => {
      state.accountState = payload;
    },
    SET_USERISAUTHORITED: (state, payload) => {
      state.userIsAuthorized = payload;
      if (payload) state.accountState = 0;      
    },
    SET_USERPROFILE: (state, payload) => {
      state.userProfile = payload;
    }
  },
  actions: {
    SET_USERISAUTHORITED: async (context) => {
      let {data} = await Axios.get(context.getters.URLS.API_URL + "auth/user_is_authorized");
      context.commit('SET_USERISAUTHORITED', data);
    },
    SET_USERPROFILE: async (context) => {
      let {data} = await Axios.get(context.getters.URLS.API_URL + "account/user_credentiails");
      context.commit('SET_USERPROFILE', data);
    },
  },
})

export default store;