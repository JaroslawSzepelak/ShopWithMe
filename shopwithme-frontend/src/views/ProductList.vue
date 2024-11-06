<template>
  <div class="container mt-5">
    <h1>Produkty</h1>
    <div class="row">
      <div class="col-md-4" v-for="product in products" :key="product.id">
        <div class="card mb-4">
          <div class="card-body">
            <h5 class="card-title">{{ product.name }}</h5>
            <p class="card-text">{{ product.description }}</p>
            <p><strong>Cena:</strong> {{ product.price }} PLN</p>
            <router-link
              :to="{ name: 'ProductDetails', params: { id: product.id } }"
              class="btn btn-primary"
            >
              Zobacz szczegóły
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import apiClient from "@/plugins/axios";

@Component
export default class ProductList extends Vue {
  products: Array<{
    id: number;
    name: string;
    description: string;
    price: number;
  }> = [];

  async created() {
    try {
      const response = await apiClient.get("/");
      this.products = response.data;
    } catch (error) {
      console.error("Błąd podczas pobierania produktów:", error);
    }
  }
}
</script>

<style scoped lang="scss">
.container {
  margin-top: 20px;
}

.card {
  transition: transform 0.3s;
  &:hover {
    transform: scale(1.05);
  }
}
</style>
