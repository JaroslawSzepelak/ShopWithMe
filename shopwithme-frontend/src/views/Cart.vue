<template>
  <div>
    <div v-if="cartItems.length" class="main-block">
      <OrderProgress :currentStep="1" class="order-progress" />
      <div class="cart">
        <h1>Twój koszyk</h1>
        <div class="cart-container">
          <div class="cart-items">
            <div
              v-for="item in cartItems"
              :key="item.productId"
              class="cart-item"
            >
              <img
                :src="item.image || placeholderImage"
                alt="Product image"
                class="product-image"
              />
              <div class="item-details">
                <h3 class="product-name">{{ item.name }}</h3>
                <p class="availability">Dostępny</p>
                <div class="pricing">
                  <p v-if="item.salePrice !== 0" class="sale-price">
                    Promocja: {{ item.salePrice }} zł za sztukę
                  </p>
                  <p v-if="item.salePrice !== 0" class="regular-price">
                    <s>{{ item.price }} zł</s>
                  </p>
                  <p v-else class="price-per-unit">
                    {{ item.price }} zł za sztukę
                  </p>
                </div>
                <div class="quantity-control">
                  <button @click="decreaseQuantity(item)" class="quantity-btn">
                    -
                  </button>
                  <input
                    type="number"
                    class="quantity-input"
                    v-model.number="item.quantity"
                    @input="updateQuantity(item)"
                    min="1"
                  />
                  <button @click="increaseQuantity(item)" class="quantity-btn">
                    +
                  </button>
                </div>
                <p class="total-price">
                  {{ item.quantity * getEffectivePrice(item) }} zł
                </p>
                <p
                  v-if="loadingProductId === item.productId"
                  class="loading-message"
                >
                  Aktualizuję ilość...
                </p>
                <p
                  v-if="updatedProductId === item.productId"
                  class="success-message"
                >
                  Ilość zaktualizowana!
                </p>
              </div>
              <button @click="showDeleteConfirmation(item)" class="remove-btn">
                <i class="fas fa-trash"></i>
              </button>
            </div>
          </div>
          <div class="cart-summary">
            <h2>Łączna wartość: {{ cartTotal }} zł</h2>
            <div class="summary-buttons">
              <button @click="checkout" class="checkout-btn">
                Dostawa zamówienia
              </button>
              <button
                @click="clearCartWithConfirmation"
                class="checkout-btn clear-btn"
              >
                Wyczyść koszyk
              </button>
              <button @click="goBack" class="checkout-btn back-btn">
                Wróć
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showConfirmModal" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>Czy na pewno chcesz usunąć produkt?</h2>
        <p>{{ currentProduct?.name }}</p>
        <div class="modal-buttons">
          <button @click="confirmDelete" class="modal-btn">Tak</button>
          <button @click="closeModal" class="modal-btn cancel">Nie</button>
        </div>
      </div>
    </div>

    <div v-if="showInfoModal" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>Produkt został usunięty!</h2>
        <p>{{ currentProduct?.name }}</p>
        <div class="modal-buttons">
          <button @click="closeModal" class="modal-btn">Ok</button>
          <button @click="restoreProduct" class="modal-btn restore">
            Przywróć produkt
          </button>
        </div>
      </div>
    </div>

    <ConfirmationModal
      :visible="showClearCartModal"
      message="Czy na pewno chcesz wyczyścić koszyk?"
      @confirm="clearCart"
      @close="showClearCartModal = false"
    />

    <EmptyCartMessage
      v-if="!cartItems.length && !showConfirmModal && !showInfoModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { CartItem } from "@/store/modules/cart";
import OrderProgress from "@/components/OrderProgress.vue";
import EmptyCartMessage from "@/components/EmptyCartMessage.vue";
import ConfirmationModal from "@/components/modals/ConfirmationModal.vue";

@Component({
  components: {
    OrderProgress,
    EmptyCartMessage,
    ConfirmationModal,
  },
})
export default class Cart extends Vue {
  placeholderImage = "https://placehold.co/100x100";
  showConfirmModal = false;
  showClearCartModal = false;
  showInfoModal = false;
  currentProduct: CartItem | null = null;
  removedProduct: CartItem | null = null;
  loadingProductId: number | null = null;
  updatedProductId: number | null = null;
  updateTimer: { [key: number]: number | undefined } = {};

  get cartItems(): CartItem[] {
    return this.$store.getters["cart/cartItems"] || [];
  }

