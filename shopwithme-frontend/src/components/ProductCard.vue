<template>
  <div class="product-card">
    <img
      v-if="product.mainImage?.url"
      :src="product.mainImage.url"
      alt="Zdjęcie produktu"
      class="product-image"
    />
    <img
      v-else
      :src="placeholderImage"
      alt="Obraz zastępczy"
      class="product-image"
    />
    <div class="product-details">
      <h3 class="product-name">{{ product.name }}</h3>
      <p class="product-lead">{{ product.lead }}</p>
      <div class="product-pricing">
        <p v-if="product.salePrice !== null" class="product-price sale-price">
          <strong>Cena promocyjna:</strong> {{ product.salePrice }} zł
          <span class="discount">
            ({{ calculateDiscount(product.price, product.salePrice) }}% taniej)
          </span>
        </p>
        <p
          v-if="product.salePrice !== null"
          class="product-price regular-price"
        >
          <s>{{ product.price }} zł</s>
        </p>
        <p v-else class="product-price">
          <strong>Cena:</strong> {{ product.price }} zł
        </p>
      </div>
      <a @click="goToProduct(product.id)" class="details-btn">
        Zobacz szczegóły
      </a>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { storageAPI } from "@/plugins/shopAxios";

@Component
export default class ProductCard extends Vue {
  @Prop({ required: true }) product!: {
    id: number;
    name: string;
    lead: string;
    price: number;
    salePrice?: number | null;
    mainImage?: {
      name?: string;
      url?: string;
    };
  };

  placeholderImage = "https://placehold.co/300x300";

  async mounted() {
    if (this.product.mainImage?.name && !this.product.mainImage.url) {
      try {
        const response = await storageAPI.getFile(this.product.mainImage.name);
        const imageUrl = window.URL.createObjectURL(new Blob([response.data]));
        this.$set(this.product.mainImage, "url", imageUrl);
      } catch (error) {
        console.error(
          `Błąd podczas pobierania zdjęcia dla produktu ${this.product.id}:`,
          error
        );
      }
    }
  }

  calculateDiscount(originalPrice: number, salePrice: number): number {
    return Math.round(((originalPrice - salePrice) / originalPrice) * 100);
  }

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
    margin-right: 40px;
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

    .product-pricing {
      .product-price {
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 10px;
        color: #c70a0a;

        &.sale-price {
          color: #c70a0a;
        }

        &.regular-price {
          font-size: 1.2rem;
          color: #666;
        }

        .discount {
          font-size: 1rem;
          color: #666;
          margin-left: 10px;
        }
      }
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

/* Responsive styling */
@media (max-width: 900px) {
  .product-card {
    flex-direction: column;
    align-items: center;
    width: 70%;

    .product-image {
      margin-top: 40px;
      margin-right: 0;
      width: 80%;
      height: auto;
    }

    .product-details {
      padding: 15px;

      .product-name {
        font-size: 1.6rem;
      }

      .product-price {
        margin-top: 30px;
        margin-bottom: 30px;
        font-size: 1.4rem;
      }
    }
  }
}

@media (max-width: 800px) {
  .product-card {
    padding: 10px;

    .product-details {
      .product-name {
        font-size: 1.4rem;
      }

      .product-lead {
        font-size: 0.9rem;
      }

      .product-price {
        font-size: 1.3rem;
      }

      .details-btn {
        box-sizing: border-box;
        font-size: 0.9rem;
        padding: 8px 16px;
        width: 70%;
      }
    }
  }
}

@media (max-width: 500px) {
  .product-card {
    width: 80%;
    box-sizing: border-box;
    flex-direction: column;
    padding: 5px;

    .product-image {
      margin-top: 20px;
      height: auto;
    }

    .product-details {
      padding: 10px;

      .product-name {
        font-size: 1.2rem;
      }

      .product-lead {
        font-size: 0.8rem;
      }

      .product-price {
        font-size: 1.2rem;
      }

      .details-btn {
        box-sizing: border-box;
        font-size: 0.8rem;
        padding: 6px 12px;
        width: 80%;
      }
    }
  }
}
</style>
