<template>
  <div class="category-admin">
    <router-link to="/admin/categories/create" class="btn btn-primary my-3">
      Utwórz kategorię
    </router-link>

    <!-- Loading Spinner -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie kategorii...</p>
    </div>

    <!-- Categories Table -->
    <div v-else>
      <table class="table table-striped table-bordered">
        <thead class="thead-dark">
          <tr>
            <th>ID</th>
            <th>Nazwa</th>
            <th>Akcje</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="category in paginatedCategories" :key="category.id">
            <td>{{ category.id }}</td>
            <td>{{ category.name }}</td>
            <td class="text-center">
              <button
                class="btn btn-sm btn-danger mx-1"
                @click="confirmRemoveCategory(category)"
              >
                Usuń
              </button>
              <router-link
                :to="{ name: 'CategoryEdit', params: { id: category.id } }"
                class="btn btn-sm btn-warning mx-1"
              >
                Edytuj
              </router-link>
            </td>
          </tr>
          <tr v-if="paginatedCategories.length === 0">
            <td colspan="3" class="text-center">Brak dostępnych kategorii.</td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination Controls -->
      <div class="pagination-container">
        <div class="page-size-selector">
          <label for="pageSize">Ilość kategorii na stronę:</label>
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

    <!-- External Confirm Delete Modal -->
    <ConfirmationModal
      :visible="showConfirmModal"
      :message="`Czy na pewno chcesz usunąć kategorię: '${categoryToDelete?.name}'?`"
      @confirm="removeCategory"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ConfirmationModal from "@/components/ConfirmationModal.vue";

@Component({
  components: {
    ConfirmationModal,
  },
})
export default class CategoryAdmin extends Vue {
  categoryToDelete: any = null;
  showConfirmModal = false;

  get paginatedCategories() {
    return this.$store.getters["admin/adminCategories/allCategories"];
  }

  get pageSize() {
    return this.$store.getters["admin/adminCategories/pageSize"];
  }

  get pageIndex() {
    return this.$store.getters["admin/adminCategories/pageIndex"];
  }

  get totalPages() {
    return this.$store.getters["admin/adminCategories/totalPages"];
  }

  get isLoading() {
    return this.$store.getters["admin/adminCategories/isLoading"];
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
    await this.fetchCategories();
  }

  async fetchCategories() {
    await this.$store.dispatch("admin/adminCategories/fetchCategories");
  }

  async changePage(page: number) {
    if (page > 0 && page <= this.totalPages) {
      await this.$store.dispatch("admin/adminCategories/changePage", page);
      this.scrollToTop();
    }
  }

  async changePageSize(size: number) {
    await this.$store.dispatch(
      "admin/adminCategories/changePageSize",
      Number(size)
    );
    this.scrollToTop();
  }

  confirmRemoveCategory(category: any) {
    this.categoryToDelete = category;
    this.showConfirmModal = true;
  }

  closeModal() {
    this.categoryToDelete = null;
    this.showConfirmModal = false;
  }

  async removeCategory() {
    if (!this.categoryToDelete) return;

    try {
      await this.$store.dispatch(
        "admin/adminCategories/deleteCategory",
        this.categoryToDelete.id
      );
      this.closeModal();
      await this.fetchCategories();
    } catch (error) {
      console.error("Błąd podczas usuwania kategorii:", error);
    }
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
}
</script>

<style scoped lang="scss">
.category-admin {
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
</style>
