<template>
  <div class="app">
    <Navbar />
    <main>
      <router-view :key="$route.params.id"></router-view>
    </main>
    <Footer />
    <AppModal
      :visible="modalVisible"
      :message="modalMessage"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Navbar from "./components/Navbar.vue";
import Footer from "./components/Footer.vue";
import AppModal from "@/components/modals/AppModal.vue";

@Component({
  components: {
    Navbar,
    Footer,
    AppModal,
  },
})
export default class App extends Vue {
  modalVisible = false;
  modalMessage = "";

  created() {
    //sessionStorage.clear();
  }

  mounted() {
    this.$root.$on("showModal", (message: string) => {
      this.modalMessage = message;
      this.modalVisible = true;
    });
  }

  closeModal() {
    this.modalVisible = false;
    this.$router.go(-1);
  }

  beforeDestroy() {
    this.$root.$off("showModal");
  }
}
</script>

<style lang="scss">
@import "./assets/styles.scss";

.app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex-grow: 1;
}
</style>
