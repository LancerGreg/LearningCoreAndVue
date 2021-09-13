import Vue from 'vue'
import Vuex from 'vuex'
import Axios from 'axios';

Vue.use(Vuex)

const store = new Vuex.Store({
  state: () => ({
    urls: {
      API_URL: "https://localhost:44332/api/"
    },
    accountState : 1,
    accountStates: {
      OnUserCredentials: 0, 
      OnSignIn: 1, 
      OnSignUp: 2, 
      OnForgotPassword: 3
    },
    userIsAuthorized: false
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
    }
  },
  mutations: {
    SET_ACCOUNTSTATE: (state, payload) => {
      state.accountState = payload;
    },
    SET_USERISAUTHORITED: (state, payload) => {
      state.userIsAuthorized = payload;
      if (payload) state.accountState = 0;      
    }
  },
  actions: {
    SET_USERISAUTHORITED: async (context) => {
      let {data} = await Axios.get(context.getters.URLS.API_URL + "account/user_is_authorized");
      context.commit('SET_USERISAUTHORITED', data);
    },
  },
})

export default store;