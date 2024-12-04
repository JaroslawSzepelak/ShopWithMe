<template>
  <div class="product-details" :key="$route.params.id">
    <div class="product-container">
      <div class="image-section">
        <img :src="mainImage" alt="Product image" class="main-image" />
        <div class="image-thumbnails">
          <button class="arrow-btn" @click="prevImage">❮</button>
          <img
            v-for="(image, index) in product.images"
            :key="index"
            :src="image"
            alt="Thumbnail"
            class="thumbnail"
            :class="{ active: mainImage === image }"
            @click="setMainImage(image)"
          />
          <button class="arrow-btn" @click="nextImage">❯</button>
        </div>
      </div>
      <div class="details-section">
        <h2>{{ product.name }}</h2>
        <p class="delivery-info">
          Darmowa wysyłka w ciągu 2 dni | 2 lata gwarancji
        </p>
        <div class="rating">
          <i class="fas fa-star" v-for="n in 5" :key="n"></i>
          <span class="reviews">4.5 | 392 opinii</span>
        </div>
        <p class="price">{{ product.price }} zł</p>
        <div class="product-benefits">
          <p><i class="fas fa-check-circle"></i> Darmowy zwrot</p>
          <p><i class="fas fa-check-circle"></i> Kontakt 24/7</p>
        </div>
        <div class="buttons">
          <button class="buy-now">Kup teraz</button>
          <button class="add-to-cart" @click="addToCart">
            Dodaj do koszyka
          </button>
          <button class="back-btn" @click="goBack">Wróć</button>
        </div>
      </div>
    </div>
    <div class="tabs">
      <button>DANE TECHNICZNE</button>
      <button>NAJCZĘŚCIEJ KUPOWANE RAZEM</button>
      <button>OPINIE</button>
      <button>PYTANIA I ODPOWIEDZI</button>
    </div>
    <TechnicalDetails />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import TechnicalDetails from "@/components/TechnicalDetails.vue";

@Component({
  components: {
    TechnicalDetails,
  },
})
export default class ProductDetails extends Vue {
  mainImage = "";

  get product() {
    return this.$store.getters["products/selectedProduct"];
  }

  get images() {
    return this.product.images || [];
  }

  async created() {
    const productId = Number(this.$route.params.id);
    await this.fetchProduct(productId);
  }

  async beforeRouteUpdate(to: any, from: any, next: any) {
    const productId = Number(to.params.id);
    if (productId !== this.product.id) {
      await this.fetchProduct(productId);
    }
    next();
  }

  async fetchProduct(productId: number) {
    try {
      await this.$store.dispatch("products/fetchProduct", productId);
      this.mainImage = this.images[0] || "";
    } catch (error) {
      console.error("Error fetching product details:", error);
    }
  }

  setMainImage(image: string) {
    this.mainImage = image;
  }

  nextImage() {
    const currentIndex = this.images.indexOf(this.mainImage);
    const nextIndex = (currentIndex + 1) % this.images.length;
    this.mainImage = this.images[nextIndex];
  }

  prevImage() {
    const currentIndex = this.images.indexOf(this.mainImage);
    const prevIndex =
      (currentIndex - 1 + this.images.length) % this.images.length;
    this.mainImage = this.images[prevIndex];
  }

  goBack() {
    this.$router.go(-1);
  }

  async addToCart() {
    try {
      await this.$store.dispatch("cart/addItem", this.product.id);
      this.$router.push("/cart");
    } catch (error) {
      console.error("Error adding product to cart:", error);
    }
  }
}
</script>

<style scoped lang="scss">
@import "~@fortawesome/fontawesome-free/css/all.css";

.product-details {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 30px;
  font-family: Arial, sans-serif;
  background-color: #f9f9f9;

  .product-container {
    display: flex;
    max-width: 1200px;
    width: 100%;
    gap: 2rem;
  }

  .image-section {
    flex: 1;
    display: flex;
    flex-direction: column;
    align-items: center;

    .main-image {
      width: 100%;
      max-width: 400px;
      border-radius: 8px;
      margin-bottom: 1rem;
    }

    .image-thumbnails {
      display: flex;
      align-items: center;
      gap: 1rem;

      .arrow-btn {
        background: none;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #555;
      }

      .thumbnail {
        width: 60px;
        height: 60px;
        object-fit: cover;
        border: 2px solid transparent;
        border-radius: 4px;
        cursor: pointer;
        transition: border 0.3s;

        &.active {
          border-color: #c70a0a;
        }
      }
    }
  }

  .details-section {
    flex: 1;
    display: flex;
    flex-direction: column;
    align-items: flex-start;

    h2 {
      font-size: 1.8rem;
      font-weight: bold;
      margin-bottom: 0.5rem;
    }

    .delivery-info {
      color: #888;
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
    }

    .rating {
      font-size: 1.1rem;
      margin-bottom: 1rem;

      .fas.fa-star {
        color: #ffd700;
        margin-right: 0.2rem;
      }

      .reviews {
        color: #888;
        font-size: 0.9rem;
      }
    }

    .price {
      font-size: 2rem;
      font-weight: bold;
      color: #c70a0a;
      margin-bottom: 1rem;
    }

    .product-benefits {
      color: #666;
      font-size: 0.9rem;
      margin-bottom: 1.5rem;

      .fas.fa-check-circle {
        color: #28a745;
        margin-right: 0.5rem;
      }
    }

    .buttons {
      display: flex;
      gap: 1rem;
      margin-top: 1.5rem;

      .buy-now {
        padding: 0.8rem 1.5rem;
        border: none;
        font-size: 1rem;
        background-color: #c70a0a;
        color: #fff;
        border-radius: 5px;
        cursor: pointer;
        &:hover {
          background-color: darken(#c70a0a, 10%);
        }
      }

      .add-to-cart {
        padding: 0.8rem 1.5rem;
        border: 2px solid #c70a0a;
        font-size: 1rem;
        color: #c70a0a;
        background-color: #fff;
        border-radius: 5px;
        cursor: pointer;
        &:hover {
          background-color: #f9f9f9;
        }
      }

      .back-btn {
        padding: 0.8rem 1.5rem;
        background-color: #444;
        color: #fff;
        border-radius: 5px;
        cursor: pointer;
        &:hover {
          background-color: darken(#444, 20%);
        }
      }
    }
  }

  .tabs {
    display: flex;
    justify-content: center;
    gap: 1rem;
    margin-top: 3rem;
    width: 100%;
    background-color: #c70a0a;

    button {
      flex: 1;
      padding: 1rem;
      font-size: 1rem;
      color: #fff;
      background-color: #c70a0a;
      text-transform: uppercase;
      font-weight: bold;
      border: none;
      &:hover {
        background-color: darken(#c70a0a, 10%);
      }
    }
  }
}
</style>
