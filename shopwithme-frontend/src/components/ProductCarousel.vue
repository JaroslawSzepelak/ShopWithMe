<template>
  <div class="carousel">
    <div class="carousel-content">
      <img
        :src="currentProduct.image"
        alt="product image"
        class="product-image"
      />
      <div class="product-info">
        <h3>{{ currentProduct.name }}</h3>
        <p>{{ currentProduct.price }} zł</p>
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
  products = [
    {
      name: "Telewizor MAX",
      price: "3000,00",
      image: "https://placehold.co/1200x600/abb7e7/7b8277",
    },
    {
      name: "Laptop Pro",
      price: "4500,00",
      image: "https://placehold.co/1200x600/abd4c7/7b8277",
    },
    {
      name: "Smartfon X",
      price: "1500,00",
      image: "https://placehold.co/1200x600/e6dcae/7b8277",
    },
  ];

  currentIndex = 0;
  intervalId: number | null = null;

  get currentProduct() {
    return this.products[this.currentIndex];
  }

  mounted() {
    this.startAutoSlide();
  }

  beforeDestroy() {
    this.stopAutoSlide();
  }

  startAutoSlide() {
    this.intervalId = window.setInterval(this.nextProduct, 5000);
  }

  stopAutoSlide() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  nextProduct() {
    this.currentIndex = (this.currentIndex + 1) % this.products.length;
  }

  prevProduct() {
    this.currentIndex =
      (this.currentIndex - 1 + this.products.length) % this.products.length;
  }
}
</script>

<style scoped lang="scss">
.carousel {
  position: relative;
  width: 100%;
  height: 400px;
  background-color: #e6f7ff;
  overflow: hidden;

  .carousel-content {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;

    .product-image {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    .product-info {
      position: absolute;
      bottom: 20px;
      left: 20px;
      background: rgba(255, 255, 255, 0.9);
      padding: 10px 20px;
      border-radius: 8px;
      text-align: left;

      h3 {
        margin: 0;
        font-size: 1.5em;
        color: #333;
      }

      p {
        margin: 0;
        font-size: 1.2em;
        color: #333;
        font-weight: bold;
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
    }

    .prev-btn {
      left: 10px;
    }

    .next-btn {
      right: 10px;
    }
  }
}
</style>
