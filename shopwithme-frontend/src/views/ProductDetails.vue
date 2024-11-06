<template>
  <div class="container mt-5">
    <h1>Szczegóły produktu</h1>
    <div v-if="product" class="card">
      <div class="card-body">
        <h5 class="card-title">{{ product.name }}</h5>
        <p class="card-text">{{ product.description }}</p>
        <p><strong>Cena:</strong> {{ product.price }} PLN</p>
        <button @click="addToCart" class="btn btn-success">
          Dodaj do koszyka
        </button>
      </div>
    </div>
    <router-link to="/products" class="btn btn-secondary mt-3"
      >Powrót do listy produktów</router-link
    >
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import apiClient from "@/plugins/axios";

@Component
export default class ProductDetails extends Vue {
  product: {
    id: number;
    name: string;
    description: string;
    price: number;
  } | null = null;

  async created() {
    const productId = this.$route.params.id;
    try {
      const response = await apiClient.get(`/${productId}`);
      this.product = response.data;
    } catch (error) {
      console.error("Błąd podczas pobierania szczegółów produktu:", error);
    }
  }

  addToCart() {
    console.log(`Dodano ${this.product?.name} do koszyka`);
  }
}
</script>

<style scoped lang="scss">
.container {
  margin-top: 20px;
}
</style>
