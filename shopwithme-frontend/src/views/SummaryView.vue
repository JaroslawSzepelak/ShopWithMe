<template>
  <div>
    <!-- Modal o nieprawidłowych danych kontaktowych -->
    <div v-if="showModal" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>Dane kontaktowe wygasły</h2>
        <p>
          Twoje dane kontaktowe są niekompletne lub wygasły. Proszę je uzupełnić
          ponownie.
        </p>
        <button @click="goToContactForm">Wróć do edycji danych</button>
      </div>
    </div>

    <!-- Modal o potwierdzeniu zamówienia -->
    <div v-if="orderModalVisible" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>Zamówienie zostało złożone!</h2>
        <p>
          Dziękujemy za złożenie zamówienia. Numer Twojego zamówienia to:
          <strong>{{ orderId }}</strong
          >.
        </p>
        <p>Otrzymasz szczegóły zamówienia na podany adres e-mail.</p>
        <button @click="goToHomePage">Powrót do strony głównej</button>
      </div>
    </div>

    <div v-if="cartItems.length" class="summary-container">
      <OrderProgress :currentStep="4" class="order-progress" />
      <div class="summary-content">
        <!-- Zawartość koszyka -->
        <div class="cart-section">
          <h1>Zawartość koszyka</h1>
          <div
            v-for="item in cartItems"
            :key="item.productId"
            class="cart-item"
          >
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
          <button class="change-btn" @click="editCart">Zmień koszyk</button>
        </div>

        <!-- Pozostałe sekcje -->
        <div class="details-section">
          <div class="details-block">
            <h2>Adres i dane kontaktowe</h2>
            <div class="address-details">
              <p>
                <strong>Imię i nazwisko:</strong> {{ contactData.firstname }}
                {{ contactData.lastname }}
              </p>
              <p>
                <strong>Adres:</strong> {{ contactData.address }} -
                {{ contactData.city }}, {{ contactData.zip }}
              </p>
              <p><strong>Telefon:</strong> {{ contactData.phoneNumber }}</p>
              <p><strong>Email:</strong> {{ contactData.email }}</p>
              <p>
                <strong>Metoda dostawy:</strong>
                {{ selectedDeliveryMethodLabel }} - {{ deliveryCost }} zł
              </p>
            </div>
            <button class="change-btn" @click="editAddress">Zmień</button>
          </div>

          <div class="details-block">
            <h2>Metoda płatności</h2>
            <p>
              <template v-if="selectedPaymentMethod === 'card'">
                Płatność kartą (**** **** **** {{ cardNumber.slice(-4) }} )
              </template>
              <template v-else-if="selectedPaymentMethod === 'bank_transfer'">
                Przelew internetowy (Numer konta: {{ accountNumber }} )
              </template>
              <template v-else> Za pobraniem </template>
            </p>
            <button class="change-btn" @click="editPaymentMethod">Zmień</button>
          </div>
        </div>

        <!-- Podsumowanie -->
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

    <EmptyCartMessage v-else-if="!isLoading" />
  </div>
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
export default class SummaryView extends Vue {
  placeholderImage = "https://placehold.co/100x100";
  isLoading = true;
  showModal = false;
  orderModalVisible = false;
  orderId: string | null = null;

  async mounted() {
    await this.loadData();
    this.checkContactData();
  }

  async loadData() {
    try {
      await this.$store.dispatch("contactData/fetchContactData");
      await this.$store.dispatch("deliveryMethods/fetchDeliveryMethod");
    } catch (error) {
      console.error("Błąd podczas ładowania danych:", error);
    } finally {
      this.isLoading = false;
    }
  }

  get contactData(): Record<string, string | null> {
    return this.$store.state.contactData.contactData || {};
  }

  get selectedDeliveryMethodLabel(): string {
    const methodMap: Record<string, string> = {
      poczta: "Poczta",
      kurier: "Kurier",
      odbior_osobisty: "Odbiór osobisty",
    };
    return methodMap[this.$store.state.deliveryMethods.deliveryMethod] || "";
  }

  get deliveryCost(): number {
    return this.$store.state.deliveryMethods.deliveryCost || 0;
  }

  get selectedPaymentMethod(): string {
    return this.$store.state.paymentMethod.selectedMethod || "";
  }

  get cardNumber(): string {
    return this.$store.state.paymentMethod.cardNumber || "";
  }

  get accountNumber(): string {
    return this.$store.state.paymentMethod.accountNumber || "";
  }

  get cartItems(): CartItem[] {
    return this.$store.getters["cart/cartItems"] || [];
  }

  get cartTotal(): number {
    return this.$store.getters["cart/cartTotal"] || 0;
  }

  get totalCost(): number {
    return (
      this.cartTotal +
      this.deliveryCost +
      (this.selectedPaymentMethod === "cash_on_delivery" ? 5 : 0)
    );
  }

