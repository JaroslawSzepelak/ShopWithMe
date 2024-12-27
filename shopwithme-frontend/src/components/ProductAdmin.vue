<template>
  <div class="product-admin">
    <router-link to="/admin/products/create" class="btn btn-primary my-3">
      Utwórz produkt
    </router-link>

    <!-- Loading Spinner -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie produktów...</p>
    </div>

    <!-- Product Table -->
    <div v-else>
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

      <!-- Pagination Controls -->
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
              :class="{ 'btn-primary': page === pageIndex }"
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

    <!-- Confirmation Modal -->
    <ConfirmationModal
      v-if="showConfirmModal"
      :visible="showConfirmModal"
      :message="
        'Czy na pewno chcesz usunąć produkt: ' + productToDelete?.name + '?'
      "
      @confirm="removeProduct"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ConfirmationModal from "@/components/modals/ConfirmationModal.vue";

@Component({
  components: { ConfirmationModal },
})
export default class ProductAdmin extends Vue {
  productToDelete: any = null;
  showConfirmModal = false;

  get paginatedProducts() {
    return this.$store.getters["admin/adminProducts/allProducts"];
  }

  get pageSize() {
    return this.$store.getters["admin/adminProducts/pageSize"];
  }

  get pageIndex() {
    return this.$store.getters["admin/adminProducts/pageIndex"];
  }

  get totalPages() {
    return this.$store.getters["admin/adminProducts/totalPages"];
  }

  get isLoading() {
    return this.$store.getters["admin/adminProducts/isLoading"];
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

  async created() {
    this.fetchProducts();
  }

  async fetchProducts() {
    await this.$store.dispatch("admin/adminProducts/fetchProducts");
  }

  async changePage(page: number) {
    if (page > 0 && page <= this.totalPages) {
      await this.$store.dispatch("admin/adminProducts/changePage", page);
      this.scrollToTop();
    }
  }

  async changePageSize(size: number) {
    await this.$store.dispatch(
      "admin/adminProducts/changePageSize",
      Number(size)
    );
    this.scrollToTop();
  }

  confirmRemoveProduct(product: any) {
    this.productToDelete = product;
    this.showConfirmModal = true;
  }

  closeModal() {
    this.productToDelete = null;
    this.showConfirmModal = false;
  }

  async removeProduct() {
    if (!this.productToDelete) return;

    try {
      await this.$store.dispatch(
        "admin/adminProducts/deleteProduct",
        this.productToDelete.id
      );
      this.closeModal();
      await this.fetchProducts();
    } catch (error) {
      console.error("Błąd podczas usuwania produktu:", error);
    }
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: "smooth" });
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

@media (max-width: 1250px) {
  .product-admin {
    padding: 1rem;
    .pagination-container {
      flex-direction: column;
      align-items: center;
      gap: 30px;
    }

    .page-size-selector {
      margin-top: 10px;
    }
  }
}

@media (max-width: 1100px) {
  .product-admin {
    padding: 1rem;

    .table {
      display: block;
      width: 700px;
      overflow-x: auto;
      font-size: 0.9rem;
      text-wrap: nowrap;

      th,
      td {
        padding: 0.5rem;
      }
    }
  }
}

@media (max-width: 550px) {
  .product-admin {
    .table {
      th,
      td {
        white-space: nowrap;
        font-size: 0.8rem;
      }
    }
    .page-size-selector {
      select {
        font-size: 0.8rem;
      }

      label {
        font-size: 0.8rem;
      }
    }

    .pagination-container {
      .pagination-controls {
        .btn {
          padding: 5px 10px;
        }
      }
    }
  }
}
</style>
