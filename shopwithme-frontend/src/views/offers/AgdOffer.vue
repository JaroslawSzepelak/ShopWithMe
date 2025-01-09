<template>
  <div class="agd-promo">
    <div class="banner">
      <img
        src="https://placehold.co/600x400"
        alt="Promocja AGD"
        class="banner-image"
      />
      <div class="banner-text">
        <h1>INNOWACYJNE AGD DO TWOJEGO DOMU</h1>
        <p>
          Roboty kuchenne, ekspresy do kawy i inteligentne lodówki, które
          ułatwią Twoje życie.
        </p>
        <p>
          Postaw na wygodę i oszczędność czasu dzięki nowoczesnym rozwiązaniom
          AGD.
        </p>
        <p class="tagline">twoja kuchnia, <span>twoje zasady!</span></p>
      </div>
    </div>

    <div class="product-list">
      <div
        v-for="(product, index) in products"
        :key="`${product.id}-${index}`"
        class="product-card"
      >
        <img
          v-if="product.mainImage?.url"
          :src="product.mainImage.url"
          alt="Zdjęcie produktu"
          class="product-image"
        />
        <img
          v-else
          src="https://placehold.co/300x300"
          alt="Zdjęcie zastępcze"
          class="product-image"
        />
        <div class="product-info">
          <h4 @click="goToProduct(product.id)" class="product-name">
            {{ product.name }}
          </h4>
          <div class="product-price">
            <span v-if="product.salePrice">
              <span class="original-price">{{ product.price }} zł</span>
              <span class="sale-price">{{ product.salePrice }} zł</span>
              <span class="discount">(-{{ calculateDiscount(product) }}%)</span>
            </span>
            <span v-else>{{ product.price }} zł</span>
          </div>
          <button class="add-to-cart-btn" @click="addToCart(product.id)">
            Dodaj do koszyka
          </button>
        </div>
      </div>
    </div>
    <div class="button-group">
      <button @click="goToHomePage" class="home-btn">
        Wróć do strony głównej
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { storageAPI } from "@/plugins/shopAxios";

@Component
export default class WorkEnjoyOffer extends Vue {
  products: Array<any> = [];
  errorMessage: string | null = null;

  async mounted() {
    try {
      const productIds = [74, 82, 89, 96, 97, 110, 112];

      for (const id of productIds) {
        const product = await this.loadProduct(id);
        if (product) {
          this.products.push(product);
        }
      }
    } catch (error) {
      console.error("Błąd podczas ładowania produktów:", error);
      this.errorMessage =
        "Nie udało się załadować produktów. Spróbuj ponownie później.";
    }
  }

  async loadProduct(id: number) {
    try {
      await this.$store.dispatch("products/fetchProduct", id);

      const product = JSON.parse(
        JSON.stringify(this.$store.getters["products/selectedProduct"])
      );

      if (!product) {
        console.warn(`Produkt o ID ${id} nie został znaleziony.`);
        return null;
      }

      if (product?.mainImage?.name && !product.mainImage.url) {
        try {
          const response = await storageAPI.getFile(product.mainImage.name);
          product.mainImage.url = window.URL.createObjectURL(
            new Blob([response.data])
          );
        } catch (error) {
          console.error(
            `Błąd podczas ładowania obrazu dla produktu ${id}:`,
            error
          );
        }
      }

      return product;
    } catch (error) {
      console.error(`Błąd podczas pobierania produktu o ID ${id}:`, error);
      return null;
    }
  }

  calculateDiscount(product: any): number {
    if (product.price && product.salePrice) {
      const discount =
        ((product.price - product.salePrice) / product.price) * 100;
      return Math.round(discount);
    }
    return 0;
  }

  addToCart(productId: number) {
    this.$store.dispatch("cart/addItem", { productId, quantity: 1 });
    this.$router.push("/cart");
  }

  goToProduct(productId: number) {
    this.$router.push({
      name: "ProductDetails",
      params: { id: productId.toString() },
    });
  }

  goToHomePage() {
    this.$router.push("/");
  }
}
</script>

<style scoped lang="scss">
.agd-promo {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
  background-color: #fefae0;

  .banner {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 20px;
    margin-bottom: 30px;

    .banner-image {
      max-width: 600px;
      width: 100%;
      border-radius: 8px;
    }

    .banner-text {
      flex: 1;
      text-align: center;

      h1 {
        font-size: 2.5rem;
        font-weight: bold;
        margin-bottom: 10px;
        color: #000;
      }

      p {
        font-size: 1.2rem;
        margin-bottom: 10px;
        color: #333;
      }

      .tagline {
        font-size: 1.5rem;
        font-weight: bold;
        color: #c70a0a;

        span {
          color: #ff4500;
        }
      }
    }
  }

  .product-list {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 20px;
    width: 100%;
    max-width: 1200px;

    .product-card {
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      align-items: center;
      background: #fff;
      border: 1px solid #ddd;
      border-radius: 10px;
      padding: 20px;
      box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);

      &:hover {
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
      }

      .product-image {
        width: 90%;
        max-width: 250px;
        max-height: 250px;
        object-fit: cover;
        border-radius: 8px;
        margin-bottom: 15px;
      }

      .product-info {
        text-align: center;

        .product-name {
          cursor: pointer;
          font-size: 1.1rem;
          color: #333;
          font-weight: bold;
          text-decoration: none;
          transition: color 0.3s;

          &:hover {
            color: #c70a0a;
          }
        }

        .product-price {
          font-size: 1.1rem;
          color: #c70a0a;

          .original-price {
            text-decoration: line-through;
            margin-right: 10px;
            color: #888;
          }

          .sale-price {
            font-size: 1.3rem;
            font-weight: bold;
            color: #c70a0a;
          }

          .discount {
            font-size: 0.9rem;
            color: #28a745;
            margin-left: 10px;
          }
        }

        .add-to-cart-btn {
          background-color: #c70a0a;
          color: white;
          border: none;
          padding: 10px 20px;
          font-size: 1rem;
          border-radius: 5px;
          margin-top: 40px;
          cursor: pointer;

          &:hover {
            background-color: #a50e0e;
          }
        }
      }
    }
  }

  .button-group {
    margin-top: 30px;

    .home-btn {
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

  @media (max-width: 920px) {
    .banner {
      .image-section {
        display: none;
      }
    }
  }

  @media (max-width: 560px) {
    .product-list {
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;

      .product-card {
        max-width: 300px;
      }
    }
  }
}
</style>
