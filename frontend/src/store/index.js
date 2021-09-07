import Vue from 'vue'
import Vuex from 'vuex'

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
  },
  mutations: {
    SET_ACCOUNTSTATE: (state, payload) => {
      state.accountState = payload;
    },
  },
  actions: {
  },
  modules: {
  }
})

export default store;