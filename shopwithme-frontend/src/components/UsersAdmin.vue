<template>
  <div class="users-admin">
    <h2>Zarządzanie użytkownikami</h2>
    <router-link to="/admin/users/create" class="btn btn-primary my-3">
      Dodaj użytkownika
    </router-link>

    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie użytkowników...</p>
    </div>

    <div v-else>
      <table class="table table-striped table-bordered">
        <thead class="thead-dark">
          <tr>
            <th>ID</th>
            <th>Nazwa użytkownika</th>
            <th>Email</th>
            <th>Akcje</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in paginatedUsers" :key="user.id">
            <td>{{ user.id }}</td>
            <td>{{ user.userName }}</td>
            <td>{{ user.email || "Brak" }}</td>
            <td class="text-center">
              <router-link
                :to="{ name: 'UserEdit', params: { id: user.id } }"
                class="btn btn-sm btn-warning mx-1"
              >
                Edytuj
              </router-link>
            </td>
          </tr>
          <tr v-if="paginatedUsers.length === 0">
            <td colspan="4" class="text-center">
              Brak dostępnych użytkowników.
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination Controls -->
      <div class="pagination-container">
        <div class="page-size-selector">
          <label for="pageSize">Ilość użytkowników na stronę:</label>
          <select
            id="pageSize"
            class="page-size-select"
            :value="pageSize"
            @change="changePageSize($event)"
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
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class UsersAdmin extends Vue {
  userToDelete: any = null;
  showConfirmModal = false;

  get paginatedUsers() {
    return this.$store.getters["admin/adminUsers/allUsers"];
  }

  get pageSize() {
    return this.$store.getters["admin/adminUsers/pageSize"];
  }

  get pageIndex() {
    return this.$store.getters["admin/adminUsers/pageIndex"];
  }

  get totalPages() {
    return this.$store.getters["admin/adminUsers/totalPages"];
  }

  get isLoading() {
    return this.$store.getters["admin/adminUsers/isLoading"];
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
    await this.fetchUsers();
  }

  async fetchUsers() {
    await this.$store.dispatch("admin/adminUsers/fetchUsers");
  }

  async changePage(page: number) {
    if (page > 0 && page <= this.totalPages) {
      await this.$store.dispatch("admin/adminUsers/changePage", page);
      this.scrollToTop();
    }
  }

  async changePageSize(event: Event) {
    const target = event.target as HTMLSelectElement;
    const size = Number(target.value);
    await this.$store.dispatch("admin/adminUsers/changePageSize", size);
    this.scrollToTop();
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
}
</script>

<style scoped lang="scss">
.users-admin {
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
