<template>
  <div class="suggested-products">
    <h2>Proponowane dla Ciebie</h2>

    <!-- Loading Spinner -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie produktów...</p>
    </div>

    <!-- Carousel Content -->
    <div v-else-if="products.length > 0" class="carousel-container">
      <button class="arrow-btn prev-btn" @click="prevProducts">❮</button>
      <div class="carousel">
        <div class="products-list">
          <div
            class="product-card"
            v-for="product in products"
            :key="product.id"
          >
            <img
              v-if="product.mainImage?.url"
              :src="product.mainImage.url"
              alt="Zdjęcie produktu"
              class="product-image"
            />
            <img
              v-else
              :src="placeholderImage"
              alt="Obraz zastępczy"
              class="product-image"
            />
            <div class="product-info">
              <a @click="goToProduct(product.id)" class="product-name">
                {{ product.name || "Nazwa produktu" }}
              </a>
              <p class="product-description">
                {{ product.lead || "Brak opisu wstępnego produktu" }}
              </p>
              <div class="product-price">
                <span v-if="product.salePrice">
                  <span class="original-price">{{ product.price }} zł</span>
                  <span class="sale-price">{{ product.salePrice }} zł</span>
                  <span class="discount"
                    >(-{{ calculateDiscount(product) }}%)</span
                  >
                </span>
                <span v-else>{{ product.price }} zł</span>
              </div>

              <button
                :class="[
                  'cart-btn',
                  { 'in-cart': isProductInCart(product.id) },
                ]"
                @click="
                  isProductInCart(product.id) ? goToCart() : addToCart(product)
                "
              >
                {{
                  isProductInCart(product.id) ? "W Koszyku" : "Dodaj do koszyka"
                }}
              </button>
            </div>
          </div>
        </div>
      </div>
      <button class="arrow-btn next-btn" @click="nextProducts">❯</button>
    </div>

    <!-- No Products -->
    <div v-else>
      <p class="none-products">Brak proponowanych produktów.</p>
    </div>

    <div class="view-all-products" v-if="!isLoading && products.length > 0">
      <router-link to="/products" class="btn-view-all">
        Zobacz wszystkie produkty
      </router-link>
    </div>

    <!-- Modal -->
    <AppModal
      :visible="showModal"
      :message="modalMessage"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import AppModal from "@/components/modals/AppModal.vue";
import { storageAPI } from "@/plugins/shopAxios";

@Component({
  components: {
    AppModal,
  },
})
export default class SuggestedProducts extends Vue {
  placeholderImage = "https://placehold.co/200x200";
  pageIndex = 1;
  showModal = false;
  modalMessage = "";

  get products() {
    return this.$store.getters["products/suggestedProducts"];
  }

  get isLoading() {
    return this.$store.getters["products/isLoading"];
  }

  get totalPages() {
    return this.$store.getters["products/totalSuggestedPages"];
  }

  async created() {
    await this.fetchSuggestedProducts();
  }

  async fetchSuggestedProducts() {
    await this.$store.dispatch(
      "products/fetchSuggestedProducts",
      this.pageIndex
    );
    const products = this.products;

    for (const product of products) {
      if (product.mainImage?.name) {
        try {
          const response = await storageAPI.getFile(product.mainImage.name);

          const imageUrl = window.URL.createObjectURL(
            new Blob([response.data])
          );

          this.$set(product.mainImage, "url", imageUrl);
        } catch (error) {
          console.error(
            `Błąd podczas pobierania zdjęcia dla produktu ${product.id}:`,
            error
          );
        }
      }
    }
  }

  async nextProducts() {
    if (this.pageIndex < this.totalPages) {
      this.pageIndex++;
      await this.fetchSuggestedProducts();
    } else {
      this.modalMessage = "To już ostatnia sekcja produktów.";
      this.showModal = true;
    }
  }

  async prevProducts() {
    if (this.pageIndex > 1) {
      this.pageIndex--;
      await this.fetchSuggestedProducts();
    } else {
      this.modalMessage = "Jesteś na początku listy produktów.";
      this.showModal = true;
    }
  }

  closeModal() {
    this.showModal = false;
  }

  calculateDiscount(product: any): number {
    if (product.price && product.salePrice) {
      const discount =
        ((product.price - product.salePrice) / product.price) * 100;
      return Math.round(discount);
    }
    return 0;
  }

  async addToCart(product: any) {
    try {
      const cartItem = { productId: product.id, quantity: 1 };
      await this.$store.dispatch("cart/addItem", cartItem);
      this.modalMessage = `${product.name} został dodany do koszyka.`;
      this.showModal = true;
    } catch (error) {
      console.error("Błąd podczas dodawania produktu do koszyka:", error);
      this.modalMessage =
        "Wystąpił błąd podczas dodawania produktu do koszyka.";
      this.showModal = true;
    }
  }

