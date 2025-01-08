<template>
  <div class="set-promotion">
    <h1>Ustaw promocję dla produktu</h1>
    <form @submit.prevent="submitPromotion">
      <div class="form-group">
        <label class="toggle-switch">
          <div class="slider-container">
            <input type="checkbox" v-model="promotion.isSaleOn" />
            <span class="slider"></span>
            <span class="label">
              {{ promotion.isSaleOn ? "Włączona" : "Wyłączona" }}
            </span>
          </div>
        </label>
      </div>

      <div class="form-group">
        <label for="salePrice">Cena promocyjna:</label>
        <input
          id="salePrice"
          type="number"
          v-model.number="promotion.salePrice"
          placeholder="Wprowadź cenę promocyjną"
          :class="{ 'input-error': errors.salePrice }"
        />
        <p v-if="errors.salePrice" class="error-message">
          {{ errors.salePrice }}
        </p>
      </div>

      <div class="form-group">
        <label for="dateSaleFrom">Data rozpoczęcia promocji:</label>
        <input
          id="dateSaleFrom"
          type="date"
          v-model="promotion.dateSaleFrom"
          :class="{ 'input-error': errors.dateSaleFrom }"
        />
        <p v-if="errors.dateSaleFrom" class="error-message">
          {{ errors.dateSaleFrom }}
        </p>
      </div>

      <div class="form-group">
        <label for="dateSaleTo">Data zakończenia promocji:</label>
        <input
          id="dateSaleTo"
          type="date"
          v-model="promotion.dateSaleTo"
          :class="{ 'input-error': errors.dateSaleTo }"
        />
        <p v-if="errors.dateSaleTo" class="error-message">
          {{ errors.dateSaleTo }}
        </p>
      </div>

      <div class="form-actions">
        <button type="submit" class="btn btn-primary">Zapisz</button>
        <button type="button" @click="cancel" class="btn btn-secondary">
          Anuluj
        </button>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class SetPromotion extends Vue {
  promotion = {
    isSaleOn: false,
    salePrice: null,
    dateSaleFrom: "",
    dateSaleTo: "",
  };

  errors: {
    salePrice: string;
    dateSaleFrom: string;
    dateSaleTo: string;
  } = {
    salePrice: "",
    dateSaleFrom: "",
    dateSaleTo: "",
  };

  async mounted() {
    try {
      await this.$store.dispatch(
        "admin/adminProducts/fetchProduct",
        this.$route.params.id
      );
      const product =
        this.$store.getters["admin/adminProducts/selectedProduct"];

      const formatDate = (date: string) =>
        date ? new Date(date).toISOString().split("T")[0] : "";

      this.promotion.isSaleOn = product.isSaleOn || false;
      this.promotion.salePrice = product.salePrice || null;
      this.promotion.dateSaleFrom = formatDate(product.dateSaleFrom);
      this.promotion.dateSaleTo = formatDate(product.dateSaleTo);
    } catch (error) {
      console.error("Błąd podczas pobierania danych produktu:", error);
    }
  }

  resetErrors(): void {
    this.errors = {
      salePrice: "",
      dateSaleFrom: "",
      dateSaleTo: "",
    };
  }

  validatePromotion(): boolean {
    this.resetErrors();

    if (this.promotion.isSaleOn) {
      if (!this.promotion.salePrice) {
        this.errors.salePrice = "Cena promocyjna jest wymagana.";
      }
      if (this.promotion.dateSaleFrom && this.promotion.dateSaleTo) {
        const from = new Date(this.promotion.dateSaleFrom).getTime();
        const to = new Date(this.promotion.dateSaleTo).getTime();
        if (from > to) {
          this.errors.dateSaleFrom =
            "Data rozpoczęcia nie może być późniejsza niż zakończenia.";
        }
      }
    }

    return !Object.values(this.errors).some((error) => error);
  }

  async submitPromotion() {
    if (!this.validatePromotion()) {
      return;
    }

    try {
      const product = {
        ...this.$store.getters["admin/adminProducts/selectedProduct"],
        salePrice: this.promotion.salePrice,
        dateSaleFrom: this.promotion.dateSaleFrom
          ? new Date(this.promotion.dateSaleFrom).toISOString()
          : null,
        dateSaleTo: this.promotion.dateSaleTo
          ? new Date(this.promotion.dateSaleTo).toISOString()
          : null,
        isSaleOn: this.promotion.isSaleOn,
      };

      await this.$store.dispatch("admin/adminProducts/updateProduct", product);

      this.$router.push("/admin/products");
    } catch (error) {
      console.error("Błąd podczas zapisywania promocji:", error);
    }
  }

  cancel() {
    this.$router.push("/admin/products");
  }
}
</script>

<style scoped lang="scss">
.set-promotion {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  color: #333;

  h1 {
    text-align: center;
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 15px;

    label {
      font-weight: bold;
      display: block;
      margin-bottom: 5px;
    }

    input {
      box-sizing: border-box;
      width: 100%;
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 5px;
      font-size: 1rem;
    }

    .input-error {
      border-color: #c70a0a;
    }

    .error-message {
      color: #c70a0a;
      font-size: 0.9rem;
      margin-top: 5px;
    }
  }

  .toggle-switch {
    display: flex;
    margin-bottom: 20px;

    input {
      opacity: 0;
      width: 0;
      height: 0;
    }

    .slider-container {
      display: flex;
      justify-content: flex-start;
      align-items: center;
      margin: 20px;

      .slider {
        box-sizing: border-box;
        position: relative;
        width: 80px;
        height: 40px;
        background-color: #ccc;
        border-radius: 25px;
        transition: background-color 0.3s;
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 5px;
      }

      input:checked + .slider {
        background-color: #c70a0a;
      }

      input:checked + .slider .label {
        color: white;
      }

      .slider:before {
        content: "";
        position: absolute;
        width: 25px;
        height: 25px;
        background-color: white;
        border-radius: 50%;
        top: 6px;
        left: 6px;
        transition: transform 0.3s;
      }

      input:checked + .slider:before {
        transform: translateX(45px);
      }

      .label {
        margin-left: 10px;
        font-size: 1.2rem;
        color: #333;
        user-select: none;
      }
    }
  }

  .form-actions {
    display: flex;
    justify-content: space-between;

    .btn {
      padding: 10px 20px;
      border-radius: 5px;
      font-size: 1rem;
      cursor: pointer;
    }

    .btn-primary {
      background-color: #c70a0a;
      color: white;
      border: none;

      &:hover {
        background-color: #a80808;
      }
    }

    .btn-secondary {
      background-color: #666;
      color: white;
      border: none;

      &:hover {
        background-color: #4a4a4a;
      }
    }
  }
}
</style>
