<template>
  <div class="laptops-accessories-promo">
    <div class="banner">
      <div class="text-section">
        <h1>LAPTOPY I AKCESORIA<br />TERAZ W PROMOCJI!</h1>
        <h2>Pracuj i baw się szybciej!</h2>
        <p>
          Laptopy, smartfony i akcesoria w cenach, które zaskakują. Idealne do
          pracy, nauki i rozrywki. Znajdź sprzęt dopasowany do Twoich potrzeb –
          od biura po gaming!
        </p>
      </div>
      <div class="image-section">
        <img src="https://placehold.co/800x400" alt="Laptopy i akcesoria" />
      </div>
    </div>
    <div class="product-list">
      <div v-for="product in products" :key="product.id" class="product-card">
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
      const productIds = [57, 60, 66, 78, 79, 87, 94];

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
.laptops-accessories-promo {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
  background-color: #f5fbe5;

  .banner {
    box-sizing: border-box;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    background-color: #eaf5d5;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 30px;

    .text-section {
      flex: 1;
      padding: 20px;

      h1 {
        font-size: 2.5rem;
        color: #ff4747;
        margin-bottom: 10px;
        text-transform: uppercase;
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
    }

    .image-section {
      flex: 1;
      display: flex;
      justify-content: center;

      img {
        max-width: 100%;
        border-radius: 8px;
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
          margin-bottom: 10px;

          &:hover {
            color: #a50e0e;
          }
        }

        .product-price {
          font-size: 1.1rem;
          margin-bottom: 10px;

          .original-price {
            text-decoration: line-through;
            margin-right: 5px;
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
            margin-left: 5px;
          }
        }

        .add-to-cart-btn {
          background-color: #c70a0a;
          color: white;
          border: none;
          padding: 10px 20px;
          margin-top: 40px;
          font-size: 1rem;
          border-radius: 5px;
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