  goToProduct(productId: number) {
    this.$router.push(`/products/${productId}`).then(() => {
      window.scrollTo({ top: 0, behavior: "smooth" });
    });
  }

  goToCart() {
    this.$router.push("/cart");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  isProductInCart(productId: number): boolean {
    return this.$store.getters["cart/isProductInCart"](productId);
  }

  logImageError(productId: number) {
    console.error(`Nie udało się załadować zdjęcia dla produktu ${productId}.`);
  }
}
</script>

<style scoped lang="scss">
.suggested-products {
  text-align: center;
  padding: 2rem 1rem;
  min-height: 500px;
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
    max-width: 1600px;
  }

  .products-list {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
    justify-content: center;
  }

  .product-card {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 0 0 calc(100% / 5);
    max-width: 300px;
    background-color: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    text-align: center;
    padding: 1.5rem;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    min-height: 430px;

    .product-image {
      width: 100%;
      height: auto;
      max-height: 300px;
      object-fit: cover;
      border-radius: 8px;
      margin-bottom: 1rem;
    }

    .product-info {
      display: flex;
      flex-direction: column;
      flex-grow: 1;
      margin-top: 0.5rem;

      .product-name {
        cursor: pointer;
        font-size: 1.1rem;
        color: #333;
        font-weight: bold;
        text-decoration: none;
        margin-bottom: 1rem;
        min-height: 50px;
        transition: color 0.3s;

        &:hover {
          color: #c70a0a;
        }
      }

      .product-description {
        font-size: 0.9rem;
        color: #666;
        flex-grow: 1;
        margin-bottom: 1rem;
        min-height: 70px;
      }

      .product-price {
        font-size: 1.2rem;
        font-weight: bold;
        margin-bottom: 15px;

        .original-price {
          text-decoration: line-through;
          color: #777;
          font-size: 0.9rem;
          margin-right: 10px;
        }

        .sale-price {
          color: #c70a0a;
          font-size: 1.4rem;
          font-weight: bold;
        }

        .discount {
          color: #555;
          font-size: 1rem;
          margin-left: 5px;
        }
      }

      .cart-btn {
        background-color: #c70a0a;
        color: #fff;
        border: none;
        padding: 0.5rem 1.5rem;
        font-size: 1rem;
        border-radius: 4px;

        &.in-cart {
          background-color: #fff;
          color: #c70a0a;
          border: 1px solid #c70a0a;
        }

        &:hover {
          background-color: darken(#c70a0a, 10%);
          &.in-cart {
            background-color: #ffecec;
          }
        }
      }
    }
  }

  .none-products {
    margin-top: 200px;
  }

  .arrow-btn {
    font-size: 2rem;
    color: #c70a0a;
    background: none;
    border: none;
    cursor: pointer;
    z-index: 10;

    &:hover {
      color: #a00000;
    }

    &.prev-btn {
      position: absolute;
      left: 0;
    }

    &.next-btn {
      position: absolute;
      right: 0;
    }
  }

  .view-all-products {
    margin-top: 100px;

    .btn-view-all {
      display: inline-block;
      padding: 20px 30px;
      font-size: 1.2rem;
      background-color: #c70a0a;
      color: #fff;
      border-radius: 5px;
      text-decoration: none;
      transition: background-color 0.3s;

      &:hover {
        background-color: #a50d0d;
      }
    }
  }

  .loading-overlay {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    min-height: 600px;

    .spinner {
      box-sizing: border-box;
      border: 6px solid #f3f3f3;
      border-radius: 50%;
      border-top: 6px solid #c70a0a;
      width: 40px;
      height: 40px;
      animation: spin 1s linear infinite;
    }

    p {
      margin-top: 10px;
      font-size: 1.2rem;
      color: #555;
    }
  }

  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
  @media (max-width: 1300px) {
    .product-card {
      flex: 0 0 calc(33.33% - 1rem);
    }
  }

  @media (max-width: 992px) {
    .product-card {
      flex: 0 0 calc(50% - 1rem);
    }
  }

  @media (max-width: 840px) {
    .product-card {
      flex: 0 0 calc(100% - 1rem);
      max-width: 250px;
    }

    .cart-btn {
      padding: 0.3rem 1rem;
      font-size: 0.8rem;
    }

    .view-all-products {
      margin-top: 60px;

      .btn-view-all {
        padding: 10px 15px;
        font-size: 1rem;
      }
    }

    h2 {
      font-size: 2rem;
    }
  }
}
</style>
