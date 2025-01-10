<template>
  <div v-if="cartItems.length" class="main-block">
    <OrderProgress :currentStep="2" class="order-progress" />
    <div class="delivery-form-container">
      <div class="delivery-form">
        <h1>Adres i dane kontaktowe</h1>
        <form @submit.prevent="validateForm">
          <div class="form-section">
            <div class="form-group" :class="{ 'has-error': errors.firstname }">
              <label>Imię</label>
              <input
                v-model="contactData.firstname"
                type="text"
                placeholder="Wprowadź imię"
                required
              />
              <span v-if="errors.firstname" class="error-message">
                {{ errors.firstname }}
              </span>
            </div>
            <div class="form-group" :class="{ 'has-error': errors.lastname }">
              <label>Nazwisko</label>
              <input
                v-model="contactData.lastname"
                type="text"
                placeholder="Wprowadź nazwisko"
                required
              />
              <span v-if="errors.lastname" class="error-message">
                {{ errors.lastname }}
              </span>
            </div>
          </div>

          <div class="form-section">
            <div class="form-group" :class="{ 'has-error': errors.email }">
              <label>Email</label>
              <input
                v-model="contactData.email"
                type="email"
                placeholder="Wprowadź email"
                required
              />
              <span v-if="errors.email" class="error-message">
                {{ errors.email }}
              </span>
            </div>
            <div
              class="form-group"
              :class="{ 'has-error': errors.phoneNumber }"
            >
              <label>Telefon</label>
              <input
                v-model="contactData.phoneNumber"
                type="tel"
                placeholder="Wprowadź numer telefonu"
                pattern="[0-9]{9}"
                required
              />
              <span v-if="errors.phoneNumber" class="error-message">
                {{ errors.phoneNumber }}
              </span>
            </div>
          </div>

          <div class="form-section">
            <div class="form-group" :class="{ 'has-error': errors.address }">
              <label>Ulica</label>
              <input
                v-model="contactData.address"
                type="text"
                placeholder="Wprowadź adres"
                required
              />
              <span v-if="errors.address" class="error-message">
                {{ errors.address }}
              </span>
            </div>
            <div class="form-group" :class="{ 'has-error': errors.zip }">
              <label>Kod pocztowy</label>
              <input
                v-model="contactData.zip"
                type="text"
                placeholder="00-000"
                pattern="\\d{2}-\\d{3}"
                required
              />
              <span v-if="errors.zip" class="error-message">
                {{ errors.zip }}
              </span>
            </div>
            <div class="form-group" :class="{ 'has-error': errors.city }">
              <label>Miasto</label>
              <input
                v-model="contactData.city"
                type="text"
                placeholder="Wprowadź miasto"
                required
              />
              <span v-if="errors.city" class="error-message">
                {{ errors.city }}
              </span>
            </div>
          </div>

          <h2>Wybierz metodę dostawy</h2>
          <div class="delivery-methods">
            <label>
              <input
                type="radio"
                value="poczta"
                v-model="selectedDeliveryMethod"
                @change="updateDeliveryCost"
              />
              Poczta - 15 zł
            </label>
            <label>
              <input
                type="radio"
                value="kurier"
                v-model="selectedDeliveryMethod"
                @change="updateDeliveryCost"
              />
              Kurier - 17 zł
            </label>
            <label>
              <input
                type="radio"
                value="odbior_osobisty"
                v-model="selectedDeliveryMethod"
                @change="updateDeliveryCost"
              />
              Odbiór osobisty - 0 zł
            </label>
          </div>
        </form>
      </div>

      <div class="summary">
        <h2>Podsumowanie</h2>
        <p>Wartość koszyka: {{ cartTotal }} zł</p>
        <p>Koszt dostawy: {{ deliveryCost }} zł</p>
        <p>
          <strong>Razem: {{ totalCost }} zł</strong>
        </p>
        <div class="buttons">
          <button type="button" @click="goBack" class="back-btn">Wróć</button>
          <button type="submit" @click="validateForm" class="next-btn">
            Przejdź do płatności
          </button>
        </div>
      </div>
    </div>
  </div>
  <EmptyCartMessage v-else />
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { CartItem } from "@/store/modules/cart";
import OrderProgress from "@/components/OrderProgress.vue";
import EmptyCartMessage from "@/components/EmptyCartMessage.vue";

@Component({
  components: {
    OrderProgress,
    EmptyCartMessage,
  },
})
export default class DeliveryForm extends Vue {
  isLoading = true;

  contactData = {
    firstname: "",
    lastname: "",
    email: "",
    phoneNumber: "",
    address: "",
    city: "",
    zip: "",
  };

  errors = {
    firstname: "",
    lastname: "",
    email: "",
    phoneNumber: "",
    address: "",
    city: "",
    zip: "",
  };

  selectedDeliveryMethod = "poczta";
  deliveryCost = 15;

  get cartItems(): CartItem[] {
    return this.$store.getters["cart/cartItems"] || [];
  }

  get cartTotal() {
    return this.cartItems.reduce(
      (total, item) => total + item.quantity * this.getEffectivePrice(item),
      0
    );
  }

  get totalCost() {
    return this.cartTotal + this.deliveryCost;
  }

