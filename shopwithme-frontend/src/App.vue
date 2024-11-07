<template>
  <div id="app">
    <Navbar />
    <main>
      <router-view></router-view>
    </main>
    <Footer />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Navbar from "./components/Navbar.vue";
import Footer from "./components/Footer.vue";

@Component({
  components: {
    Navbar,
    Footer,
  },
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "./assets/styles.scss";

#app {
  font-family: Arial, sans-serif;
  color: #333;
}

.container {
  margin-top: 20px;
}
</style>
