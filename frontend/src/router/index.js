import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Account from '../views/Account.vue'
import ConfirmEmail from '../components/account/сonfirmEmail.vue'
import ConfirmResetPassword from '../components/account/confirmResetPassword.vue'
import AddNewFriend from '../components/friendship/add_new_friend.vue'
import FriendessTree from '../components/friendship/friendess_tree.vue'

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
    path: "/friendship/friendess_tree",
    name: "Friendess_Tree",
    component: FriendessTree
  },
  {
    path: '/friends/add_new_friend',
    name: 'Add_new_friend',
    component: AddNewFriend
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
