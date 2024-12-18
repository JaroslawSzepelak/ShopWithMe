<template>
  <div class="order-admin">
    <h4 class="orders-header text-white text-center py-3">Zamówienia</h4>
    <!-- Loading Spinner -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie zamówień...</p>
    </div>

    <!-- Orders Table -->
    <div v-else>
      <div class="form-group text-center my-3">
        <input
          id="showShipped"
          class="form-check-input"
          type="checkbox"
          v-model="showShipped"
        />
        <label for="showShipped" class="form-check-label">
          Pokaż wysłane zamówienia
        </label>
      </div>
      <table class="table table-striped table-bordered">
        <thead class="thead-dark">
          <tr>
            <th>ID</th>
            <th>Imię</th>
            <th>Nazwisko</th>
            <th>Email</th>
            <th>Data Złożenia</th>
            <th>Akcje</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="displayOrders.length === 0">
            <td colspan="6" class="text-center">Brak zamówień</td>
          </tr>
          <tr v-for="order in displayOrders" :key="order.id">
            <td>{{ order.id }}</td>
            <td>{{ order.firstname }}</td>
            <td>{{ order.lastname }}</td>
            <td>{{ order.email }}</td>
            <td>{{ formatDate(order.dateCreated) }}</td>
            <td class="text-center">
              <button
                class="btn btn-sm btn-primary mx-1"
                @click="viewOrderDetails(order)"
              >
                Szczegóły
              </button>
              <button
                class="btn btn-sm btn-warning mx-1"
                @click="editOrder(order)"
              >
                Edytuj
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Pagination Controls -->
      <div class="pagination-container">
        <div class="page-size-selector">
          <label for="pageSize">Ilość zamówień na stronę:</label>
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
import ConfirmationModal from "@/components/modals/ConfirmationModal.vue";

@Component({
  components: { ConfirmationModal },
})
export default class OrderAdmin extends Vue {
  showShipped = false;
  showConfirmModal = false;
  orderToDelete: any = null;

  get displayOrders() {
    const orders = this.$store.getters["admin/adminOrders/allOrders"];
    return orders
      ? this.showShipped
        ? orders
        : orders.filter((order: any) => !order.shipped)
      : [];
  }

  get pageSize() {
    return this.$store.getters["admin/adminOrders/pageSize"];
  }

  get pageIndex() {
    return this.$store.getters["admin/adminOrders/pageIndex"];
  }

  get totalPages() {
    return this.$store.getters["admin/adminOrders/totalPages"];
  }

  get isLoading() {
    return this.$store.getters["admin/adminOrders/isLoading"];
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

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleString("pl-PL", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    });
  }

  async toggleShipped(order: any) {
    const updatedOrder = { ...order, shipped: !order.shipped };
    await this.$store.dispatch("admin/adminOrders/updateOrder", updatedOrder);
    await this.$store.dispatch("admin/adminOrders/fetchOrders");
  }

  async changePage(page: number) {
    if (page > 0 && page <= this.totalPages) {
      await this.$store.dispatch("admin/adminOrders/changePage", page);
    }
  }

  async changePageSize(event: Event) {
    const target = event.target as HTMLSelectElement;
    const size = Number(target.value);
    await this.$store.dispatch("admin/adminOrders/changePageSize", size);
  }

  viewOrderDetails(order: any) {
    this.$router.push({ name: "OrderDetailsAdmin", params: { id: order.id } });
  }

  confirmRemoveOrder(order: any) {
    this.orderToDelete = order;
    this.showConfirmModal = true;
  }

  closeModal() {
    this.orderToDelete = null;
    this.showConfirmModal = false;
  }

  editOrder(order: any) {
    this.$router.push({ name: "OrderEditAdmin", params: { id: order.id } });
  }

  async created() {
    await this.$store.dispatch("admin/adminOrders/fetchOrders");
  }
}
</script>
<style scoped lang="scss">
.order-admin {
  .orders-header {
    background-color: #c70a0a;
  }

  .loading-overlay {
    text-align: center;
    padding: 20px;
  }

  .spinner {
    box-sizing: border-box;
    margin: 20px auto;
    width: 40px;
    height: 40px;
    border: 4px solid rgba(0, 0, 0, 0.1);
    border-left-color: #c70a0a;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    to {
      transform: rotate(360deg);
    }
  }

  .table {
    margin-top: 20px;
    width: 100%;
  }

  .thead-dark {
    background-color: #343a40;
    color: white;
  }

  .text-center {
    display: flex;
    justify-content: center;
    gap: 15px;
  }

  .btn {
    min-width: 100px;
    margin: 0;
  }

  .btn-warning {
    background-color: #ffc107;
    color: #212529;

    &:hover {
      background-color: #e0a800;
    }
  }

  .btn-primary {
    background-color: #777472;
    color: #fff;

    &:hover {
      background-color: #0c0c0c;
    }
  }

  .btn-danger {
    background-color: #dc3545;
    color: #fff;

    &:hover {
      background-color: #c82333;
    }
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
