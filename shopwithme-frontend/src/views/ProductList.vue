<template>
  <div class="product-list">
    <h1>Produkty</h1>
    <div class="product-list-container">
      <ProductCard
        v-for="product in paginatedProducts"
        :key="product.id"
        :product="product"
      />
    </div>
    <div class="pagination-container">
      <div class="page-size-selector">
        <label for="pageSize">Ilość produktów na stronę:</label>
        <select
          id="pageSize"
          class="page-size-select"
          v-model="pageSize"
          @change="updatePageSize"
        >
          <option value="10">10 na stronę</option>
          <option value="20">20 na stronę</option>
          <option value="30">30 na stronę</option>
        </select>
      </div>
      <Pagination
        :currentPage="currentPage"
        :pageCount="pageCount"
        @changePage="changePage"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ProductCard from "@/components/ProductCard.vue";
import Pagination from "@/components/Pagination.vue";
import { mapGetters } from "vuex";

@Component({
  components: {
    ProductCard,
    Pagination,
  },
  computed: {
    ...mapGetters("products", ["paginatedProducts", "pageCount"]),
  },
})
export default class ProductList extends Vue {
  currentPage: number = this.$store.getters["products/currentPage"];
  pageSize: number = this.$store.getters["products/pageSize"];

  get paginatedProducts() {
    return this.$store.getters["products/paginatedProducts"];
  }

  get pageCount() {
    return this.$store.getters["products/pageCount"];
  }

  async created() {
    try {
      const savedPageSize = Number(sessionStorage.getItem("pageSize")) || 10;
      const savedCurrentPage =
        Number(sessionStorage.getItem("currentPage")) || 1;

      this.$store.dispatch("products/setPageSize", savedPageSize);
      this.$store.dispatch("products/setCurrentPage", savedCurrentPage);

      await this.$store.dispatch("products/fetchProducts");
    } catch (error) {
      console.error(
        "Błąd podczas inicjalizacji komponentu ProductList:",
        error
      );
    }
  }

  updatePageSize() {
    this.currentPage = 1;
    this.$store.dispatch("products/setPageSize", this.pageSize);
    this.$store.dispatch("products/setCurrentPage", this.currentPage);
  }

  changePage(newPage: number) {
    this.currentPage = newPage;
    this.$store.dispatch("products/setCurrentPage", newPage);

    window.scrollTo({ top: 0, behavior: "smooth" });
  }
}
</script>

<style scoped lang="scss">
.product-list {
  padding: 20px;

  h1 {
    font-size: 2rem;
    margin-bottom: 20px;
    text-align: center;
    color: #333;
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
  }
}
</style>
