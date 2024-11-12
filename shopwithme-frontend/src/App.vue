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
  display: flex;
  flex-direction: column;
  min-height: 100vh; /* Cała wysokość ekranu */
}

main {
  flex-grow: 1;
  padding: 20px;
}
</style>
