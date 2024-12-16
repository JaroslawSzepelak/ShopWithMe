<template>
  <div>
    <ProductCarousel class="productCarousel" />
    <WelcomeSection class="welcome" />
    <PromotionsSection />
    <SuggestedProducts />
    <AboutSection />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import WelcomeSection from "@/components/homeComponents/WelcomeSection.vue";
import ProductCarousel from "@/components/homeComponents/OffersCarousel.vue";
import PromotionsSection from "@/components/homeComponents/PromotionsSection.vue";
import SuggestedProducts from "@/components/homeComponents/SuggestedProducts.vue";
import AboutSection from "@/components/homeComponents/AboutSection.vue";

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
  margin-left: 30px;
  margin-right: 30px;
}
.productCarousel {
  margin-top: 30px;
}
</style>
