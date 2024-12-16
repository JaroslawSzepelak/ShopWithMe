<template>
  <div class="order-details-admin">
    <!-- Spinner / Loader -->
    <div v-if="isLoading || !order" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie szczegółów zamówienia...</p>
    </div>

    <!-- Główna zawartość -->
    <div v-else>
      <h2 class="text-center">Szczegóły Zamówienia #{{ order.id }}</h2>

      <!-- Dane kontaktowe -->
      <div class="order-section">
        <h3>Dane Kontaktowe</h3>
        <p>
          <strong>Imię i Nazwisko:</strong> {{ order.firstname }}
          {{ order.lastname }}
        </p>
        <p><strong>Email:</strong> {{ order.email }}</p>
        <p><strong>Telefon:</strong> {{ order.phoneNumber }}</p>
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

  goBack() {
    this.$router.push({ name: "OrderAdmin" });
  }
}
</script>

<style scoped lang="scss">
.order-details-admin {
  padding: 2rem;
  background-color: #f9f9f9;

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
</style>
