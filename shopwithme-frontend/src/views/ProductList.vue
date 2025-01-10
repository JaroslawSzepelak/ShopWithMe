<template>
  <div class="product-list">
    <!-- Filtry -->
    <div class="filters">
      <div class="filters-container">
        <input
          type="text"
          v-model="filters.search"
          placeholder="Wprowadź frazę"
          class="form-control filter-input"
        />
        <select v-model="filters.categoryId" class="form-control filter-input">
          <option value="">Wszystkie kategorie</option>
          <option
            v-for="category in categories"
            :key="category.id"
            :value="category.id"
          >
            {{ category.name }}
          </option>
        </select>
        <button @click="applyFilters" class="btn btn-dark filter-button">
          Filtruj
        </button>
      </div>
    </div>
    <h1>Produkty</h1>

    <!-- Ekranik ładowania -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie produktów...</p>
    </div>

    <!-- Komunikat o błędzie -->
    <div v-else-if="error" class="error-message">
      <p>{{ error }}</p>
    </div>

    <!-- Lista produktów -->
    <div v-else>
      <div v-if="products.length > 0" class="product-list-container">
        <ProductCard
          v-for="product in products"
          :key="product.id"
          :product="product"
        />
      </div>
      <div v-else class="no-products">
        <p>Brak produktów do wyświetlenia.</p>
        <button @click="goToShop" class="shop-btn">
          Przejdź do strony głównej skklepu
        </button>
      </div>

      <!-- Paginacja -->
      <div class="pagination-container">
        <div class="page-size-selector">
          <label for="pageSize">Ilość produktów na stronę:</label>
          <select
            id="pageSize"
            class="page-size-select"
            :value="pageSize"
            @change="changePageSize($event.target.value)"
          >
            <option value="10">10 na stronę</option>
            <option value="20">20 na stronę</option>
            <option value="30">30 na stronę</option>
          </select>
        </div>
        <div class="pagination-controls">
          <button
            :disabled="pageIndex === 1"
            @click="changePage(pageIndex - 1)"
            class="btn btn-secondary"
          >
            Poprzednia
          </button>
          <span v-for="page in visiblePages" :key="page">
            <button
              v-if="page !== -1"
              class="btn"
              :class="{
                'btn-primary': page === pageIndex,
                'btn-secondary': page !== pageIndex,
              }"
              @click="changePage(page)"
            >
              {{ page }}
            </button>
            <span v-else class="dots">...</span>
          </span>
          <button
            :disabled="pageIndex === totalPages"
            @click="changePage(pageIndex + 1)"
            class="btn btn-secondary"
          >
            Następna
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ProductCard from "@/components/ProductCard.vue";

@Component({
  components: {
    ProductCard,
  },
})
export default class ProductList extends Vue {
  isLoading = false;
  error: string | null = null;
  filters = {
    search: sessionStorage.getItem("search") || "",
    categoryId: sessionStorage.getItem("categoryId") || "",
  };

  get products() {
    return this.$store.state.products.paginatedProducts || [];
  }

  get pageIndex() {
    return this.$store.state.products.currentPage || 1;
  }

  get pageSize() {
    return this.$store.state.products.pageSize || 10;
  }

  get totalPages() {
    return this.$store.state.products.totalPages || 1;
  }

  get categories() {
    return this.$store.getters["categories/allCategories"];
  }

  async created() {
    try {
      const savedPageSize = Number(sessionStorage.getItem("pageSize")) || 10;
      const savedPageIndex = Number(sessionStorage.getItem("currentPage")) || 1;

      this.$store.commit("products/SET_PAGE_SIZE", savedPageSize);
      this.$store.commit("products/SET_CURRENT_PAGE", savedPageIndex);

      await this.$store.dispatch("categories/fetchAllCategories");
      await this.fetchProducts(savedPageIndex, savedPageSize);
    } catch (error) {
      this.error = "Nie udało się załadować listy produktów.";
      console.error("Błąd podczas inicjalizacji widoku ProductList:", error);
    }
  }

  async fetchProducts(pageIndex: number, pageSize: number) {
    this.isLoading = true;
    this.error = null;

    try {
      await this.$store.dispatch("products/fetchProducts", {
        pageIndex,
        pageSize,
      });
    } catch (error) {
      this.error = "Wystąpił błąd podczas pobierania listy produktów.";
      console.error("Błąd podczas pobierania produktów:", error);
    } finally {
      this.isLoading = false;
    }
  }

  async changePage(newPageIndex: number) {
    if (newPageIndex > 0 && newPageIndex <= this.totalPages) {
      try {
        this.$store.commit("products/SET_CURRENT_PAGE", newPageIndex);
        sessionStorage.setItem("currentPage", String(newPageIndex));
        await this.fetchProducts(newPageIndex, this.pageSize);
        this.scrollToTop();
      } catch (error) {
        this.error = "Nie udało się zmienić strony.";
        console.error("Błąd podczas zmiany strony:", error);
      }
    }
  }

