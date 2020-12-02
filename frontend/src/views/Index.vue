<template>
  <div>
    <h2>Click a day to use its code!</h2>
    <div class="days">
      <div
        class="day-button"
        v-for="d in days"
        v-bind:key="d"
      >
        <router-link v-bind:to="{path: '/' + d}">{{d}}</router-link>
      </div>
    </div>
  </div>
</template>

<script>
import days from "@/components/days"
const DAYS = [...Array(days.length).keys()].map(i => i+1);

export default {
  name: "Index",
  data() {
    return {
      days: DAYS
    }
  }
}
</script>

<style>
.days {
  width: min(650px, 100%);
  margin: 0 auto;

  display: flex;
  flex-flow: row wrap;
  justify-content: center;
}

.day-button {
  width: 100px;
  height: 100px;
  border: 5px solid black;
  border-radius: 10px;
  margin: 10px;

  display: flex;
  justify-content: center;
  align-items: center;
}
</style>