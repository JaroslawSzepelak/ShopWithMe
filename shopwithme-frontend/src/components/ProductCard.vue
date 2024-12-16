<template>
  <div class="product-card">
    <img
      :src="product.image || placeholderImage"
      alt="Product Image"
      class="product-image"
    />
    <div class="product-details">
      <h3 class="product-name">{{ product.name }}</h3>
      <p class="product-lead">{{ product.lead }}</p>
      <p class="product-price">
        <strong>Cena:</strong> {{ product.price }} PLN
      </p>
      <a @click="goToProduct(product.id)" class="details-btn">
        Zobacz szczegóły
      </a>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component
export default class ProductCard extends Vue {
  @Prop({ required: true }) product!: {
    id: number;
    name: string;
    lead: string;
    price: number;
    image?: string;
  };

  placeholderImage = "https://placehold.co/300x300";

  goToProduct(productId: number) {
    this.$router.push({
      name: "ProductDetails",
      params: { id: productId.toString() },
    });
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
}
</script>

<style scoped lang="scss">
.product-card {
  box-sizing: border-box;
  display: flex;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.3s, box-shadow 0.3s;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;

  &:hover {
    transform: translateY(-5px);
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
  }

  .product-image {
    width: 300px;
    height: 300px;
    object-fit: cover;
    flex-shrink: 0;
  }

  .product-details {
    padding: 20px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 1;

    .product-name {
      font-size: 1.8rem;
      margin-bottom: 10px;
    }

    .product-lead {
      font-size: 1rem;
      color: #666;
      margin-bottom: 10px;
    }

    .product-price {
      font-size: 1.5rem;
      font-weight: bold;
      margin-bottom: 20px;
      color: #c70a0a;
    }

    .details-btn {
      box-sizing: border-box;
      cursor: pointer;
      display: inline-block;
      background-color: #c70a0a;
      color: #fff;
      text-decoration: none;
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      width: 50%;
      margin: 0 auto;
      text-align: center;
      transition: background-color 0.3s;

      &:hover {
        background-color: #a50e0e;
      }
    }
  }
}
</style>
