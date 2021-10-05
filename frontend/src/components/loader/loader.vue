<template>
    <v-progress-circular
      :size="loaderPerams.size"
      :color="loaderPerams.color"
      :width="loaderPerams.width"
      indeterminate
    ></v-progress-circular>
</template>

<script>
export default {
    props: {
        loaderPerams: {
            size: {
                default: 25
            },
            color: {
                default: "black"
            },
            width: {
                default: 3
            },
        },
    },
    data: function ()  {
        return {
        }
    },
}
</script>
