<template>
    <v-card id="inspire">
        <div class="px-2">
            <h2>Paste friend name to the <b>search</b></h2>
            <v-text-field label="search" v-model.lazy.trim="search"></v-text-field>
            <UsersTable :match="'Best Match'" :search="search"/>
            <v-spacer></v-spacer>
            <UsersTable :match="'Other Match'" :search="search"/>
        </div>
    </v-card>
</template>

<script>
import UsersTable from "../addNewFriend/addByName/usersTable.vue"

export default {
  components: {
      UsersTable
  },
  data() {
        return {
            search: "",
        };
    },
}
</script>

<style scoped>
.v-icon-action {
    margin: 0 0 0 10px;
    padding: 2px;
}
</style>