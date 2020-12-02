<template>
  <div>
    <h2>Day 1: Report Repair</h2>
    <div class="day-body">
      <h3>Part 1</h3>
      <div class="part-container">
        <div class="part-description">
          An entered .txt file contains a pair of numbers that sum to 2020. Provide the product of these numbers.
        </div>
        <div class="part-action">
          <FileUpload/>
          <button @click="getData(0)">Calculate Part 1</button>
          <button @click="clearData(0)">Clear Answer</button>
          <p>Result: {{display1}}</p>
        </div>
      </div>

      <h3>Part 2</h3>
      <div class="part-container">
        <div class="part-description">
          The same list of numbers also contains a set of 3 numbers that sum to 2020. Calculate the product of these numbers.
        </div>
        <div class="part-action">
          <FileUpload/>
          <button @click="getData(1)">Calculate Part 2</button>
          <button @click="clearData(1)">Clear Answer</button>
          <p>Result: {{display2}}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

import FileUpload from "@/components/FileUpload";

export default {
  name: "DataGetter",
  components: {
    FileUpload,
  },

  data() {
    return {
      text: [
        "Nothing yet...",
        "Nothing yet..."
      ],
      result: [
        null,
        null
      ]
    }
  },

  computed: {
    display1() {
      return (this.product1) ? this.product1 : this.text[0];
    },
    display2() {
      return (this.product2) ? this.product2 : this.text[1];
    },
    product1() {
      if (this.result[0]) {
        return this.result[0].combos.map(c => c.product);
      } else {
        return null;
      }
    },
    product2() {
      if (this.result[1]) {
        return this.result[1].combos.map(c => c.product);
      } else {
        return null;
      }
    }
  },

  methods: {
    getData(part) {
      this.result[part] = null;
      this.text[part] = "Loading...";
      axios.get(`/api/days/1/${part+1}`)
      .then(res => {
        this.result[part] = res.data;
      })
      .catch(err => {
        this.text[part] = "Bad request!";
        console.error(err);
      });
    },
    clearData(part) {
      this.result[part] = null;
      this.text[part] = "Nothing yet...";
    }
  }
}
</script>

<style>
.day-body {
  width: min(650px, 100%);
  margin: 0 auto;
  display: flex;
  flex-direction: column;
}

.part-container {
  display: flex;
}

.part-description {
  flex: 2 0;
}

.part-action {
  flex: 1 0;
}
</style>