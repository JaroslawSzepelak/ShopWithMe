<template>
  <div class="carousel">
    <div
      class="carousel-content"
      @mouseenter="stopAutoSlide"
      @mouseleave="startAutoSlide"
    >
      <div class="carousel-track" :style="trackStyle">
        <div class="carousel-slide">
          <router-link to="/special-offers">
            <img
              src="@/assets/banners/Offers1.png"
              alt="Special Offers"
              class="product-image"
            />
          </router-link>
        </div>
        <div class="carousel-slide">
          <router-link to="/special-offers">
            <img
              src="@/assets/banners/Offers2.png"
              alt="Special Offers"
              class="product-image"
            />
          </router-link>
        </div>
        <div class="carousel-slide">
          <router-link to="/special-offers">
            <img
              src="@/assets/banners/Offers3.png"
              alt="Special Offers"
              class="product-image"
            />
          </router-link>
        </div>
      </div>
      <button class="arrow-btn prev-btn" @click="prevProduct">❮</button>
      <button class="arrow-btn next-btn" @click="nextProduct">❯</button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class ProductCarousel extends Vue {
  offers = [
    {
      image: "https://placehold.co/1200x600/abd4c7/7b8277",
    },
    {
      image: "https://placehold.co/1200x600/e6dcae/7b8277",
    },
  ];

  currentIndex = 0;
  intervalId: number | null = null;

  get trackStyle() {
    return {
      transform: `translateX(-${this.currentIndex * 100}%)`,
      transition: "transform 0.6s ease-in-out",
    };
  }

  mounted() {
    this.startAutoSlide();
  }

  beforeDestroy() {
    this.stopAutoSlide();
  }

  startAutoSlide() {
    if (!this.intervalId) {
      this.intervalId = window.setInterval(this.nextProduct, 5000);
    }
  }

  stopAutoSlide() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  nextProduct() {
    this.currentIndex = (this.currentIndex + 1) % (this.offers.length + 1);
  }

  prevProduct() {
    this.currentIndex =
      (this.currentIndex - 1 + this.offers.length + 1) %
      (this.offers.length + 1);
  }
}
</script>

<style scoped lang="scss">
.carousel {
  position: relative;
  width: 100%;
  height: 600px;
  overflow: hidden;

  .carousel-content {
    position: relative;
    width: 100%;
    height: 100%;

    .carousel-track {
      display: flex;
      transition: transform 0.6s ease-in-out;
      height: 100%;
    }

    .carousel-slide {
      display: flex;
      align-items: center;
      justify-content: center;
      min-width: 100%;
      height: 100%;

      .product-image {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain;
      }
    }

    .arrow-btn {
      position: absolute;
      top: 50%;
      transform: translateY(-50%);
      font-size: 2rem;
      background: none;
      border: none;
      cursor: pointer;
      color: red;
      padding: 0 10px;
      z-index: 2;

      &:hover {
        color: darkred;
      }
    }

    .prev-btn {
      left: 40px;
    }

    .next-btn {
      right: 40px;
    }
  }
}
</style>
