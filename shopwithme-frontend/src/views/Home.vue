<template>
  <div>
    <WelcomeSection class="welcome" />
    <ProductCarousel class="productCarousel" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import WelcomeSection from "@/components/WelcomeSection.vue";
import ProductCarousel from "@/components/ProductCarousel.vue";

@Component({
  components: {
    WelcomeSection,
    ProductCarousel,
  },
})
export default class Home extends Vue {}
</script>

<style scoped>
.welcome {
  max-width: 1200px;
  margin-left: 30px;
}
.productCarousel {
  margin-top: 30px;
}
</style>
