<template>
  <div class="product-admin">
    <router-link to="/admin/products/create" class="btn btn-primary my-3">
      Utwórz produkt
    </router-link>
    <table class="table table-striped table-bordered">
      <thead class="thead-dark">
        <tr>
          <th>ID</th>
          <th>Nazwa</th>
          <th>Kategoria</th>
          <th class="text-right">Cena</th>
          <th>Akcje</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in paginatedProducts" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.name }}</td>
          <td>{{ product.categoryId }}</td>
          <td class="text-right">{{ product.price.toFixed(2) }} PLN</td>
          <td class="text-center">
            <button
              class="btn btn-sm btn-danger mx-1"
              @click="removeProduct(product.id)"
            >
              Usuń
            </button>
            <button
              class="btn btn-sm btn-warning mx-1"
              @click="handleEdit(product)"
            >
              Edytuj
            </button>
          </td>
        </tr>
        <tr v-if="paginatedProducts.length === 0">
          <td colspan="5" class="text-center">Brak dostępnych produktów.</td>
        </tr>
      </tbody>
    </table>
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
import Pagination from "@/components/Pagination.vue";

@Component({
  components: {
    Pagination,
  },
})
export default class ProductAdmin extends Vue {
  currentPage = 1;
  pageSize = 10;

  get paginatedProducts() {
    const allProducts = this.$store.getters["admin/adminProducts/allProducts"];
    if (!allProducts || !Array.isArray(allProducts)) {
      return [];
    }
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return allProducts.slice(start, end);
  }

  get pageCount() {
    const allProducts = this.$store.getters["admin/adminProducts/allProducts"];
    return Math.ceil((allProducts?.length || 0) / this.pageSize);
  }

  async beforeCreate() {
    try {
      await this.$store.dispatch("admin/adminProducts/fetchProducts");
    } catch (error: any) {
      if (error.response?.status === 404) {
        this.$router.push("/not-found");
      } else {
        console.error("Błąd podczas pobierania produktów:", error);
      }
    }
  }

  async removeProduct(id: number) {
    try {
      await this.$store.dispatch("admin/adminProducts/deleteProduct", id);
    } catch (error: any) {
      if (error.response?.status === 404) {
        this.$router.push("/not-found");
      } else {
        console.error("Błąd podczas usuwania produktu:", error);
      }
    }
  }

  handleEdit(product: { id: number }) {
    this.$router.push(`/admin/products/edit/${product.id}`);
  }

  updatePageSize() {
    this.currentPage = 1;
    console.log("Page Size Updated:", this.pageSize);
  }

  changePage(newPage: number) {
    this.currentPage = newPage;
    console.log("Page Changed to:", newPage);
  }
}
</script>

<style scoped lang="scss">
.product-admin {
  .btn {
    margin-bottom: 15px;
  }

  .table {
    width: 100%;
    margin-top: 20px;
  }

  .thead-dark {
    background-color: #343a40;
    color: white;
  }

  .text-right {
    text-align: right;
  }

  .text-center {
    text-align: center;
  }

  .pagination-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: 20px;

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
