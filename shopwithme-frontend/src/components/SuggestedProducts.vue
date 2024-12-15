<template>
  <div class="suggested-products">
    <h2>Proponowane dla Ciebie</h2>

    <div v-if="products.length > 0" class="carousel-container">
      <button class="arrow-btn prev-btn" @click="prevProducts">❮</button>
      <div class="carousel">
        <div class="products-list">
          <div
            class="product-card"
            v-for="(product, index) in products"
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
                {{ product.lead || "Opis wstępny produktu" }}
              </p>
              <p class="product-price">{{ product.price || "100,00" }} zł</p>
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

    <div v-else>
      <p>Brak proponowanych produktów.</p>
    </div>

    <!-- Update Modal usage -->
    <AppModal
      :visible="showModal"
      :message="modalMessage"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import AppModal from "./modals/AppModal.vue";

@Component({
  components: {
    AppModal,
  },
})
export default class SuggestedProducts extends Vue {
  placeholderImage = "https://placehold.co/200x200";
  pageIndex = 1;
  products: any[] = [];
  totalRows = 0;
  showModal = false;
  modalMessage = "";

  async mounted() {
    await this.fetchSuggestedProducts();
  }

  async fetchSuggestedProducts() {
    try {
      await this.$store.dispatch(
        "products/fetchSuggestedProducts",
        this.pageIndex
      );
      this.products = this.$store.state.products.products;
      this.totalRows = this.$store.state.products.totalRows;
    } catch (error) {
      console.error("Error fetching suggested products:", error);
    }
  }

  async nextProducts() {
    if (
      this.pageIndex <
      Math.ceil(this.totalRows / this.$store.state.products.suggestedPageSize)
    ) {
      this.pageIndex++;
      await this.fetchSuggestedProducts();
    } else {
      this.modalMessage = "Brak kolejnych produktów.";
      this.showModal = true;
    }
  }

  async prevProducts() {
    if (this.pageIndex > 1) {
      this.pageIndex--;
      await this.fetchSuggestedProducts();
    } else {
      this.modalMessage = "Jesteś na początku kolejki.";
      this.showModal = true;
    }
  }

  closeModal() {
    this.showModal = false;
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

  goToCart() {
    this.$router.push("/cart");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  isProductInCart(productId: number): boolean {
    return this.$store.getters["cart/isProductInCart"](productId);
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
    max-width: 1600px;
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
      height: 180px;
      object-fit: cover;
    }

    .product-info {
      display: flex;
      flex-direction: column;
      flex-grow: 1;
      margin-top: 0.5rem;

      .product-name {
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
        color: #c70a0a;
        margin-bottom: 15px;
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
}
</style>
