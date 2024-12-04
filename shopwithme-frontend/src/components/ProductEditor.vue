<template>
  <div>
    <h4 class="text-center text-white p-2" :class="themeClass">
      {{ editMode ? "Edytuj produkt" : "Utwórz nowy produkt" }}
    </h4>
    <h4 v-if="$v.product.$error" class="bg-danger text-white text-center p-2">
      Wszystkie pola są wymagane.
    </h4>
    <div class="form-group" v-if="editMode">
      <label>ID (Tylko do odczytu)</label>
      <input class="form-control" disabled v-model="product.id" />
    </div>
    <div class="form-group">
      <label>Nazwa</label>
      <input class="form-control" v-model.trim="product.name" />
    </div>
    <div class="form-group">
      <label>Opis</label>
      <textarea
        class="form-control"
        v-model.trim="product.description"
      ></textarea>
    </div>
    <div class="form-group">
      <label>Kategoria</label>
      <select class="form-control" v-model="product.categoryId">
        <option
          v-for="category in categories"
          :key="category.id"
          :value="category.id"
        >
          {{ category.name }}
        </option>
      </select>
    </div>
    <div class="form-group">
      <label>Cena (PLN)</label>
      <input
        type="number"
        class="form-control"
        v-model.number="product.price"
      />
    </div>
    <div class="form-group">
      <label>Obrazki (URL)</label>
      <textarea
        class="form-control"
        v-model.trim="product.images"
        placeholder="Podaj URL obrazków oddzielone przecinkami"
      ></textarea>
    </div>
    <div class="text-center">
      <router-link to="/admin/products" class="btn btn-secondary m-1">
        Anuluj
      </router-link>
      <button class="btn m-1" :class="themeClassButton" @click="handleSave">
        {{ editMode ? "Zapisz zmiany" : "Utwórz produkt" }}
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Vuelidate from "vuelidate";
import { required, minValue } from "vuelidate/lib/validators";

Vue.use(Vuelidate);

@Component
export default class ProductEditor extends Vue {
  product = {
    id: null,
    name: "",
    description: "",
    categoryId: null,
    price: 0,
    images: "",
  };

  get editMode(): boolean {
    return this.$route.params.op === "edit";
  }

  get themeClass(): string {
    return this.editMode ? "bg-info" : "bg-primary";
  }

  get themeClassButton(): string {
    return this.editMode ? "btn-info" : "btn-primary";
  }

  get categories(): Array<any> {
    return this.$store.getters["categories/allCategories"] || [];
  }

  validations() {
    return {
      product: {
        name: { required },
        description: { required },
        categoryId: { required },
        price: { required, minValue: minValue(0) },
        images: { required },
      },
    };
  }

  created() {
    this.$v.$touch();
    if (this.editMode) {
      const productId = this.$route.params.id;
      const product = this.$store.getters["products/productById"](productId);
      if (product) {
        this.product = { ...product, images: product.images.join(", ") };
      }
    }
  }

  async handleSave() {
    this.$v.$touch();
    if (!this.$v.$invalid) {
      const productToSave = {
        ...this.product,
        images: this.product.images.split(",").map((img: string) => img.trim()),
      };
      if (this.editMode) {
        await this.$store.dispatch("products/updateProduct", productToSave);
      } else {
        await this.$store.dispatch("products/createProduct", productToSave);
      }
      this.$router.push("/admin/products");
    }
  }
}
</script>

<style scoped lang="scss">
.bg-primary {
  background-color: #007bff !important;
}

.bg-info {
  background-color: #17a2b8 !important;
}

.btn-primary {
  background-color: #007bff;
  border-color: #007bff;
}

.btn-info {
  background-color: #17a2b8;
  border-color: #17a2b8;
}

.bg-danger {
  background-color: #dc3545 !important;
}

textarea {
  resize: none;
  height: 100px;
}
</style>