  async changePageSize(newPageSize: number) {
    try {
      this.$store.commit("products/SET_PAGE_SIZE", Number(newPageSize));
      sessionStorage.setItem("pageSize", String(newPageSize));
      this.$store.commit("products/SET_CURRENT_PAGE", 1);
      sessionStorage.setItem("currentPage", "1");
      await this.fetchProducts(1, Number(newPageSize));
      this.scrollToTop();
    } catch (error) {
      this.error = "Nie udało się zmienić ilości produktów na stronę.";
      console.error("Błąd podczas zmiany rozmiaru strony:", error);
    }
  }

  applyFilters() {
    this.$store.dispatch("products/setFilters", {
      categoryId: this.filters.categoryId || null,
      search: this.filters.search || null,
    });
    location.reload();
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  get visiblePages(): number[] {
    const maxVisible = 5;
    const pages: number[] = [];

    pages.push(1);

    if (this.totalPages <= maxVisible) {
      for (let i = 2; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      const minPage = Math.max(2, this.pageIndex - 2);
      const maxPage = Math.min(this.totalPages - 1, this.pageIndex + 2);

      if (minPage > 2) pages.push(-1);

      for (let i = minPage; i <= maxPage; i++) {
        pages.push(i);
      }

      if (maxPage < this.totalPages - 1) pages.push(-1);

      pages.push(this.totalPages);
    }

    return pages;
  }

  goToShop(): void {
    this.$router.push("/");
  }
}
</script>

<style scoped lang="scss">
.product-list {
  padding: 20px;

  .filters {
    display: flex;
    justify-content: center;
    margin-bottom: 20px;

    .filters-container {
      display: flex;
      flex-wrap: wrap;
      gap: 10px;
      max-width: 800px;
      justify-content: center;
      align-items: center;
      width: 100%;
    }

    .filter-input {
      padding: 8px;
      border: 1px solid #ccc;
      border-radius: 5px;
      font-size: 1rem;
      flex: 1;
      min-width: 150px;
      max-width: 300px;
    }

    .filter-button {
      background-color: #333;
      color: #fff;
      padding: 10px 20px;
      border: none;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
      transition: background-color 0.3s;

      &:hover {
        background-color: #555;
      }
    }
  }

  @media (max-width: 768px) {
    .filters-container {
      flex-direction: column;
      gap: 15px;
    }

    .filter-input,
    .filter-button {
      width: 100%;
    }
  }

  h1 {
    font-size: 2rem;
    margin-bottom: 20px;
    text-align: center;
    color: #333;
  }

  .loading-overlay {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 200px;

    .spinner {
      box-sizing: border-box;
      width: 50px;
      height: 50px;
      border: 5px solid #ccc;
      border-top-color: #333;
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }

    p {
      margin-top: 15px;
      font-size: 1.2rem;
      color: #555;
    }
  }

  @keyframes spin {
    from {
      transform: rotate(0deg);
    }
    to {
      transform: rotate(360deg);
    }
  }

  .error-message,
  .no-products {
    text-align: center;
    color: #555;
    font-size: 1.2rem;
    margin-top: 20px;
  }

  .product-list-container {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }

  .pagination-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 20px;
    padding: 10px;

    .page-size-selector {
      display: flex;
      align-items: center;
      gap: 10px;

      label {
        font-size: 1rem;
        font-weight: bold;
      }

      .page-size-select {
        padding: 5px;
        font-size: 1rem;
        border: 1px solid #ccc;
        border-radius: 5px;
      }
    }

    .pagination-controls {
      display: flex;
      gap: 10px;

      .btn {
        padding: 10px 15px;
        background-color: #555;
        color: #fff;
        border-radius: 5px;
        cursor: pointer;
        font-weight: bold;

        &:hover {
          background-color: #333;
        }

        &.btn-primary {
          background-color: #111;
          border-color: #000;
        }

        &:disabled {
          background-color: #888;
          cursor: not-allowed;
        }
      }

      .dots {
        padding: 10px 15px;
        color: #777;
        font-weight: bold;
        cursor: default;
      }
    }
  }
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

/* Responsywne style */
@media (max-width: 1200px) {
  .product-list-container {
    flex-direction: row;
    flex-wrap: wrap;
    gap: 15px;
  }

  .pagination-container {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }

  .page-size-selector {
    width: 100%;
    justify-content: center;
    margin-bottom: 30px;
  }
}

@media (max-width: 800px) {
  h1 {
    font-size: 1.8rem;
  }

  .product-list-container {
    gap: 10px;
  }

  .pagination-container {
    .btn {
      font-size: 0.9rem;
      padding: 8px 12px;
    }
  }
}

@media (max-width: 500px) {
  .product-list {
    padding: 10px;
  }

  h1 {
    font-size: 1.5rem;
  }

  .page-size-selector label {
    font-size: 0.9rem;
  }

  .page-size-select {
    font-size: 0.9rem;
    padding: 4px;
  }

  .pagination-container {
    .btn {
      font-size: 0.8rem;
      padding: 6px 10px;
    }
  }

  .shop-btn {
    font-size: 0.9rem;
    padding: 8px 16px;
  }
}

@media (max-width: 450px) {
  .pagination-container {
    .pagination-controls {
      flex-wrap: wrap;
      align-items: center;
      justify-content: center;
      gap: 5px;

      .btn {
        font-size: 0.7rem;
        padding: 5px 8px;
        text-align: center;
      }
    }
  }
}
</style>
