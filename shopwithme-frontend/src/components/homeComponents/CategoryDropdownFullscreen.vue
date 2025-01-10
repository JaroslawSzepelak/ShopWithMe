<template>
  <div class="category-dropdown">
    <button class="dropdown-button" @click="toggleFullscreen">
      <i class="fas fa-bars"></i> Kategorie
    </button>
    <div
      v-show="isFullscreen"
      class="fullscreen-dropdown"
      :class="{ 'slide-in': isFullscreen }"
    >
      <div class="header">
        <button class="back-button" v-if="showProducts" @click="goBack">
          <i class="fas fa-chevron-left"></i> Powrót
        </button>
        <button class="close-button" @click="toggleFullscreen">
          <i class="fas fa-times"></i> Zamknij
        </button>
      </div>
      <div v-if="!showProducts" class="categories">
        <ul>
          <li
            v-for="category in categories"
            :key="category.id"
            @click="fetchProducts(category)"
          >
            {{ category.name }}
          </li>
        </ul>
      </div>
      <div v-if="showProducts" class="products">
        <h3>Produkty</h3>
        <div class="product-scroll">
          <div
            v-for="product in products"
            :key="product.id"
            class="product-item"
            @click="goToProductDetails(product.id)"
          >
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

            <div class="product-info">
              <h4>{{ product.name }}</h4>
              <p>{{ product.lead }}</p>
              <p class="product-price">
                <span v-if="product.salePrice">
                  <span class="original-price">{{ product.price }} zł</span>
                  <span class="sale-price">{{ product.salePrice }} zł</span>
                  <span class="discount"
                    >(-{{ calculateDiscount(product) }}%)</span
                  >
                </span>
                <span v-else>
                  <strong>Cena:</strong> {{ product.price }} zł
                </span>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { storageAPI } from "@/plugins/shopAxios";
import { Component, Vue } from "vue-property-decorator";

@Component
export default class CategoryDropdownFullScreen extends Vue {
  isFullscreen = false;
  showProducts = false;
  selectedCategory: any = null;
  placeholderImage = "https://placehold.co/100x100";

  get categories() {
    return this.$store.getters["categories/allCategories"];
  }

  get products() {
    return this.selectedCategory?.products || [];
  }

  async mounted() {
    try {
      await this.$store.dispatch("categories/fetchAllCategories", true);
    } catch (error) {
      console.error("Błąd podczas pobierania kategorii z produktami:", error);
    }
  }

  toggleFullscreen() {
    this.isFullscreen = !this.isFullscreen;
    this.showProducts = false;
  }

  async fetchProducts(category: { id: number; name: string; products: any[] }) {
    this.selectedCategory = category;
    this.showProducts = true;

    const products = this.products;

    for (const product of products) {
      if (product.mainImage?.name) {
        try {
          const response = await storageAPI.getFile(product.mainImage.name);

          const imageUrl = window.URL.createObjectURL(
            new Blob([response.data])
          );
          this.$set(product.mainImage, "url", imageUrl);
        } catch (error) {
          console.error(
            `Błąd podczas pobierania zdjęcia dla produktu ${product.id}:`,
            error
          );
        }
      }
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

  goBack() {
    this.showProducts = false;
  }

  goToProductDetails(productId: number) {
    if (this.$route.params.id === productId.toString()) {
      this.$emit("refresh-product", productId);
    } else {
      this.$router
        .push({
          name: "ProductDetails",
          params: { id: productId.toString() },
        })
        .catch((err) => {
          if (err.name !== "NavigationDuplicated") {
            console.error(err);
          }
        });
    }

    this.isFullscreen = false;
  }
}
</script>

<style scoped lang="scss">
.category-dropdown {
  position: relative;
  display: inline-block;

  .dropdown-button {
    background-color: #333;
    color: #fff;
    border: none;
    font-size: 1rem;
    padding: 0.5rem 1rem;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s;

    &:hover {
      background-color: #555;
    }
  }

  .fullscreen-dropdown {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: #fff;
    z-index: 2000;
    display: flex;
    flex-direction: column;
    transform: translateX(-100%);
    transition: transform 0.3s ease-in-out;

    &.slide-in {
      transform: translateX(0);
    }

    .header {
      display: flex;
      justify-content: space-between;
      padding: 1rem;
      border-bottom: 1px solid #ddd;

      .back-button,
      .close-button {
        background: none;
        border: none;
        font-size: 1rem;
        cursor: pointer;
      }
    }

    .categories {
      flex: 1;
      padding: 1rem;
      overflow-y: auto;

      ul {
        list-style: none;
        padding: 0;

        li {
          padding: 1rem;
          border-bottom: 1px solid #ddd;
          cursor: pointer;

          &:hover {
            background-color: #f5f5f5;
          }
        }
      }
    }

    .products {
      flex: 1;
      padding: 1rem;
      overflow-y: auto;

      h3 {
        margin-bottom: 1rem;
      }

      .product-scroll {
        display: flex;
        flex-direction: column;
        gap: 1rem;

        .product-item {
          display: flex;
          gap: 1rem;
          align-items: center;
          cursor: pointer;
          border: 1px solid #ddd;
          padding: 1rem;
          border-radius: 5px;

          &:hover {
            background-color: #f9f9f9;
          }

          .product-image {
            width: 80px;
            height: 80px;
            object-fit: cover;
            border-radius: 5px;
          }

          .product-info {
            h4 {
              margin: 0;
            }

            .product-price {
              font-size: 0.9rem;

              .original-price {
                text-decoration: line-through;
                color: #777;
                font-size: 0.8rem;
                margin-right: 0.5rem;
              }

              .sale-price {
                color: #c70a0a;
                font-size: 1rem;
                font-weight: bold;
              }

              .discount {
                color: #555;
                font-size: 0.8rem;
                margin-left: 0.5rem;
              }
            }

            p {
              margin: 0.5rem 0;
              font-size: 0.9rem;
            }
          }
        }
      }
    }
  }

  @media (max-width: 768px) {
    .fullscreen-dropdown {
      .categories li {
        font-size: 1rem;
      }

      .products .product-item {
        flex-direction: column;
        align-items: flex-start;

        .product-image {
          width: 100px;
          height: 100px;
        }
      }
    }
  }
}
</style>
