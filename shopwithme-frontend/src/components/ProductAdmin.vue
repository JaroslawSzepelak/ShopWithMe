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
          <td>{{ product.category }}</td>
          <td class="text-right">{{ product.price.toFixed(2) }} PLN</td>
          <td class="text-center">
            <button
              class="btn btn-sm btn-danger mx-1"
              @click="confirmRemoveProduct(product)"
            >
              Usuń
            </button>
            <router-link
              :to="{ name: 'ProductEdit', params: { id: product.id } }"
              class="btn btn-sm btn-warning mx-1"
            >
              Edytuj
            </router-link>
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

    <!-- Modal -->
    <div v-if="showConfirmModal" class="modal-overlay">
      <div class="modal-content">
        <p class="modal-message">
          Czy na pewno chcesz usunąć produkt: "{{ productToDelete?.name }}"?
        </p>
        <div class="modal-buttons">
          <button @click="removeProduct" class="btn modal-btn">Tak</button>
          <button @click="closeModal" class="btn modal-btn cancel-btn">
            Nie
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Pagination from "@/components/ProductPagination.vue";

@Component({
  components: {
    Pagination,
  },
})
export default class ProductAdmin extends Vue {
  currentPage = 1;
  pageSize = 10;

  productToDelete: any = null;
  showConfirmModal = false;

  get modalMessage() {
    return this.$route.query.modalMessage || null;
  }

  closeModal() {
    this.productToDelete = null;
    this.showConfirmModal = false;
  }

  confirmRemoveProduct(product: any) {
    this.productToDelete = product;
    this.showConfirmModal = true;
  }

  async removeProduct() {
    if (!this.productToDelete) return;

    try {
      await this.$store.dispatch(
        "admin/adminProducts/deleteProduct",
        this.productToDelete.id
      );
      this.closeModal();
      this.$store.dispatch("admin/adminProducts/fetchProducts");
    } catch (error) {
      console.error("Błąd podczas usuwania produktu:", error);
    }
  }

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
      console.error("Błąd podczas pobierania produktów:", error);
    }
  }

  updatePageSize() {
    this.currentPage = 1;
  }

  changePage(newPage: number) {
    this.currentPage = newPage;
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

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background-color: #ffffff;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  text-align: center;
  max-width: 400px;
  width: 100%;
}

.modal-message {
  font-size: 1.2rem;
  color: #333333;
  margin-bottom: 15px;
}

.modal-buttons {
  display: flex;
  gap: 10px;
  justify-content: center;
}

.modal-btn {
  background-color: #c70a0a;
  color: white;
  border: none;
  padding: 10px 20px;
  font-size: 1rem;
  font-weight: bold;
  border-radius: 5px;
  cursor: pointer;

  &:hover {
    background-color: #a60a0a;
  }

  &.cancel-btn {
    background-color: #666666;

    &:hover {
      background-color: #555555;
    }
  }
}
</style>