  get cartTotal(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.quantity * this.getEffectivePrice(item),
      0
    );
  }

  mounted() {
    this.$store
      .dispatch("cart/fetchCart")
      .then(() => {
        this.loadProductImages();
      })
      .catch((error) => {
        console.error("Błąd podczas pobierania koszyka:", error);
      });
  }

  getEffectivePrice(item: CartItem): number {
    return item.salePrice !== 0 ? item.salePrice : item.price;
  }

  async loadProductImages() {
    for (const item of this.cartItems) {
      if (item.image || !item.mainImageId) {
        continue;
      }

      try {
        const imageResponse = await this.$store.dispatch(
          "cart/fetchProductImage",
          item.mainImageId
        );
        item.image = URL.createObjectURL(new Blob([imageResponse.data]));
      } catch (error) {
        console.error(
          `Błąd podczas pobierania zdjęcia dla produktu ${item.productId}:`,
          error
        );
      }
    }
  }

  async removeFromCart(productId: number) {
    try {
      await this.$store.dispatch("cart/removeItem", productId);
    } catch (error) {
      console.error("Błąd podczas usuwania produktu z koszyka:", error);
    }
  }

  showDeleteConfirmation(product: CartItem) {
    this.currentProduct = product;
    this.showConfirmModal = true;
  }

  async confirmDelete() {
    if (this.currentProduct) {
      this.removedProduct = { ...this.currentProduct };
      await this.removeFromCart(this.currentProduct.productId);
      this.currentProduct = null;

      this.showConfirmModal = false;
      this.showInfoModal = true;
    }
  }

  restoreProduct() {
    if (this.removedProduct) {
      this.$store.dispatch("cart/addItem", {
        productId: this.removedProduct.productId,
        quantity: this.removedProduct.quantity,
      });
      this.removedProduct = null;
    }
    this.showInfoModal = false;
  }

  clearCartWithConfirmation() {
    this.showClearCartModal = true;
  }

  async clearCart() {
    try {
      await this.$store.dispatch("cart/clearCart");
      this.showClearCartModal = false;
    } catch (error) {
      console.error("Błąd podczas czyszczenia koszyka:", error);
    }
  }

  closeModal() {
    this.showConfirmModal = false;
    this.showInfoModal = false;
  }

  goBack() {
    this.$router.go(-1);
  }

  checkout() {
    this.$router.push("/delivery-form");
  }

  increaseQuantity(item: CartItem) {
    item.quantity++;
    this.updateQuantity(item);
  }

  decreaseQuantity(item: CartItem) {
    if (item.quantity > 1) {
      item.quantity--;
      this.updateQuantity(item);
    }
  }

  updateQuantity(item: CartItem) {
    if (item.quantity < 1) {
      item.quantity = 1;
    }

    if (this.updateTimer[item.productId]) {
      clearTimeout(this.updateTimer[item.productId]);
    }

    this.updateTimer[item.productId] = window.setTimeout(async () => {
      try {
        this.loadingProductId = item.productId;
        await this.$store.dispatch("cart/updateItem", {
          productId: item.productId,
          quantity: item.quantity,
        });

        this.updatedProductId = item.productId;

        setTimeout(() => {
          this.updatedProductId = null;
        }, 3000);

        this.loadingProductId = null;
      } catch (error) {
        console.error("Błąd podczas aktualizacji ilości w koszyku:", error);
        this.loadingProductId = null;
      }
    }, 3000);
  }
}
</script>

