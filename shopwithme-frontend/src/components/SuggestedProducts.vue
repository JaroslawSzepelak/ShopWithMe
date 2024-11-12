<template>
  <div class="suggested-products">
    <h2>Proponowane dla Ciebie</h2>
    <div v-if="visibleProducts.length > 0" class="carousel-container">
      <button class="arrow-btn prev-btn" @click="prevProducts">❮</button>
      <div class="carousel">
        <div class="products-list">
          <div
            class="product-card"
            v-for="(product, index) in visibleProducts"
            :key="index"
          >
            <img
              :src="product.image || placeholderImage"
              alt="Product image"
              class="product-image"
            />
            <div class="product-info">
              <router-link :to="`/products/${product.id}`" class="product-name">
                {{ product.name || "Nazwa produktu" }}
              </router-link>
              <p class="product-description">
                {{ product.description || "Opis produktu" }}
              </p>
              <p class="product-price">{{ product.price || "100,00" }} zł</p>
              <button class="add-to-cart">Dodaj do koszyka</button>
            </div>
          </div>
        </div>
      </div>
      <button class="arrow-btn next-btn" @click="nextProducts">❯</button>
    </div>

    <div v-if="showModal" class="modal-overlay" @click="closeModal">
      <div class="modal-content">
        <p>{{ modalMessage }}</p>
        <button @click="closeModal">Zamknij</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import axios from "axios";

@Component
export default class SuggestedProducts extends Vue {
  products: any[] = [];
  visibleProducts: any[] = [];
  placeholderImage = "https://placehold.co/200x200";
  productsToShow = 5;
  startIndex = 0;
  showModal = false;
  modalMessage = "";

  async mounted() {
    await this.fetchProducts();
    this.updateProductsToShow();
    window.addEventListener("resize", this.updateProductsToShow);
  }

  beforeDestroy() {
    window.removeEventListener("resize", this.updateProductsToShow);
  }

  async fetchProducts() {
    try {
      const response = await axios.get("http://localhost:5000/api/Products");
      this.products = response.data;
      this.updateVisibleProducts();
    } catch (error) {
      console.error("Błąd podczas pobierania produktów:", error);
    }
  }

  updateProductsToShow() {
    const screenWidth = window.innerWidth;
    if (screenWidth < 800) {
      this.productsToShow = 1;
    } else if (screenWidth < 1400) {
      this.productsToShow = 3;
    } else {
      this.productsToShow = 5;
    }
    this.startIndex = 0;
    this.updateVisibleProducts();
  }

  updateVisibleProducts() {
    this.visibleProducts = this.products.slice(
      this.startIndex,
      this.startIndex + this.productsToShow
    );
  }

  nextProducts() {
    const nextIndex = this.startIndex + this.productsToShow;
    if (nextIndex < this.products.length) {
      this.startIndex = nextIndex;
      this.updateVisibleProducts();
    } else if (this.startIndex < this.products.length - 1) {
      this.startIndex = this.products.length - this.productsToShow;
      this.updateVisibleProducts();
    } else {
      this.modalMessage = "Brak produktów w kolejce";
      this.showModal = true;
    }
  }

  prevProducts() {
    if (this.startIndex > 0) {
      this.startIndex = Math.max(0, this.startIndex - this.productsToShow);
      this.updateVisibleProducts();
    } else {
      this.modalMessage = "Początek kolejki produktów";
      this.showModal = true;
    }
  }

  closeModal() {
    this.showModal = false;
  }
}
</script>

<style scoped lang="scss">
.suggested-products {
  text-align: center;
  padding: 2rem 1rem;
  background-color: #f3f3f3;

  h2 {
    font-size: 2.5rem;
    margin-bottom: 2rem;
    color: #333;
  }

  .carousel-container {
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    max-width: 90%;
    margin: 0 auto;
  }

  .carousel {
    overflow: hidden;
    width: 100%;
    max-width: 1400px;
  }

  .products-list {
    display: flex;
    justify-content: center;
    gap: 1rem;
  }

  .product-card {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 0 0 calc(100% / 5);
    max-width: 240px;
    background-color: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    text-align: center;
    padding: 1.5rem;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);

    .product-image {
      width: 100%;
      height: 180px;
      object-fit: cover;
    }

    .product-info {
      display: flex;
      flex-direction: column;
      justify-content: flex-start;
      flex-grow: 1;
      margin-top: 0.5rem;

      .product-name {
        font-size: 1.1rem;
        color: #333;
        font-weight: bold;
        text-decoration: none;
        transition: color 0.3s;
        margin-bottom: 0.5rem;

        &:hover {
          color: #c70a0a;
        }
      }

      .product-description {
        font-size: 0.9rem;
        color: #666;
        flex-grow: 1;
        margin-bottom: 0.5rem;
      }

      .product-price {
        font-size: 1.2rem;
        font-weight: bold;
        color: #c70a0a;
        margin-top: auto; /* Umieszcza cenę przy dolnej krawędzi */
        margin-bottom: 15px;
      }

      .add-to-cart {
        background-color: #c70a0a;
        color: #fff;
        border: none;
        padding: 0.5rem 1.5rem;
        font-size: 1rem;
        border-radius: 4px;
        margin-top: auto;
        cursor: pointer;

        &:hover {
          background-color: darken(#c70a0a, 10%);
        }
      }
    }
  }

  /* Responsywne dopasowanie liczby produktów w rzędzie */
  @media (max-width: 1640px) {
    .product-card {
      flex: 0 0 calc(100% / 3);
    }
  }

  @media (max-width: 1000px) {
    .product-card {
      flex: 0 0 100%;
    }
  }

  .arrow-btn {
    font-size: 2rem;
    background: none;
    border: none;
    cursor: pointer;
    color: #c70a0a;
    padding: 0;
    z-index: 2;
    position: absolute;
    top: 50%;
    transform: translateY(-50%);

    &:hover {
      color: darken(#c70a0a, 10%);
    }

    &.prev-btn {
      left: 0.5rem;
    }

    &.next-btn {
      right: 0.5rem;
    }
  }

  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal-content {
    background-color: #fff;
    padding: 2rem;
    border-radius: 8px;
    text-align: center;
    max-width: 300px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);

    p {
      font-size: 1.2rem;
      color: #333;
      margin-bottom: 1rem;
    }

    button {
      background-color: #c70a0a;
      color: #fff;
      border: none;
      padding: 0.5rem 1rem;
      font-size: 1rem;
      border-radius: 4px;
      cursor: pointer;

      &:hover {
        background-color: darken(#c70a0a, 10%);
      }
    }
  }
}
</style>
