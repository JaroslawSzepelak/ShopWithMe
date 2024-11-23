<template>
  <section class="welcome-section">
    <div class="content">
      <h1>Witamy w sklepie ShopWithMe!</h1>
      <p>
        Znajdź najlepsze produkty dostępne w naszym sklepie w wyjątkowych
        cenach.
      </p>
      <router-link to="/products" class="btn btn-primary btn-lg"
        >Zobacz produkty</router-link
      >
    </div>
    <div class="image-container">
      <img src="https://placehold.co/600x400" alt="Shop Image" />
    </div>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class WelcomeSection extends Vue {}
</script>

<style scoped lang="scss">
.welcome-section {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 40px 20px;
  background-color: #f8f9fa;
  border-radius: 8px;
  margin-top: 20px;

  .content {
    max-width: 60%;
    h1 {
      font-size: 2.5rem;
      color: #333;
      margin-bottom: 10px;
    }

    p {
      font-size: 1.2rem;
      color: #555;
      margin-bottom: 20px;
    }

    .btn-primary {
      font-size: 1.2rem;
      padding: 15px 30px;
      border-radius: 5px;
      background-color: $primary-color;
      color: $light-color;
      &:hover {
        background-color: darken($primary-color, 10%);
      }
    }
  }

  .image-container {
    max-width: 40%;
    img {
      width: 100%;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }
  }
}
</style>
