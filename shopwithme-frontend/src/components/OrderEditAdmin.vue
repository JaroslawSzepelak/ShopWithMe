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

      <!-- Order Details -->
      <div class="order-details text-center mb-4">
        <p>
          <strong>Data złożenia:</strong> {{ formatDate(order.dateCreated) }}
        </p>
      </div>

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

        <!-- Order Status -->
        <div class="order-section mb-4">
          <h4 class="section-title">Status Zamówienia</h4>
          <div class="form-group">
            <label>Status</label>
            <select v-model.number="form.status" class="form-control">
              <option
                v-for="(label, value) in statusOptions"
                :key="value"
                :value="value"
              >
                {{ label }}
              </option>
            </select>
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
              <th>Cena Łączna</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="line in order.lines" :key="line.id">
              <td>{{ line.product.name }}</td>
              <td>{{ line.quantity }}</td>
              <td>{{ getLinePrice(line).toFixed(2) }} zł</td>
              <td>{{ (line.quantity * getLinePrice(line)).toFixed(2) }} zł</td>
              <td>
                <button
                  @click="confirmRemoveCartLine(line)"
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
              <SearchAutocomplete
                :isAsync="true"
                :items="autocompleteProducts"
                :isLoading="loadingAutocomplete"
                @search="onProductSearch"
                @select="onProductSelect"
              />
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
            <button
              @click="addCartLine"
              class="btn add-product-button"
              :disabled="!selectedProduct"
            >
              Dodaj
            </button>
          </div>
          <p
            v-if="!selectedProduct && productSearch.length > 0"
            class="text-danger mt-2"
          >
            Musisz wybrać produkt z listy.
          </p>
        </div>
      </div>
    </div>
    <!-- Confirmation Modal -->
    <ConfirmationModal
      v-if="isConfirmationModalVisible"
      :visible="isConfirmationModalVisible"
      :message="'Czy na pewno chcesz usunąć produkt?'"
      @confirm="removeCartLine"
      @close="isConfirmationModalVisible = false"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SearchAutocomplete from "@/components/SearchAutocompleteOrderEditor.vue";
import ConfirmationModal from "@/components/modals/ConfirmationModal.vue";

@Component({
  components: { SearchAutocomplete, ConfirmationModal },
})
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
    status: 0,
  };
  productSearch = "";
  autocompleteProducts: { id: number; name: string }[] = [];
  selectedProduct: { id: number; name: string } | null = null;
  loadingAutocomplete = false;
  newCartLine = { productId: 0, quantity: 1 };
  isConfirmationModalVisible = false;
  lineToRemove: any = null;

  statusOptions = {
    0: "Zlecone",
    1: "Wysłane",
    2: "Zakończone",
    3: "Anulowane",
  };

  get isLoading() {
    return this.$store.getters["admin/adminOrders/isLoading"];
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

  getLinePrice(line: any): number {
    return line.price > 0 ? line.price : line.product.price;
  }

  async fetchOrderDetails() {
    const orderId = Number(this.$route.params.id);
    await this.$store.dispatch("admin/adminOrders/fetchOrder", orderId);
    this.order = this.$store.getters["admin/adminOrders/selectedOrder"];
    Object.assign(this.form, this.order);
  }

  async onProductSearch(query: string) {
    if (query.trim().length >= 3) {
      this.loadingAutocomplete = true;
      try {
        await this.$store.dispatch(
          "admin/adminProducts/fetchAutocompleteProducts",
          query
        );
        this.autocompleteProducts =
          this.$store.getters["admin/adminProducts/autocompleteProducts"];
      } catch (error) {
        console.error("Błąd podczas wyszukiwania produktów:", error);
      } finally {
        this.loadingAutocomplete = false;
      }
    } else {
      this.autocompleteProducts = [];
    }
  }

  onProductSelect(product: { id: number; name: string }) {
    this.selectedProduct = product;
  }

  addCartLine() {
    if (!this.selectedProduct) return;

    this.$store.dispatch("admin/adminOrders/updateCartLine", {
      productId: this.selectedProduct.id,
      quantity: this.newCartLine.quantity,
      orderId: this.order.id,
    });
    this.selectedProduct = null;
    this.autocompleteProducts = [];
    this.productSearch = "";
    location.reload();
  }

  confirmRemoveCartLine(line: any) {
    this.lineToRemove = line;
    this.isConfirmationModalVisible = true;
  }

  removeCartLine() {
    if (this.lineToRemove) {
      this.$store.dispatch("admin/adminOrders/removeCartLine", {
        productId: this.lineToRemove.product.id,
        quantity: this.lineToRemove.quantity,
        orderId: this.order.id,
      });
      this.isConfirmationModalVisible = false;
      location.reload();
    }
  }

  async saveOrder() {
    try {
      const updatedOrder = {
        id: this.form.id,
        firstname: this.form.firstname,
        lastname: this.form.lastname,
        email: this.form.email,
        phoneNumber: this.form.phoneNumber,
        address: this.form.address,
        city: this.form.city,
        zip: this.form.zip,
        status: Number(this.form.status),
      };

      await this.$store.dispatch("admin/adminOrders/updateOrder", updatedOrder);
      this.$router.push({ name: "OrderAdmin" });
    } catch (error) {
      console.error("Błąd podczas aktualizacji zamówienia:", error);
      alert(
        "Nie udało się zapisać zmian. Sprawdź wprowadzone dane i spróbuj ponownie."
      );
    }
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
  padding-bottom: 200px;
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

@media (max-width: 768px) {
  .order-edit-admin {
    padding: 1rem;

    .order-section {
      .section-title {
        font-size: 1.2rem;
      }

      .form-group {
        .form-control {
          max-width: 100%;
        }
      }
    }

    .actions .btn {
      font-size: 0.9rem;
      padding: 0.4rem 1rem;
    }

    .table {
      font-size: 0.9rem;

      th,
      td {
        padding: 0.5rem;
      }
    }

    .add-cart-line {
      .form-inline {
        flex-direction: column;

        .form-group {
          width: 90%;

          label {
            font-size: 0.8rem;
          }

          .form-control {
            width: 100%;
          }
        }

        button {
          width: 60%;
        }
      }
    }
  }
}

@media (max-width: 480px) {
  .order-edit-admin {
    .order-section {
      .section-title {
        font-size: 1rem;
      }

      .form-group {
        .form-control {
          font-size: 0.8rem;
        }
      }
    }

    .actions .btn {
      font-size: 0.8rem;
      padding: 0.3rem 0.8rem;
    }

    .table {
      font-size: 0.8rem;
      text-wrap: nowrap;
      th,
      td {
        padding: 0.4rem;
      }
    }

    .add-cart-line {
      .form-inline {
        .form-group {
          label {
            font-size: 0.7rem;
          }

          .form-control {
            font-size: 0.8rem;
          }
        }

        button {
          font-size: 0.8rem;
        }
      }
    }
  }
}
</style>
