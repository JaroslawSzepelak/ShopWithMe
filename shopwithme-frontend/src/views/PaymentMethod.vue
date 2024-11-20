<template>
  <div class="payment-method-container">
    <div class="payment-method">
      <h1>Wybierz metodę płatności</h1>
      <div class="method-container">
        <div class="radio-group">
          <label>
            <input
              type="radio"
              value="card"
              v-model="selectedMethod"
              @change="handleMethodChange"
            />
            <span class="custom-radio"></span>
            Płatność kartą
          </label>
          <label>
            <input
              type="radio"
              value="bank_transfer"
              v-model="selectedMethod"
              @change="handleMethodChange"
            />
            <span class="custom-radio"></span>
            Przelew internetowy
          </label>
          <label>
            <input
              type="radio"
              value="cash_on_delivery"
              v-model="selectedMethod"
              @change="handleMethodChange"
            />
            <span class="custom-radio"></span>
            Za pobraniem
          </label>
        </div>

        <!-- Dynamiczne wyświetlanie dodatkowych pól -->
        <div class="details" v-if="selectedMethod">
          <div v-if="selectedMethod === 'card'">
            <h2>Podaj dane karty:</h2>
            <label for="cardNumber">Numer karty:</label>
            <input
              id="cardNumber"
              type="text"
              placeholder="1234 5678 9101 1121"
              v-model="cardNumber"
              @input="filterNumericInput('cardNumber')"
            />
          </div>
          <div v-if="selectedMethod === 'bank_transfer'">
            <h2>Podaj numer konta bankowego:</h2>
            <label for="accountNumber">Numer konta:</label>
            <input
              id="accountNumber"
              type="text"
              placeholder="12345678901234567890123456"
              v-model="accountNumber"
              @input="filterNumericInput('accountNumber')"
            />
          </div>
          <div v-if="selectedMethod === 'cash_on_delivery'">
            <h2>Informacja:</h2>
            <p>Płatność za pobraniem kosztuje dodatkowo 5 zł.</p>
          </div>
        </div>

        <div class="summary">
          <h2>Podsumowanie</h2>
          <p>Wartość koszyka: {{ cartTotal }} zł</p>
          <p>Koszt dostawy: {{ deliveryCost }} zł</p>
          <p>Opłata dodatkowa za pobraniem: {{ extraCost }} zł</p>
          <p>
            <strong>Razem: {{ totalCost }} zł</strong>
          </p>
        </div>

        <div class="buttons">
          <button class="back-btn" @click="goBack">Wróć</button>
          <button class="confirm-btn" @click="confirmPaymentMethod">
            Potwierdź
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      selectedMethod: null,
      cardNumber: "",
      accountNumber: "",
      extraCost: 0,
    };
  },
  computed: {
    cartTotal() {
      return this.$store.getters["cart/cartTotal"] || 0;
    },
    deliveryCost() {
      return this.$store.getters["deliveryMethods/deliveryCost"] || 0;
    },
    totalCost() {
      return this.cartTotal + this.deliveryCost + this.extraCost;
    },
  },
  methods: {
    handleMethodChange() {
      this.cardNumber = "";
      this.accountNumber = "";
      this.extraCost = this.selectedMethod === "cash_on_delivery" ? 5 : 0;
    },
    filterNumericInput(field) {
      this[field] = this[field].replace(/\D/g, ""); // Usuwa wszystkie znaki inne niż cyfry
    },
    goBack() {
      this.$router.go(-1);
    },
    confirmPaymentMethod() {
      if (this.selectedMethod === "card" && !this.cardNumber) {
        alert("Wprowadź numer karty!");
        return;
      }
      if (this.selectedMethod === "bank_transfer" && !this.accountNumber) {
        alert("Wprowadź numer konta!");
        return;
      }

      // Zapisanie metody płatności do Vuex
      this.$store.dispatch("paymentMethod/setPaymentMethod", {
        method: this.selectedMethod,
        cardNumber: this.cardNumber,
        accountNumber: this.accountNumber,
      });

      // Nawigacja do podsumowania
      this.$router.push("/summary");
    },
  },
};
</script>

<style scoped lang="scss">
.payment-method-container {
  display: flex;
  justify-content: center;
  background-color: #f2f2f2;
  padding: 40px;
  padding-bottom: 230px;
}

.payment-method {
  background-color: #ffffff;
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 600px;

  h1 {
    font-size: 2rem;
    margin-bottom: 20px;
    text-align: center;
  }

  .radio-group {
    display: flex;
    flex-direction: column;
    gap: 15px;

    label {
      display: flex;
      align-items: center;
      font-size: 1.1rem;
      cursor: pointer;

      input {
        position: absolute;
        opacity: 0;
      }

      .custom-radio {
        width: 20px;
        height: 20px;
        border: 2px solid #c70a0a;
        border-radius: 50%;
        display: inline-block;
        margin-right: 10px;
        position: relative;
      }

      input:checked + .custom-radio {
        background-color: #c70a0a;
      }
    }
  }

  .details {
    margin-top: 20px;
    padding: 15px;
    background-color: #f2f2f2;
    border-radius: 8px;

    h2 {
      font-size: 1.2rem;
      margin-bottom: 10px;
    }

    label {
      display: block;
      margin-bottom: 5px;
      font-size: 0.9rem;
    }

    input {
      width: 100%;
      padding: 10px;
      font-size: 1rem;
      border: 1px solid #ccc;
      border-radius: 5px;
      color: #333;
      ::placeholder {
        color: #dddcdc;
      }
    }

    p {
      font-size: 0.9rem;
      color: #555;
      margin: 0;
    }
  }

  .summary {
    margin-top: 20px;
    padding: 15px;
    background-color: #fff;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);

    h2 {
      font-size: 1.2rem;
      margin-bottom: 10px;
    }

    p {
      margin: 5px 0;
    }

    strong {
      font-size: 1.2rem;
      color: #c70a0a;
    }
  }

  .buttons {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;

    .back-btn {
      background-color: #666;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
    }

    .confirm-btn {
      background-color: #c70a0a;
      color: white;
      border: none;
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
    }
  }
}
</style>
