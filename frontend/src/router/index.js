import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Account from '../views/Account.vue'
import Messanger from '../views/Messanger.vue'
import ConfirmEmail from '../components/account/сonfirmEmail.vue'
import ConfirmResetPassword from '../components/account/confirmResetPassword.vue'
import AddNewFriend from '../components/friendship/addNewFriend.vue'
import FriendessTree from '../components/friendship/friendessTree.vue'
import InviteOfFreindship from '../components/friendship/inviteOfFreindship.vue'
import Error500 from '../views/Error500.vue'
import Error404 from '../views/Error404.vue'

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
    path: '/messanger',
    name: 'Messanger',
    component: Messanger,
  },
  {
    path: "/friendship/friendess_tree",
    name: "Friendess_Tree",
    component: FriendessTree
  },
  {
    path: '/friendship/add_new_friend',
    name: 'Add_new_friend',
    component: AddNewFriend
  },
  {
    path: '/friendship/invite_of_freindship',
    name: 'Invite_of_freindship',
    component: InviteOfFreindship
  },
  {
    path: '/error/500',
    name: 'Error_500',
    component: Error500
  },
  { 
    path: '*', 
    beforeEnter: (to, from, next) => { next('/error/404') } 
  },
  {
    path: '/error/404',
    name: 'Error_404',
    component: Error404
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
