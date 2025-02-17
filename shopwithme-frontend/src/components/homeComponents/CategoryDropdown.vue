<template>
  <div class="category-dropdown" ref="dropdown">
    <button class="dropdown-button" @click="toggleDropdown">
      <i class="fas fa-bars"></i> Kategorie
    </button>
    <div v-show="isOpen" class="dropdown-menu">
      <div class="dropdown-content">
        <!-- Category List -->
        <ul class="categories">
          <li
            v-for="category in categories"
            :key="category.id"
            @click="fetchProducts(category)"
          >
            {{ category.name }}
            <i class="fas fa-chevron-right"></i>
          </li>
        </ul>

        <!-- Product List -->
        <div class="products" v-if="products.length">
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
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { storageAPI } from "@/plugins/shopAxios";

@Component
export default class CategoryDropdown extends Vue {
  isOpen = false;
  placeholderImage = "https://placehold.co/100x100";
  selectedCategory: any = null;

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
    document.addEventListener("click", this.handleOutsideClick);
  }

  destroyed() {
    document.removeEventListener("click", this.handleOutsideClick);
  }

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  async fetchProducts(category: { id: number; name: string; products: any[] }) {
    this.selectedCategory = category;

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

  goToProductDetails(productId: number) {
    if (this.$route.params.id === productId.toString()) {
      this.$emit("refresh-product", productId);
    } else {
      this.$router.push({
        name: "ProductDetails",
        params: { id: productId.toString() },
      });
    }
  }

  handleOutsideClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const dropdown = this.$el as HTMLElement;

    if (dropdown && !dropdown.contains(target)) {
      this.isOpen = false;
    }
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

  .dropdown-menu {
    box-sizing: border-box;
    position: absolute;
    top: calc(100% + 0.5rem);
    left: 0;
    background-color: #ffffff;
    border: 1px solid #ddd;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    z-index: 1000;
    display: flex;
    width: 850px;

    .dropdown-content {
      display: flex;

      .categories {
        box-sizing: border-box;
        list-style: none;
        margin: 0;
        padding: 0;
        width: 300px;
        border-right: 1px solid #ddd;

        li {
          padding: 0.75rem 1rem;
          color: #333;
          cursor: pointer;
          font-weight: 500;
          display: flex;
          justify-content: space-between;
          transition: background-color 0.3s;

          &:hover {
            background-color: #f0f0f0;
            color: #000;
          }
        }
      }

      .products {
        flex: 1;
        padding: 1rem;
        overflow-y: auto;
        max-height: 600px;
        min-width: 550px;

        h3 {
          margin-bottom: 1rem;
        }

        .product-scroll {
          display: flex;
          flex-direction: column;
          gap: 1rem;

          .product-item {
            display: flex;
            align-items: center;
            gap: 1rem;
            cursor: pointer;
            padding: 0.5rem;
            border: 1px solid #ddd;
            border-radius: 5px;
            transition: background-color 0.3s;

            &:hover {
              background-color: #f9f9f9;
            }

            .product-image {
              width: 60px;
              height: 60px;
              object-fit: cover;
              border-radius: 5px;
            }

            .product-info {
              h4 {
                margin: 0;
                font-size: 1rem;
                color: #333;
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
                margin: 0.25rem 0;
                font-size: 0.9rem;
                color: #555;
              }
            }
          }
        }
      }
    }
  }
}
</style>