  checkContactData(): void {
    const { firstname, lastname, email, phoneNumber, address, city, zip } =
      this.contactData;

    if (
      !firstname ||
      !lastname ||
      !email ||
      !phoneNumber ||
      !address ||
      !city ||
      !zip
    ) {
      this.showModal = true;
    }
  }

  async confirmOrder(): Promise<void> {
    try {
      await this.$store.dispatch("order/createOrder");
      this.orderId = this.$store.getters["order/orderSummary"]?.id;

      if (!this.orderId) {
        throw new Error("Brak ID zamówienia w odpowiedzi API.");
      }

      await this.$store.dispatch("cart/fetchCart", []);
      this.orderModalVisible = true;
    } catch (error) {
      console.error("Błąd podczas składania zamówienia:", error);
      alert("Nie udało się złożyć zamówienia. Spróbuj ponownie później.");
    }
  }

  closeModal(): void {
    this.showModal = false;
    this.orderModalVisible = false;
  }

  goToContactForm(): void {
    this.$router.push("/delivery-form");
  }

  editAddress(): void {
    this.$router.push("/delivery-form");
  }

  editPaymentMethod(): void {
    this.$router.push("/payment-method");
  }

  editCart(): void {
    this.$router.push("/cart");
  }

  goToHomePage(): void {
    this.orderModalVisible = false;
    this.$router.push("/");
  }
}
</script>

<style scoped lang="scss">
.summary-container {
  display: flex;
  flex-direction: column;
  background-color: #f9f9f9;
  padding: 40px;
  gap: 20px;
}

.summary-content {
  display: flex;
  justify-content: space-between;
  gap: 20px;
  flex-wrap: wrap;
}

.cart-section {
  flex: 2;
  background: #fff;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 100%;

  h1 {
    margin-bottom: 20px;
    font-size: 1.5rem;
  }

  .cart-item {
    display: flex;
    align-items: center;
    gap: 15px;
    margin-bottom: 15px;
    flex-wrap: wrap;

    img {
      width: 80px;
      height: 80px;
      object-fit: cover;
      border-radius: 8px;
    }

    .item-details {
      font-size: 0.9rem;
    }
  }
}

.details-section {
  flex: 3;
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 100%;

  .details-block {
    background: #fff;
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);

    h2 {
      margin-bottom: 10px;
      font-size: 1.2rem;
    }

    p {
      font-size: 0.9rem;
    }
  }
}

.summary-total {
  display: flex;
  flex-direction: column;
  flex: 1;
  background-color: #fff;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 20px;
  max-width: 100%;

  h2 {
    margin-bottom: 20px;
    font-size: 1.5rem;
  }

  p {
    font-size: 1rem;
  }

  .confirm-btn {
    box-sizing: border-box;
    background-color: #c70a0a;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    margin-top: 20px;
    width: 100%;
    font-size: 1rem;

    &:hover {
      background-color: #a50e0e;
    }
  }
}

.change-btn {
  background: none;
  border: none;
  color: #c70a0a;
  font-size: 0.9rem;
  cursor: pointer;

  &:hover {
    color: #a50e0e;
  }
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background-color: #fff;
  padding: 2rem;
  border-radius: 8px;
  text-align: center;
  max-width: 300px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);

  p {
    font-size: 1.2rem;
    color: #333;
    margin-bottom: 1rem;
  }

  button {
    background-color: #c70a0a;
    color: #fff;
    border: none;
    padding: 0.5rem 1rem;
    font-size: 1rem;
    border-radius: 4px;
    cursor: pointer;

    &:hover {
      background-color: #a50e0e;
    }
  }
}

@media (max-width: 1024px) {
  .summary-content {
    flex-wrap: wrap;
    gap: 15px;
  }

  .cart-section,
  .details-section,
  .summary-total {
    flex: 1 1 100%;
    max-width: 100%;

    .confirm-btn {
      width: 70%;
      align-self: center;
    }
  }

  .cart-item {
    img {
      width: 70px;
      height: 70px;
    }

    .item-details {
      font-size: 0.8rem;
    }
  }

  h1 {
    font-size: 1.2rem;
  }

  h2 {
    font-size: 1.3rem;
  }

  p {
    font-size: 0.9rem;
  }
}

@media (max-width: 768px) {
  .summary-container {
    padding: 20px;
  }

  .cart-item {
    gap: 10px;

    img {
      width: 60px;
      height: 60px;
    }
  }

  .confirm-btn {
    font-size: 0.9rem;
    padding: 8px 16px;
  }
}

@media (max-width: 480px) {
  .summary-container {
    padding: 10px;
  }

  .cart-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;

    img {
      width: 50px;
      height: 50px;
    }

    .item-details {
      font-size: 0.8rem;
    }
  }

  .details-block {
    p {
      font-size: 0.8rem;
    }
  }

  h1,
  h2 {
    font-size: 1rem;
  }

  .confirm-btn {
    font-size: 0.8rem;
    padding: 6px 12px;
  }
}
</style>
