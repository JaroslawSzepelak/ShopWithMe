<template>
  <div class="product-details" :key="$route.params.id">
    <div v-if="product" class="product-container">
      <div class="image-section">
        <img :src="mainImage" alt="Product image" class="main-image" />
        <div class="image-thumbnails">
          <button class="arrow-btn" @click="prevImage">❮</button>
          <img
            v-for="(image, index) in images"
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
        <div class="quantity-control">
          <button @click="decreaseQuantity" class="quantity-btn">-</button>
          <input
            type="number"
            class="quantity-input"
            v-model.number="quantity"
            min="1"
          />
          <button @click="increaseQuantity" class="quantity-btn">+</button>
        </div>
        <div class="buttons">
          <button
            :class="['cart-btn', { 'in-cart': isProductInCart }]"
            @click="isProductInCart ? goToCart() : addToCart()"
          >
            {{ isProductInCart ? "W Koszyku" : "Dodaj do koszyka" }}
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
    <div class="description-section">
      <h2>Opis produktu</h2>
      <p>{{ product.description || "Brak opisu produktu." }}</p>
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
  quantity = 1;

  get product() {
    return (
      this.$store.getters["products/selectedProduct"] || {
        id: null,
        name: "Produkt zastępczy",
        price: 0,
        images: this.defaultImages,
        description: "Brak dostępnych szczegółów dla tego produktu.",
      }
    );
  }

  get images() {
    return this.product.images || this.defaultImages;
  }

  get defaultImages() {
    return [
      "https://placehold.co/400/abb7e7/7b8277",
      "https://placehold.co/400/abd4c7/7b8277",
      "https://placehold.co/400/e6dcae/7b8277",
      "https://placehold.co/400",
    ];
  }

  get isProductInCart(): boolean {
    return this.$store.getters["cart/isProductInCart"](this.product.id);
  }

  beforeRouteEnter(to, from, next) {
    next(() => {
      window.scrollTo({ top: 0, behavior: "smooth" });
    });
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
      const cartItem = { productId: this.product.id, quantity: this.quantity };
      await this.$store.dispatch("cart/addItem", cartItem);
      this.$router.push("/cart");
    } catch (error) {
      console.error("Error adding product to cart:", error);
    }
  }

  goToCart() {
    this.$router.push("/cart");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  increaseQuantity() {
    this.quantity++;
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
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
        box-sizing: border-box;
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

    .quantity-control {
      display: flex;
      align-items: center;
      gap: 10px;

      .quantity-btn {
        width: 35px;
        height: 35px;
        background-color: #e0e0e0;
        border: none;
        border-radius: 50%;
        font-size: 1.2rem;
        cursor: pointer;
        color: #333;
      }

      .quantity-input {
        box-sizing: border-box;
        width: 50px;
        height: 35px;
        text-align: center;
        font-size: 1rem;
        border: 1px solid #ccc;
        border-radius: 4px;
      }
    }

    .buttons {
      display: flex;
      gap: 1rem;
      margin-top: 1.5rem;

      .cart-btn {
        padding: 0.8rem 1.5rem;
        font-size: 1rem;
        border-radius: 5px;
        cursor: pointer;

        &.in-cart {
          background-color: #fff;
          color: #c70a0a;
          border: 1px solid #c70a0a;
        }

        &:not(.in-cart) {
          background-color: #c70a0a;
          color: #fff;
          border: none;
        }

        &:hover {
          opacity: 0.9;
          &.in-cart {
            background-color: #ffecec;
          }
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

    button {
      flex: 1;
      padding: 1rem;
      font-size: 1rem;
      color: #c70a0a;
      background-color: #fff;
      text-transform: uppercase;
      font-weight: bold;
      border: 2px solid #c70a0a;
      border-radius: 5px;
      cursor: pointer;
      &:hover {
        background-color: #f9f9f9;
      }
    }
  }

  .description-section {
    margin-top: 50px;
    margin-bottom: 50px;
    text-align: center;
    max-width: 1000px;

    h2 {
      margin-bottom: 20px;
    }

    p {
      font-size: 1rem;
      text-align: left;
      color: #666;
    }
  }
}
</style>