<style scoped lang="scss">
.main-block {
  background-color: #f2f2f2;
}
.cart {
  background-color: #f2f2f2;
  display: flex;
  flex-direction: column;
  align-items: center;
  font-family: Arial, sans-serif;
  padding: 20px;
  min-height: 100vh;

  h1 {
    font-size: 2rem;
    margin-bottom: 20px;
  }

  .cart-container {
    display: flex;
    flex-direction: row;
    gap: 20px;
    max-width: 1300px;
    width: 100%;
  }

  .cart-items {
    flex: 3;
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .cart-item {
    display: flex;
    align-items: center;
    padding: 20px;
    background-color: #ffffff;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);

    .product-image {
      width: 120px;
      height: 120px;
      border-radius: 8px;
      margin-right: 15px;
    }

    .item-details {
      flex: 1;
      display: flex;
      flex-direction: column;

      .product-name {
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 5px;
      }

      .availability {
        font-size: 1rem;
        color: green;
        margin-bottom: 5px;
      }

      .price-per-unit {
        font-size: 1rem;
        color: #555;
        margin-bottom: 10px;
      }

      .pricing .sale-price {
        color: #c70a0a;
        font-size: 1rem;
      }

      .pricing .regular-price {
        color: #666;
        font-size: 0.9rem;
        text-decoration: line-through;
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

      .total-price {
        font-size: 1.2rem;
        font-weight: bold;
        color: #c70a0a;
        margin-top: 10px;
      }
    }

    .remove-btn {
      background-color: transparent;
      color: #ff4d4d;
      border: none;
      font-size: 3rem;
      cursor: pointer;
      padding: 10px;
    }
  }

  .cart-summary {
    flex: 1;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    padding: 20px;
    background-color: #ffffff;
    color: #000000;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);

    h2 {
      font-size: 1.8rem;
      margin-bottom: 20px;
    }

    .summary-buttons {
      margin-top: auto;
      display: flex;
      flex-direction: column;
      gap: 10px;
      width: 100%;
    }

    .checkout-btn {
      background-color: #c70a0a;
      color: white;
      border: none;
      padding: 15px 30px;
      border-radius: 5px;
      font-size: 1.1rem;
      cursor: pointer;

      &:hover {
        background-color: #a80808;
      }
    }

    .clear-btn {
      background-color: #fff;
      color: #c70a0a;
      border: 1px solid #c70a0a;

      &:hover {
        background-color: #ffecec;
      }
    }

    .back-btn {
      background-color: #666;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;

      &:hover {
        background-color: #4a4a4a;
      }
    }
  }
  .shop-btn {
    background-color: #c70a0a;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    margin-top: 20px;
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
  max-width: 400px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);

  h2 {
    margin-bottom: 1rem;
  }

  p {
    font-size: 1.2rem;
    margin-bottom: 1rem;
  }

  .modal-buttons {
    display: flex;
    gap: 10px;
    justify-content: center;

    .modal-btn {
      background-color: #c70a0a;
      color: #fff;
      border: none;
      padding: 0.5rem 1rem;
      font-size: 1rem;
      border-radius: 4px;
      cursor: pointer;

      &.cancel {
        background-color: #666;
      }

      &.restore {
        background-color: #4caf50;
      }

      &:hover {
        opacity: 0.9;
      }
    }
  }
}

.loading-message {
  font-size: 0.9rem;
  color: #666;
  margin-top: 5px;
}
.success-message {
  font-size: 0.9rem;
  color: green;
  margin-top: 5px;
}

@media (max-width: 1024px) {
  .cart {
    .cart-container {
      flex-wrap: wrap;
    }

    .cart-items {
      margin-bottom: 40px;
    }

    .cart-item {
      .product-image {
        width: 100px;
        height: 100px;
      }

      .item-details {
        .product-name {
          font-size: 1.2rem;
        }

        .total-price {
          font-size: 1rem;
        }
      }
    }

    .cart-summary {
      h2 {
        font-size: 1.5rem;
      }
    }
  }
}

@media (max-width: 850px) {
  .cart {
    .cart-container {
      flex-direction: column;
    }

    .cart-summary {
      margin-top: 1rem;
      align-items: center;

      h2 {
        font-size: 1.2rem;
      }

      .summary-buttons {
        width: 50%;
      }
    }
  }
}

@media (max-width: 570px) {
  .cart {
    .cart-summary {
      .checkout-btn {
        padding: 10px 20px;
        font-size: 0.8rem;
      }
    }
  }
}

@media (max-width: 500px) {
  .cart {
    .cart-container {
      width: 270px;
    }
    .cart-item {
      flex-direction: column;
      align-items: center;

      .product-image {
        width: 80%;
        height: auto;
        margin: 0;
      }

      .item-details {
        margin-top: 30px;
        width: 80%;

        .product-name {
          font-size: 1.2rem;
          margin-bottom: 5px;
        }

        .availability {
          font-size: 1rem;
          margin-bottom: 5px;
        }

        .price-per-unit {
          font-size: 0.9rem;
          color: #555;
          margin-bottom: 10px;
        }

        .quantity-control {
          display: flex;
          justify-content: space-around;
          width: 100%;

          .quantity-btn {
            width: 30px;
            height: 30px;
            font-size: 1rem;
          }

          .quantity-input {
            width: 40px;
            height: 30px;
            font-size: 0.9rem;
          }
        }

        .total-price {
          font-size: 2rem;
          font-weight: bold;
          text-align: center;
          color: #c70a0a;
          margin-top: 15px;
        }
      }

      .remove-btn {
        align-self: center;
        font-size: 2rem;
        margin-top: 10px;
      }
    }

    .cart-summary {
      margin-top: 40px;
      .summary-buttons {
        width: 70%;
      }
    }
  }
}
</style>
