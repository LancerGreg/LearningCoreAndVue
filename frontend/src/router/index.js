import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Account from '../views/Account.vue'
import Friends from '../views/Friends.vue'
import ConfirmEmail from '../components/account/сonfirmEmail.vue'
import ConfirmResetPassword from '../components/account/confirmResetPassword.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/account',
    name: 'Account',
    component: Account,
  },
  {
    path: '/account/confirm_email',
    name: 'ConfirmEmail',
    component: ConfirmEmail,
  },
  {
    path: '/account/confirm_reset_password',
    name: 'ConfirmResetPassword',
    component: ConfirmResetPassword,
  },
  {
    path: '/friends',
    name: 'Friends',
    component: Friends
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
