<template>
  <div>
    <button @click="getData">Get Answer</button>
    <button @click="clearData">Clear Answer</button>
    <input type="text" v-model="day" placeholder="day"/>
    <input type="text" v-model="part" placeholder="part"/>
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
      day: null,
      part: null
    }
  },

  methods: {
    getData() {
      this.result = "Loading...";
      axios.get(`/api/days/${this.day}/${this.part}`)
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