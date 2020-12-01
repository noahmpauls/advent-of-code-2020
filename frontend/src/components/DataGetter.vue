<template>
  <div>
    <button @click="getData">Get Answer</button>
    <button @click="clearData">Clear Answer</button>
    <input type="text" v-model="day"/>
    {{result}}
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "DataGetter",

  data() {
    return {
      result: "Nothing yet...",
      day: null
    }
  },

  methods: {
    getData() {
      this.result = "Loading...";
      axios.get(`/api/days/${this.day}`)
      .then(res => {
        this.result = res.data;
      })
      .catch(err => {
        this.result = "Bad request!";
        console.error(err);
      });
    },
    clearData() {
      this.result = "Nothing yet...";
    }
  }
}
</script>

<style>

</style>