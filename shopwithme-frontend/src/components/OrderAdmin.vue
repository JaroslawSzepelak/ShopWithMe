<template>
  <div class="order-admin">
    <h4 class="bg-info text-white text-center p-2">Zamówienia</h4>
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
      <thead>
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
          <td>{{ order.name }}</td>
          <td>{{ `${order.city}, ${order.zip}` }}</td>
          <td class="text-right">{{ getTotal(order).toFixed(2) }} PLN</td>
          <td class="text-center">
            <button
              class="btn btn-sm"
              :class="order.shipped ? 'btn-success' : 'btn-danger'"
              @click="toggleShipped(order)"
            >
              {{ order.shipped ? "Anuluj wysyłkę" : "Potwierdź wysyłkę" }}
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class OrderAdmin extends Vue {
  showShipped = false;

  get displayOrders() {
    const orders = this.$store.getters["orders/allOrders"];
    return this.showShipped
      ? orders
      : orders.filter((order: any) => !order.shipped);
  }

  getTotal(order: any): number {
    if (order.cartLines && order.cartLines.length > 0) {
      return order.cartLines.reduce(
        (total: number, line: any) =>
          total + line.quantity * line.product.price,
        0
      );
    }
    return 0;
  }

  async toggleShipped(order: any): Promise<void> {
    try {
      const updatedOrder = { ...order, shipped: !order.shipped };
      await this.$store.dispatch("orders/updateOrder", updatedOrder);
      await this.$store.dispatch("orders/fetchOrders");
    } catch (error) {
      console.error("Błąd podczas aktualizacji zamówienia:", error);
    }
  }

  async created(): Promise<void> {
    try {
      await this.$store.dispatch("orders/fetchOrders");
    } catch (error) {
      console.error("Błąd podczas pobierania zamówień:", error);
    }
  }
}
</script>

<style scoped lang="scss">
.order-admin {
  .table {
    margin-top: 20px;
  }

  .form-check-label {
    margin-left: 10px;
  }

  .btn {
    min-width: 150px;
  }

  .btn-success {
    background-color: #28a745;
    color: white;
  }

  .btn-danger {
    background-color: #dc3545;
    color: white;
  }
}
</style>
