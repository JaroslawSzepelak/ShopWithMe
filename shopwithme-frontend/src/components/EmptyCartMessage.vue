<template>
  <div class="empty-cart-container">
    <h2>Twój koszyk jest pusty</h2>
    <p>Dodaj produkty do koszyka, aby złożyć zamówienie.</p>
    <button @click="goToShop" class="shop-btn">Przejdź do sklepu</button>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class EmptyCartMessage extends Vue {
  goToShop(): void {
    this.$router.push("/");
  }
}
</script>

<style scoped>
.empty-cart-container {
  text-align: center;
  padding: 40px 20px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  margin: 100px auto;
}

.shop-btn {
  background-color: #c70a0a;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  margin-top: 20px;
}

.shop-btn:hover {
  background-color: #a30606;
}

h2 {
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 10px;
}

p {
  font-size: 1.2rem;
  color: #666;
}
</style>
