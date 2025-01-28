<template>
  <div class="product-details" :key="$route.params.id">
    <div v-if="product" class="product-container">
      <div class="image-section">
        <img :src="mainImage" alt="Zdjęcie produktu" class="main-image" />
        <div class="image-thumbnails">
          <button class="arrow-btn" @click="prevImage">❮</button>
          <img
            v-for="(image, index) in thumbnailsList"
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
        <div class="pricing">
          <p v-if="product.salePrice !== null" class="price sale-price">
            Cena promocyjna: {{ product.salePrice }} zł
            <span class="discount">
              ({{ calculateDiscount(product.price, product.salePrice) }}%
              taniej)
            </span>
          </p>
          <p v-if="product.salePrice !== null" class="price regular-price">
            <s>{{ product.price }} zł</s>
          </p>
          <p v-else class="price">{{ product.price }} zł</p>
        </div>
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
import { storageAPI } from "@/plugins/shopAxios";

@Component({
  components: {
    TechnicalDetails,
  },
})
export default class ProductDetails extends Vue {
  mainImage = "";
  quantity = 1;
  thumbnailsList: string[] = [];

  get product() {
    return (
      this.$store.getters["products/selectedProduct"] || {
        id: null,
        name: "Produkt zastępczy",
        price: 0,
        images: [],
        description: "Brak dostępnych szczegółów dla tego produktu.",
        mainImage: null,
      }
    );
  }

  get defaultThumbnails() {
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

      const product = this.product;
      const imageUrls: string[] = [];

      if (product.mainImage?.name) {
        const response = await storageAPI.getFile(product.mainImage.name);
        const mainImageUrl = window.URL.createObjectURL(
          new Blob([response.data])
        );
        product.mainImage.url = mainImageUrl;
        this.mainImage = mainImageUrl;
        imageUrls.push(mainImageUrl);
      }

      if (product.images && product.images.length > 0) {
        for (const image of product.images) {
          const response = await storageAPI.getFile(image.name);
          const imageUrl = window.URL.createObjectURL(
            new Blob([response.data])
          );
          imageUrls.push(imageUrl);
        }
      }

      if (imageUrls.length === 0) {
        this.mainImage = this.defaultThumbnails[0];
        this.thumbnailsList = this.defaultThumbnails;
      } else {
        this.thumbnailsList = imageUrls;
      }
    } catch (error) {
      console.error("Błąd podczas pobierania szczegółów produktu:", error);
      this.mainImage = this.defaultThumbnails[0];
      this.thumbnailsList = this.defaultThumbnails;
    }
  }

  calculateDiscount(originalPrice: number, salePrice: number): number {
    return Math.round(((originalPrice - salePrice) / originalPrice) * 100);
  }

  setMainImage(image: string) {
    this.mainImage = image;
  }

  nextImage() {
    const currentIndex = this.thumbnailsList.indexOf(this.mainImage);
    const nextIndex = (currentIndex + 1) % this.thumbnailsList.length;
    this.mainImage = this.thumbnailsList[nextIndex];
  }

  prevImage() {
    const currentIndex = this.thumbnailsList.indexOf(this.mainImage);
    const prevIndex =
      (currentIndex - 1 + this.thumbnailsList.length) %
      this.thumbnailsList.length;
    this.mainImage = this.thumbnailsList[prevIndex];
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
      console.error("Błąd podczas dodawania produktu do koszyka:", error);
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

    .price {
      font-size: 2rem;
      font-weight: bold;
      margin-bottom: 1rem;
    }

    .pricing .sale-price {
      color: #c70a0a;
      font-size: 1.8rem;
      margin-bottom: 0.5rem;
    }

    .pricing .regular-price {
      color: #666;
      font-size: 1.2rem;
      text-decoration: line-through;
      margin-bottom: 0.5rem;
    }

    .pricing .discount {
      color: #28a745;
      font-size: 1rem;
      margin-left: 10px;
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

  .description-section {
    margin-top: 50px;
    margin-bottom: 50px;
    padding-left: 50px;
    padding-right: 50px;
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

@media (max-width: 1200px) {
  .product-container {
    flex-direction: column;
    align-items: center;
    gap: 1.5rem;
  }

  .image-section {
    .main-image {
      max-width: 300px;
    }

    .image-thumbnails {
      gap: 0.5rem;

      .thumbnail {
        width: 50px !important;
        height: 50px !important;
      }
    }
  }

  .details-section {
    h2 {
      font-size: 1.6rem;
    }

    .price {
      font-size: 1.8rem;
    }
  }
}

@media (max-width: 800px) {
  .image-section {
    .main-image {
      max-width: 250px;
    }

    .image-thumbnails {
      .thumbnail {
        width: 40px !important;
        height: 40px !important;
      }
    }
  }

  .details-section {
    h2 {
      font-size: 1.4rem;
    }

    .price {
      font-size: 1.6rem;
    }

    .quantity-control {
      align-self: center;
    }
  }

  .buttons {
    flex-direction: column;
    align-self: center;

    .cart-btn,
    .back-btn {
      width: 100%;
      text-align: center;
    }
  }
}

@media (max-width: 500px) {
  .image-section {
    .main-image {
      max-width: 200px;
      padding: 40px;
    }
  }

  .details-section {
    h2 {
      font-size: 1.2rem;
    }

    .price {
      font-size: 1.4rem;
    }

    .quantity-control .quantity-btn {
      width: 25px;
      height: 25px;
      font-size: 0.9rem;
    }

    .quantity-control .quantity-input {
      width: 35px;
      height: 25px;
    }
  }

  .buttons {
    gap: 0.5rem;

    .cart-btn,
    .back-btn {
      font-size: 0.9rem;
      padding: 0.6rem 1rem;
    }
  }

  .description-section p {
    font-size: 0.9rem;
  }
}
</style>
