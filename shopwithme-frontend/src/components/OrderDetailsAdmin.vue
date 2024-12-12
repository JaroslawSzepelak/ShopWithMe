<template>
  <div class="order-details-admin">
    <h2 class="text-center">Szczegóły Zamówienia #{{ order.id }}</h2>
    <div class="order-section">
      <h3>Dane Kontaktowe</h3>
      <p>
        <strong>Imię i Nazwisko:</strong> {{ order.firstname }}
        {{ order.lastname }}
      </p>
      <p><strong>Email:</strong> {{ order.email }}</p>
      <p><strong>Telefon:</strong> {{ order.phoneNumber }}</p>
    </div>
    <div class="order-section">
      <h3>Adres</h3>
      <p><strong>Ulica:</strong> {{ order.address }}</p>
      <p><strong>Miasto:</strong> {{ order.city }}</p>
      <p><strong>Kod pocztowy:</strong> {{ order.zip }}</p>
    </div>
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
    <div class="order-summary">
      <h3>Podsumowanie Zamówienia</h3>
      <p><strong>Suma:</strong> {{ calculateTotal(order).toFixed(2) }} PLN</p>
    </div>
    <div class="actions">
      <button @click="goBack" class="btn btn-secondary">Powrót</button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class OrderDetailsAdmin extends Vue {
  get order() {
    return this.$store.getters["order/selectedOrder"];
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
}
</style>
