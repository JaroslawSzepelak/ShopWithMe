<template>
  <div>
    <h1>Twój koszyk</h1>
    <ul v-if="cartItems.length">
      <li v-for="item in cartItems" :key="item.productId">
        {{ item.name }} - {{ item.quantity }} x {{ item.price }} PLN
        <button
          @click="removeFromCart(item.productId)"
          class="btn btn-danger btn-sm"
        >
          Usuń
        </button>
      </li>
    </ul>
    <p v-else>Koszyk jest pusty.</p>
    <button v-if="cartItems.length" @click="checkout" class="btn btn-primary">
      Przejdź do kasy
    </button>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class Cart extends Vue {
  cartItems: Array<{
    productId: number;
    name: string;
    quantity: number;
    price: number;
  }> = [];

  created() {
    this.cartItems = [
      { productId: 1, name: "Produkt 1", quantity: 2, price: 100 },
      { productId: 2, name: "Produkt 2", quantity: 1, price: 200 },
    ];
  }

  removeFromCart(productId: number) {
    this.cartItems = this.cartItems.filter(
      (item) => item.productId !== productId
    );
  }

  checkout() {
    console.log("Przechodzenie do kasy");
  }
}
</script>

<style scoped>
h1 {
  margin-top: 20px;
}
</style>
