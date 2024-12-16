<template>
  <div class="order-edit-admin">
    <!-- Loader -->
    <div v-if="isLoading" class="loading-overlay">
      <div class="spinner"></div>
      <p>Ładowanie zamówienia...</p>
    </div>

    <!-- Main Content -->
    <div v-else-if="order">
      <h2 class="text-center mb-4">Edytuj Zamówienie #{{ order.id }}</h2>

      <!-- Order Form -->
      <form @submit.prevent="saveOrder" class="order-form mx-auto">
        <div class="order-section mb-4">
          <h4 class="section-title">Dane Kontaktowe</h4>
          <div class="form-group">
            <label>Imię</label>
            <input v-model="form.firstname" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Nazwisko</label>
            <input v-model="form.lastname" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Email</label>
            <input v-model="form.email" type="email" class="form-control" />
          </div>
          <div class="form-group">
            <label>Telefon</label>
            <input
              v-model="form.phoneNumber"
              type="text"
              class="form-control"
            />
          </div>
        </div>

        <!-- Address -->
        <div class="order-section mb-4">
          <h4 class="section-title">Adres</h4>
          <div class="form-group">
            <label>Ulica</label>
            <input v-model="form.address" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Miasto</label>
            <input v-model="form.city" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Kod pocztowy</label>
            <input v-model="form.zip" type="text" class="form-control" />
          </div>
        </div>

        <!-- Save Button -->
        <div class="actions text-center">
          <button type="submit" class="btn save-changes-button">
            Zapisz zmiany
          </button>
          <button @click="goBack" type="button" class="btn btn-secondary">
            Powrót
          </button>
        </div>
      </form>

      <!-- Cart Lines -->
      <div class="order-section mt-5">
        <h4 class="section-title">Produkty w Zamówieniu</h4>
        <table class="table table-striped table-bordered text-center">
          <thead class="thead-dark">
            <tr>
              <th>Produkt</th>
              <th>Ilość</th>
              <th>Cena</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="line in order.lines" :key="line.id">
              <td>{{ line.product.name }}</td>
              <td>{{ line.quantity }}</td>
              <td>{{ line.product.price }} PLN</td>
              <td>
                <button
                  @click="removeCartLine(line)"
                  class="btn btn-sm btn-danger"
                >
                  Usuń
                </button>
              </td>
            </tr>
          </tbody>
        </table>

        <!-- Add Product to CartLine -->
        <div class="add-cart-line mt-4">
          <h4 class="section-title">Dodaj Produkt</h4>
          <div class="form-inline justify-content-center">
            <div class="form-group mr-2">
              <label class="mr-2">Produkt</label>
              <input
                v-model="productSearch"
                @input="fetchProductsAutocomplete"
                class="form-control"
                placeholder="Wpisz nazwę produktu"
                list="product-options"
              />
              <datalist id="product-options">
                <option
                  v-for="product in autocompleteProducts"
                  :key="product.id"
                  :value="product.name"
                  :data-id="product.id"
                ></option>
              </datalist>
            </div>
            <div class="form-group mr-2">
              <label class="mr-2">Ilość</label>
              <input
                v-model.number="newCartLine.quantity"
                type="number"
                class="form-control"
                min="1"
              />
            </div>
            <button @click="addCartLine" class="btn add-product-button">
              Dodaj
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class OrderEditAdmin extends Vue {
  order: any = null;
  form = {
    id: 0,
    firstname: "",
    lastname: "",
    email: "",
    phoneNumber: "",
    address: "",
    city: "",
    zip: "",
  };
  productSearch = "";
  autocompleteProducts: any[] = [];
  newCartLine = { productId: 0, quantity: 1 };

  get isLoading() {
    return this.$store.getters["admin/adminOrders/isLoading"];
  }

  async fetchOrderDetails() {
    const orderId = Number(this.$route.params.id);
    await this.$store.dispatch("admin/adminOrders/fetchOrder", orderId);
    this.order = this.$store.getters["admin/adminOrders/selectedOrder"];
    Object.assign(this.form, this.order);
  }

  async fetchProductsAutocomplete() {
    try {
      const response = await this.$axios.get("/api/products/autocomplete", {
        params: { query: this.productSearch },
      });
      this.autocompleteProducts = response.data;
    } catch (error) {
      console.error("Error fetching autocomplete:", error);
    }
  }

  addCartLine() {
    const product = this.autocompleteProducts.find(
      (p) => p.name === this.productSearch
    );
    if (product) {
      this.$store.dispatch("admin/adminOrders/updateCartLine", {
        productId: product.id,
        quantity: this.newCartLine.quantity,
        orderId: this.order.id,
      });
    }
  }

  removeCartLine(line: any) {
    this.$store.dispatch("admin/adminOrders/removeCartLine", {
      productId: line.product.id,
      quantity: line.quantity,
      orderId: this.order.id,
    });
  }

  async saveOrder() {
    await this.$store.dispatch("admin/adminOrders/updateOrder", this.form);
    this.$router.push({ name: "OrderAdmin" });
  }

  goBack() {
    this.$router.push({ name: "OrderAdmin" });
  }

  async created() {
    await this.fetchOrderDetails();
  }
}
</script>

<style scoped lang="scss">
.order-edit-admin {
  padding: 2rem;
  max-width: 900px;
  margin: auto;

  .order-section {
    margin-bottom: 2rem;

    .section-title {
      margin-top: 40px;
      font-weight: bold;
      color: #333;
      margin-bottom: 1rem;
    }

    .form-group {
      margin-bottom: 1rem;

      .form-control {
        width: 100%;
        max-width: 400px;
        margin: auto;
      }
    }
  }

  .actions {
    text-align: center;

    .btn {
      margin-right: 10px;
      padding: 0.5rem 1.5rem;
      font-size: 1rem;
    }

    .save-changes-button {
      background-color: #c70a0a;
      color: white;
      border: none;
    }
  }

  .table {
    margin-top: 1rem;

    th {
      background-color: #918e8e;
      color: #fff;
      text-align: center;
    }

    td {
      text-align: center;
      vertical-align: middle;
    }
  }

  .add-cart-line {
    text-align: center;

    .form-inline {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 10px;

      .form-group {
        margin: 0;
        display: flex;
        flex-direction: column;
        align-items: flex-start;

        label {
          margin-bottom: 0.3rem;
          font-size: 0.9rem;
          color: #555;
          font-weight: bold;
        }

        .form-control {
          height: 38px;
          font-size: 1rem;
        }
      }

      button {
        height: 38px;
        line-height: 1.5;
        padding: 5px 15px;
        font-size: 1rem;
        background-color: #c70a0a;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        position: relative;
        top: 12px;

        &:hover {
          background-color: darken(#c70a0a, 10%);
        }
      }
    }
  }
}
</style>