  async mounted() {
    try {
      await this.$store.dispatch("contactData/fetchContactData");
      const contactData = this.$store.state.contactData.contactData;
      this.contactData = {
        firstname: contactData.firstname || "",
        lastname: contactData.lastname || "",
        email: contactData.email || "",
        phoneNumber: contactData.phoneNumber || "",
        address: contactData.address || "",
        city: contactData.city || "",
        zip: contactData.zip || "",
      };

      await this.$store.dispatch("deliveryMethods/fetchDeliveryMethod");
      this.selectedDeliveryMethod =
        this.$store.state.deliveryMethods.deliveryMethod || "poczta";
      this.deliveryCost = this.$store.state.deliveryMethods.deliveryCost || 15;
    } catch (error) {
      console.error("Błąd podczas ładowania danych:", error);
    } finally {
      this.isLoading = false;
    }
  }

  getEffectivePrice(item: CartItem): number {
    return item.salePrice !== 0 ? item.salePrice : item.price;
  }

  updateDeliveryCost() {
    if (this.selectedDeliveryMethod === "poczta") {
      this.deliveryCost = 15;
    } else if (this.selectedDeliveryMethod === "kurier") {
      this.deliveryCost = 17;
    } else if (this.selectedDeliveryMethod === "odbior_osobisty") {
      this.deliveryCost = 0;
    }
    this.$store.dispatch("deliveryMethods/setDeliveryMethod", {
      method: this.selectedDeliveryMethod,
      cost: this.deliveryCost,
    });
  }

  validateForm() {
    this.errors = {
      firstname: this.contactData.firstname ? "" : "Imię jest wymagane.",
      lastname: this.contactData.lastname ? "" : "Nazwisko jest wymagane.",
      email: /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.contactData.email)
        ? ""
        : "Podaj poprawny adres email.",
      phoneNumber: /^[0-9]{9}$/.test(this.contactData.phoneNumber)
        ? ""
        : "Podaj poprawny numer telefonu.",
      address: this.contactData.address ? "" : "Adres jest wymagany.",
      city: this.contactData.city ? "" : "Miasto jest wymagane.",
      zip: /^\d{2}-\d{3}$/.test(this.contactData.zip)
        ? ""
        : "Podaj poprawny kod pocztowy w formacie 00-000.",
    };

    const isValid = Object.values(this.errors).every((error) => !error);

    if (isValid) {
      this.goToPayment();
    }
  }

  async goToPayment() {
    try {
      await this.$store.dispatch(
        "contactData/updateContactData",
        this.contactData
      );

      await this.$store.dispatch("deliveryMethods/setDeliveryMethod", {
        method: this.selectedDeliveryMethod,
        cost: this.deliveryCost,
      });

      this.$router.push("/payment-method");
    } catch (error) {
      console.error("Błąd podczas zapisu danych:", error);
    }
  }

  goBack() {
    this.$router.go(-1);
  }

  goToShop() {
    this.$router.push("/");
  }
}
</script>

<style scoped>
.main-block {
  background-color: #f2f2f2;
}

.delivery-form-container {
  display: flex;
  justify-content: space-between;
  gap: 20px;
  padding: 40px 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.delivery-form {
  flex: 2;
  padding: 20px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.summary {
  flex: 1;
  padding: 20px;
  max-height: 250px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

form {
  display: flex;
  flex-direction: column;
}

.form-section {
  display: flex;
  justify-content: space-between;
  gap: 15px;
  margin-bottom: 20px;
}

.form-group {
  flex: 1;
  display: flex;
  flex-direction: column;
}

label {
  margin-bottom: 5px;
}

input {
  box-sizing: border-box;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  width: 100%;
}
.delivery-methods {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.delivery-methods label {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 16px;
  cursor: pointer;
}

.buttons {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.delivery-methods input[type="radio"] {
  margin: 0;
  width: 18px;
  height: 18px;
  accent-color: #c70a0a;
}

.back-btn {
  background-color: #666;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
}

.next-btn {
  background-color: #c70a0a;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
}

.next-btn:hover {
  background-color: #a80808;
}

.back-btn:hover {
  background-color: #4a4a4a;
}

.empty-cart-message {
  text-align: center;
  padding: 20px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.shop-btn {
  background-color: #c70a0a;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  margin-top: 20px;
}

.has-error input {
  border-color: red;
}

.error-message {
  color: red;
  font-size: 12px;
  margin-top: 5px;
}

@media (max-width: 1230px) {
  .delivery-form-container {
    flex-direction: column;
    gap: 15px;
    padding: 20px 20%;
  }

  .form-section {
    flex-direction: column;
  }

  .form-group {
    margin-bottom: 15px;
  }

  .summary {
    max-height: none;
  }
}

@media (max-width: 768px) {
  .delivery-form-container {
    flex-direction: column;
  }

  .delivery-form {
    order: 1;
    margin-bottom: 20px;
  }

  .summary {
    order: 2;
    width: 100%;
  }

  .buttons {
    flex-direction: column;
    gap: 10px;
  }

  .back-btn,
  .next-btn {
    width: 100%;
  }
}

@media (max-width: 480px) {
  .delivery-form-container {
    padding: 15px;
  }

  label {
    font-size: 14px;
  }

  input {
    padding: 8px;
    font-size: 14px;
  }

  .delivery-methods label {
    font-size: 14px;
  }

  .back-btn,
  .next-btn {
    font-size: 14px;
    padding: 8px 15px;
  }
}
</style>
