<template>
  <div class="order-admin">
    <h4 class="orders-header text-white text-center py-3">Zamówienia</h4>
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie zamówień...</p>
    </div>
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
            <th>Imię i nazwisko</th>
            <th>Miasto, kod pocztowy</th>
            <th class="text-right">Suma łączna</th>
            <th>Akcje</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="displayOrders.length === 0">
            <td colspan="5" class="text-center">Nie ma zamówień</td>
          </tr>
          <tr v-for="order in displayOrders" :key="order.id">
            <td>{{ order.id }}</td>
            <td>{{ getFullName(order) }}</td>
            <td>{{ `${order.city}, ${order.zip}` }}</td>
            <td class="text-right">
              {{ calculateTotal(order).toFixed(2) }} PLN
            </td>
            <td class="text-center">
              <button
                class="btn btn-sm btn-warning mx-1"
                @click="toggleShipped(order)"
              >
                {{ order.shipped ? "Anuluj wysyłkę" : "Potwierdź wysyłkę" }}
              </button>
              <button
                class="btn btn-sm btn-primary mx-1"
                @click="viewOrderDetails(order)"
              >
                Szczegóły
              </button>
              <button
                class="btn btn-sm btn-danger mx-1"
                @click="confirmRemoveOrder(order)"
              >
                Usuń
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Confirmation Modal -->
    <ConfirmationModal
      v-if="showConfirmModal"
      :visible="showConfirmModal"
      :message="
        'Czy na pewno chcesz usunąć zamówienie: ' + orderToDelete?.id + '?'
      "
      @confirm="removeOrder"
      @close="closeModal"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ConfirmationModal from "@/components/ConfirmationModal.vue";

@Component({
  components: { ConfirmationModal },
})
export default class OrderAdmin extends Vue {
  showShipped = false;
  showConfirmModal = false;
  orderToDelete: any = null;

  get displayOrders() {
    const orders = this.$store.getters["order/allOrders"];
    return orders
      ? this.showShipped
        ? orders
        : orders.filter((order: any) => !order.shipped)
      : [];
  }

  get isLoading() {
    return this.$store.getters["order/isOrderLoading"];
  }

  getFullName(order: any): string {
    return `${order.firstname || ""} ${order.lastname || ""}`.trim();
  }

  calculateTotal(order: any): number {
    if (order.lines && Array.isArray(order.lines)) {
      return order.lines.reduce((total: number, line: any) => {
        const quantity = line.quantity || 0;
        const price = line.product?.price || 0;
        return total + quantity * price;
      }, 0);
    }
    return 0;
  }

  async toggleShipped(order: any): Promise<void> {
    try {
      const updatedOrder = { ...order, shipped: !order.shipped };
      await this.$store.dispatch("order/updateOrder", updatedOrder);
      await this.$store.dispatch("order/fetchOrders");
    } catch (error) {
      console.error("Błąd podczas aktualizacji zamówienia:", error);
    }
  }

  viewOrderDetails(order: any) {
    this.$store.dispatch("order/selectOrder", order);
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
  async removeOrder() {
    if (!this.orderToDelete) return;

    try {
      await this.$store.dispatch("order/deleteOrder", this.orderToDelete.id);
      this.closeModal();
      await this.$store.dispatch("order/fetchOrders");
    } catch (error) {
      console.error("Błąd podczas usuwania zamówienia:", error);
    }
  }

  async created(): Promise<void> {
    try {
      await this.$store.dispatch("order/fetchOrders");
      console.log("Fetched orders:", this.$store.state.order.orders);
    } catch (error) {
      console.error("Błąd podczas pobierania zamówień:", error);
    }
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
}
</style>
