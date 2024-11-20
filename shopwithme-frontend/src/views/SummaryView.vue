<template>
  <div class="summary-view">
    <div class="order-summary-container">
      <h1>Podsumowanie zamówienia</h1>

      <div class="summary-section">
        <h2>Adres i dane kontaktowe</h2>
        <div class="address-details">
          <p>
            <strong>Imię i nazwisko:</strong> {{ contactData.firstname }}
            {{ contactData.lastname }}
          </p>
          <p>
            <strong>Adres:</strong> {{ contactData.address }},
            {{ contactData.city }}, {{ contactData.zip }}
          </p>
          <p><strong>Telefon:</strong> {{ contactData.phoneNumber }}</p>
          <p><strong>Email:</strong> {{ contactData.email }}</p>
        </div>
        <button class="change-btn" @click="editAddress">Zmień</button>
      </div>

      <div class="summary-section">
        <h2>Metoda dostawy</h2>
        <p>{{ selectedDeliveryMethodLabel }} - {{ deliveryCost }} zł</p>
        <button class="change-btn" @click="editDeliveryMethod">Zmień</button>
      </div>

      <div class="summary-section">
        <h2>Metoda płatności</h2>
        <p>
          <template v-if="selectedPaymentMethod === 'card'">
            Płatność kartą (**** **** **** {{ cardNumber.slice(-4) }})
          </template>
          <template v-else-if="selectedPaymentMethod === 'bank_transfer'">
            Przelew internetowy (Numer konta: {{ accountNumber }})
          </template>
          <template v-else> Za pobraniem </template>
        </p>
        <button class="change-btn" @click="editPaymentMethod">Zmień</button>
      </div>

      <div class="summary-section">
        <h2>Zawartość koszyka</h2>
        <div v-for="item in cartItems" :key="item.productId" class="cart-item">
          <img :src="item.image || placeholderImage" alt="Zdjęcie produktu" />
          <div class="item-details">
            <p>
              <strong>{{ item.name }}</strong>
            </p>
            <p>
              {{ item.quantity }} szt. - {{ item.price * item.quantity }} zł
            </p>
          </div>
        </div>
        <button class="change-btn" @click="editCart">Zmień</button>
      </div>

      <div class="summary-total">
        <h2>Podsumowanie</h2>
        <p><strong>Wartość produktów:</strong> {{ cartTotal }} zł</p>
        <p><strong>Koszt dostawy:</strong> {{ deliveryCost }} zł</p>
        <p><strong>Razem:</strong> {{ totalCost }} zł</p>
        <button class="confirm-btn" @click="confirmOrder">
          Kupuję i płacę
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      placeholderImage: "https://placehold.co/100x100",
    };
  },
  computed: {
    contactData() {
      return this.$store.state.contactData.contactData || {};
    },
    selectedDeliveryMethodLabel() {
      const methodMap = {
        poczta: "Poczta",
        kurier: "Kurier",
        odbior_osobisty: "Odbiór osobisty",
      };
      return methodMap[this.$store.state.deliveryMethods.deliveryMethod];
    },
    deliveryCost() {
      return this.$store.state.deliveryMethods.deliveryCost || 0;
    },
    selectedPaymentMethod() {
      return this.$store.state.paymentMethod.selectedMethod;
    },
    cardNumber() {
      return this.$store.state.paymentMethod.cardNumber || "";
    },
    accountNumber() {
      return this.$store.state.paymentMethod.accountNumber || "";
    },
    cartItems() {
      return this.$store.state.cart.cartItems || [];
    },
    cartTotal() {
      return this.$store.getters["cart/cartTotal"] || 0;
    },
    totalCost() {
      return (
        this.cartTotal +
        this.deliveryCost +
        (this.selectedPaymentMethod === "cash_on_delivery" ? 5 : 0)
      );
    },
  },
  methods: {
    editAddress() {
      this.$router.push("/delivery-form");
    },
    editDeliveryMethod() {
      this.$router.push("/delivery-form");
    },
    editPaymentMethod() {
      this.$router.push("/payment-method");
    },
    editCart() {
      this.$router.push("/cart");
    },
    confirmOrder() {
      // Implementacja zatwierdzenia zamówienia
      alert("Zamówienie zostało zatwierdzone!");
    },
  },
};
</script>

<style scoped lang="scss">
.summary-view {
  background-color: #f2f2f2;
  padding: 40px;
  display: flex;
  justify-content: center;
  min-height: 100vh;
}

.order-summary-container {
  background-color: #fff;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  max-width: 900px;
  width: 100%;
}

h1,
h2 {
  font-size: 1.8rem;
  margin-bottom: 15px;
}

.summary-section {
  margin-bottom: 20px;

  .address-details,
  .cart-item {
    margin-bottom: 10px;
  }

  .cart-item {
    display: flex;
    align-items: center;
    gap: 10px;

    img {
      width: 80px;
      height: 80px;
      object-fit: cover;
      border-radius: 5px;
    }

    .item-details {
      font-size: 1rem;
    }
  }
}

.summary-total {
  background-color: #f9f9f9;
  padding: 15px;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);

  p {
    margin: 5px 0;
  }

  .confirm-btn {
    background-color: #c70a0a;
    color: #fff;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    font-size: 1rem;
    cursor: pointer;
    margin-top: 10px;
  }
}

.change-btn {
  background-color: #666;
  color: #fff;
  border: none;
  padding: 5px 10px;
  border-radius: 5px;
  font-size: 0.9rem;
  cursor: pointer;
  margin-top: 10px;
}
</style>
