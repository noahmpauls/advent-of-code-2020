<!--
Purpose: provide a reusable interface for uploading file content to the Vue app.
-->

<template>
  <div>
    <input 
      type="file"
      name="avatar"
      accept=".txt"
      v-on:change="readFile"
    >
    <button v-on:click="getResponse">Get Response</button>
    <br/>
    <br/>
    <div class="file-display">
      <div class="lines">
        <div class="line"
          v-for="(l, i) in fileLines"
          v-bind:key="i"
        >
          {{l}}
        </div>
      </div>
      <div class="lines">
        <div class="line"
          v-for="(l, i) in responseLines"
          v-bind:key="i"
        >
          {{l}}
        </div>
      </div>
    </div>
  </div>
</template>


<script>
// https://stackoverflow.com/a/42316936/11898061
import axios from "axios";

export default {
  name: "FileUpload",

  data() {
    return {
      fileText: "",
      fileLines: [],
      responseLines: []
    }
  },

  methods: {
    readFile(event) {
      let file = event.target.files[0];
      let reader = new FileReader();

      reader.onload = (event) => {
        let text = event.target.result
        this.fileText = text;
        this.fileLines = text.split(/\r?\n/);
      };

      reader.onerror = (event) => {
        alert(event.target.event.name);
      };

      reader.readAsText(file);
    },

    getResponse: async function() {
      const fields = {
        text: this.fileText,
        targetSum: 2020,
        comboSize: 3
      }
      axios.post("/api/days/echo", fields)
      .then(res => {
        this.responseLines = res.data.text.split(/\r?\n/);
      })
      .catch(err => {
        this.responseLines = ["error!"]
        console.error(err);
      })
    }
  }
}
</script>


<style scoped>
.file-display{
  display: flex;
}

.lines {
  margin: 0 1%;
}

.line {
  font-family: monospace;
}
</style>