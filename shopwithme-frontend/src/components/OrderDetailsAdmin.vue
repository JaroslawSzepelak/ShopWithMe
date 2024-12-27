<template>
  <div class="order-details-admin">
    <!-- Spinner / Loader -->
    <div v-if="isLoading || !order" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie szczegółów zamówienia...</p>
    </div>

    <!-- Główna zawartość -->
    <div v-else class="order-container">
      <h2 class="text-center">Szczegóły Zamówienia #{{ order.id }}</h2>

      <!-- Status Zamówienia -->
      <div class="order-status text-center">
        <span :class="getStatusClass(order.status)">
          Status: {{ getStatusLabel(order.status) }}
        </span>
      </div>

      <!-- Dane kontaktowe -->
      <div class="order-section">
        <h3>Dane Kontaktowe</h3>
        <p>
          <strong>Imię i Nazwisko:</strong> {{ order.firstname }}
          {{ order.lastname }}
        </p>
        <p><strong>Email:</strong> {{ order.email }}</p>
        <p><strong>Telefon:</strong> {{ order.phoneNumber }}</p>
        <p>
          <strong>Data Złożenia:</strong> {{ formatDate(order.dateCreated) }}
        </p>
      </div>

      <!-- Adres -->
      <div class="order-section">
        <h3>Adres</h3>
        <p><strong>Ulica:</strong> {{ order.address }}</p>
        <p><strong>Miasto:</strong> {{ order.city }}</p>
        <p><strong>Kod pocztowy:</strong> {{ order.zip }}</p>
      </div>

      <!-- Produkty -->
      <div class="order-section">
        <h3>Produkty</h3>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Nazwa Produktu</th>
              <th>Ilość</th>
              <th>Cena Jednostkowa</th>
              <th>Cena Łączna</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="line in order.lines" :key="line.id">
              <td>{{ line.product.name }}</td>
              <td>{{ line.quantity }}</td>
              <td>{{ line.product.price.toFixed(2) }} PLN</td>
              <td>{{ (line.quantity * line.product.price).toFixed(2) }} PLN</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Podsumowanie -->
      <div class="order-summary">
        <p><strong>Suma:</strong> {{ calculateTotal(order).toFixed(2) }} PLN</p>
      </div>

      <!-- Akcje -->
      <div class="actions">
        <button @click="goBack" class="btn btn-secondary">Powrót</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class OrderDetailsAdmin extends Vue {
  get order() {
    return this.$store.getters["admin/adminOrders/selectedOrder"];
  }

  get isLoading() {
    return this.$store.getters["admin/adminOrders/isLoading"];
  }

  async fetchOrderDetails() {
    const orderId = Number(this.$route.params.id);
    if (!this.order || this.order.id !== orderId) {
      await this.$store.dispatch("admin/adminOrders/fetchOrder", orderId);
    }
  }

  async created() {
    await this.fetchOrderDetails();
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

  calculateTotal(order: any): number {
    if (order?.lines && Array.isArray(order.lines)) {
      return order.lines.reduce((total: number, line: any) => {
        const quantity = line.quantity || 0;
        const price = line.product?.price || 0;
        return total + quantity * price;
      }, 0);
    }
    return 0;
  }

  getStatusLabel(status: number): string {
    const labels = ["Zlecone", "Wysłane", "Zakończone", "Anulowane"];
    return labels[status] || "Nieznany";
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0:
        return "status-pending";
      case 1:
        return "status-shipped";
      case 2:
        return "status-completed";
      case 3:
        return "status-canceled";
      default:
        return "";
    }
  }

  goBack() {
    this.$router.push({ name: "OrderAdmin" });
  }
}
</script>

<style scoped lang="scss">
.order-details-admin {
  padding: 2rem;

  .order-status {
    margin-top: 30px;
    margin-bottom: 2rem;

    span {
      font-size: 1.2rem;
      font-weight: bold;
      padding: 0.5rem 1rem;
      border-radius: 5px;
    }

    .status-pending {
      background-color: #fffbcc;
      color: #856404;
      border: 1px solid #ffeeba;
    }

    .status-shipped {
      background-color: #d1ecf1;
      color: #0c5460;
      border: 1px solid #bee5eb;
    }

    .status-completed {
      background-color: #d4edda;
      color: #155724;
      border: 1px solid #c3e6cb;
    }

    .status-canceled {
      background-color: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }
  }

  .order-section {
    margin-bottom: 2rem;

    h3 {
      margin-bottom: 1rem;
      color: #333;
    }

    p {
      margin: 0.5rem 0;
    }
  }

  .table {
    width: 100%;
    margin-top: 1rem;
    border-collapse: collapse;

    th,
    td {
      padding: 0.75rem;
      text-align: left;
      border: 1px solid #ddd;
    }

    th {
      background-color: #c70a0a;
      color: #fff;
    }
  }

  .order-summary {
    margin-top: 2rem;

    p {
      font-size: 1.2rem;
      font-weight: bold;
    }
  }

  .actions {
    text-align: center;
    margin-top: 2rem;

    .btn {
      background-color: #6c757d;
      color: white;
      padding: 0.5rem 1rem;
      border: none;
      border-radius: 5px;
      cursor: pointer;

      &:hover {
        background-color: #5a6268;
      }
    }
  }

  .loading-overlay {
    text-align: center;
    margin-top: 2rem;

    .spinner {
      box-sizing: border-box;
      width: 40px;
      height: 40px;
      margin: auto;
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

    p {
      margin-top: 1rem;
      font-size: 1.2rem;
      color: #555;
    }
  }
}

@media (max-width: 768px) {
  .order-details-admin {
    padding: 1rem;

    .order-status {
      font-size: 1rem;
      padding: 8px;
    }

    .order-section {
      h3 {
        font-size: 1.2rem;
      }

      p {
        font-size: 0.9rem;
      }
    }

    .table {
      display: block;
      width: 600px;
      overflow-x: auto;
      font-size: 0.9rem;
      text-wrap: nowrap;

      th,
      td {
        padding: 0.5rem;
      }
    }

    .order-summary p {
      font-size: 1rem;
    }

    .actions .btn {
      font-size: 0.9rem;
      padding: 0.4rem 0.8rem;
    }
  }
}

@media (max-width: 480px) {
  .order-details-admin {
    .order-status {
      font-size: 0.9rem;
      max-width: 100%;
      margin: 10px 0;
    }

    .order-section {
      h3 {
        font-size: 1rem;
      }

      p {
        font-size: 0.8rem;
      }
    }

    .table {
      th,
      td {
        white-space: nowrap;
        font-size: 0.8rem;
      }
    }

    .order-summary p {
      font-size: 0.9rem;
    }

    .actions .btn {
      font-size: 0.8rem;
      padding: 0.3rem 0.6rem;
    }
  }
}
</style>
