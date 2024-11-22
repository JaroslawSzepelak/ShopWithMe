<template>
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
              <p class="price-per-unit">{{ item.price }} zł za sztukę</p>
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
              <p class="total-price">{{ item.quantity * item.price }} zł</p>
            </div>
            <button @click="removeFromCart(item.productId)" class="remove-btn">
              <i class="fas fa-trash"></i>
            </button>
          </div>
        </div>
        <div class="cart-summary">
          <h2>Łączna wartość: {{ cartTotal }} PLN</h2>
          <div class="summary-buttons">
            <button @click="checkout" class="checkout-btn">
              Dostawa zamówienia
            </button>
            <button @click="goBack" class="back-btn">Wróć</button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <EmptyCartMessage v-else />
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { CartItem } from "@/store/modules/cart";
import OrderProgress from "@/components/OrderProgress.vue";
import EmptyCartMessage from "@/components/EmptyCartMessage.vue";

@Component({
  components: {
    OrderProgress,
    EmptyCartMessage,
  },
})
export default class Cart extends Vue {
  placeholderImage = "https://placehold.co/100x100";

  get cartItems(): CartItem[] {
    return this.$store.getters["cart/cartItems"] || [];
  }

  get cartTotal(): number {
    return this.$store.getters["cart/cartTotal"] || 0;
  }

  mounted() {
    this.$store.dispatch("cart/fetchCart").catch((error) => {
      console.error("Błąd podczas pobierania koszyka:", error);
    });
  }

  removeFromCart(productId: number) {
    this.$store.dispatch("cart/removeItem", productId).catch((error) => {
      console.error("Błąd podczas usuwania produktu z koszyka:", error);
    });
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
    this.$store.dispatch("cart/updateItem", {
      productId: item.productId,
      quantity: item.quantity,
    });
  }

  goToShop() {
    this.$router.push("/");
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
    max-width: 1200px;
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
    }

    .back-btn {
      background-color: #666;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
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
</style>
