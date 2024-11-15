<template>
  <div>
    <WelcomeSection class="welcome" />
    <ProductCarousel class="productCarousel" />
    <PromotionsSection />
    <SuggestedProducts />
    <AboutSection />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import WelcomeSection from "@/components/WelcomeSection.vue";
import ProductCarousel from "@/components/ProductCarousel.vue";
import PromotionsSection from "@/components/PromotionsSection.vue";
import SuggestedProducts from "@/components/SuggestedProducts.vue";
import AboutSection from "@/components/AboutSection.vue";

@Component({
  components: {
    WelcomeSection,
    ProductCarousel,
    PromotionsSection,
    SuggestedProducts,
    AboutSection,
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
