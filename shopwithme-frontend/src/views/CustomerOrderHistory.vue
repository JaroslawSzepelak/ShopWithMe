<template>
  <div class="order-history">
    <h4 class="orders-header text-white text-center py-3">Twoje Zamówienia</h4>

    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie zamówień...</p>
    </div>

    <div v-else-if="error" class="error-message text-center text-danger">
      <p>{{ error }}</p>
    </div>

    <div v-else>
      <div class="table-container">
        <table class="table table-striped table-bordered">
          <thead class="thead-dark">
            <tr>
              <th>Numer zamówienia</th>
              <th>Data złożenia zamówienia</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="orders.length === 0">
              <td colspan="3" class="text-center">Brak zamówień</td>
            </tr>
            <tr v-for="order in orders" :key="order.id">
              <td>{{ order.id }}</td>
              <td>{{ formatDate(order.dateCreated) }}</td>
              <td class="table-center">
                <button
                  class="btn btn-sm btn-primary"
                  @click="viewOrder(order)"
                >
                  Szczegóły
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="pagination-container">
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
        <div>
          <label>
            Rozmiar strony:
            <select @change="changePageSize($event)" :value="pageSize">
              <option :value="10">10</option>
              <option :value="20">20</option>
              <option :value="30">30</option>
            </select>
          </label>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class CustomerOrderHistory extends Vue {
  get orders() {
    return this.$store.getters["order/allOrders"];
  }
  get isLoading() {
    return this.$store.getters["order/isLoading"];
  }
  get error() {
    return this.$store.getters["order/error"];
  }
  get pageIndex() {
    return this.$store.getters["order/pageIndex"];
  }
  get totalPages() {
    return this.$store.getters["order/totalPages"];
  }
  get pageSize() {
    return this.$store.getters["order/pageSize"];
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

  async fetchOrders() {
    await this.$store.dispatch("order/fetchOrders");
  }

  async changePage(page: number) {
    await this.$store.dispatch("order/changePage", page);
  }

  async changePageSize(event: Event) {
    const target = event.target as HTMLSelectElement;
    await this.$store.dispatch("order/changePageSize", parseInt(target.value));
  }

  viewOrder(order: any) {
    this.$router.push({
      name: "CustomerOrderDetails",
      params: { id: order.id },
    });
  }

  async created() {
    await this.fetchOrders();
  }
}
</script>

<style scoped lang="scss">
.order-history {
  .orders-header {
    background-color: #c70a0a;
  }

  .table-container {
    margin: 20px auto;
    max-width: 80%;
  }

  .table-center {
    text-align: center;
  }

  .pagination-container {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 10px;
    margin-top: 20px;
  }

  .loading-overlay {
    text-align: center;
    padding: 20px;
  }

  .error-message {
    margin: 20px;
  }
}
</style>
