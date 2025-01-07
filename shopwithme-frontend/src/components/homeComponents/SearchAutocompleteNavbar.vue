<template>
  <div class="autocomplete-navbar">
    <input
      type="text"
      v-model="localSearch"
      @input="onChange"
      @keydown.down="onArrowDown"
      @keydown.up="onArrowUp"
      @keydown.enter="onEnter"
      @focus="onFocus"
      class="form-control"
      placeholder="Wyszukaj produkt"
    />
    <ul v-if="isOpen && localSearch.length >= 3" class="autocomplete-results">
      <li v-if="isLoading" class="autocomplete-result">Ładowanie wyników...</li>
      <li
        v-for="(result, i) in items"
        :key="result.id"
        @click="selectResult(result)"
        class="autocomplete-result"
        :class="{ 'is-active': i === arrowCounter }"
      >
        <img
          v-if="result.mainImage?.url"
          :src="result.mainImage.url"
          alt="Zdjęcie produktu"
          class="result-image"
        />
        <img
          v-else
          :src="placeholderImage"
          alt="Zdjęcie zastępcze"
          class="result-image"
        />
        <div class="result-info">
          <h4>{{ result.name }}</h4>
          <p>{{ result.lead }}</p>
          <p class="price"><strong>Cena:</strong> {{ result.price }} PLN</p>
        </div>
      </li>
      <li
        v-if="!isLoading && items.length === 0"
        class="autocomplete-result no-results"
      >
        Brak wyników dla "{{ localSearch }}"
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from "vue-property-decorator";
import { storageAPI } from "@/plugins/shopAxios";

interface Product {
  id: number;
  name: string;
  lead: string;
  price: number;
  mainImage?: {
    url?: string;
    name?: string;
  };
}

@Component
export default class SearchAutocompleteNavbar extends Vue {
  @Prop({ type: String, default: "" }) search!: string;
  @Prop({ type: Array, default: () => [] }) items!: Product[];
  @Prop({ type: Boolean, default: false }) isAsync!: boolean;
  @Prop({ type: Boolean, default: false }) isLoading!: boolean;
  @Prop({ type: Boolean, default: false }) isOpen!: boolean;

  localSearch = "";
  arrowCounter = -1;
  placeholderImage = "https://placehold.co/150x150";

  mounted() {
    document.addEventListener("click", this.handleOutsideClick);
  }

  beforeDestroy() {
    document.removeEventListener("click", this.handleOutsideClick);
  }

  @Watch("search", { immediate: true })
  onSearchPropChange(newVal: string) {
    this.localSearch = newVal;
  }

  @Watch("localSearch")
  async onChange() {
    if (this.localSearch.length < 3) {
      this.$emit("update:isOpen", false);
      this.$emit("update:items", []);
      return;
    }

    this.$emit("update:isOpen", true);

    if (this.isAsync) {
      try {
        const results = await this.fetchResults();
        this.$emit("update:items", results);
      } catch (error) {
        console.error("[SearchAutocompleteNavbar] Error during search:", error);
        this.$emit("update:items", []);
      }
    }
  }

  async fetchResults() {
    try {
      const response = await this.$store.dispatch(
        "products/fetchAutocomplete",
        this.localSearch
      );

      if (!response || !Array.isArray(response)) {
        console.error("[SearchAutocompleteNavbar] Invalid API response.");
        return [];
      }

      const products = [...response];
      for (const product of products) {
        if (product.mainImage?.name) {
          try {
            const imageResponse = await storageAPI.getFile(
              product.mainImage.name
            );
            product.mainImage.url = window.URL.createObjectURL(
              new Blob([imageResponse.data])
            );
          } catch (error) {
            console.error(
              `[SearchAutocompleteNavbar] Error fetching image for product ${product.id}:`,
              error
            );
          }
        }
      }
      return products;
    } catch (error) {
      console.error(
        "[SearchAutocompleteNavbar] Error fetching results:",
        error
      );
      return [];
    }
  }

  selectResult(result: Product) {
    this.localSearch = result.name;
    this.$emit("update:search", result.name);
    this.$emit("update:isOpen", false);
    this.$emit("select", result);
  }

  handleOutsideClick(event: Event) {
    const target = event.target as HTMLElement;
    if (!this.$el.contains(target)) {
      this.$emit("update:isOpen", false);
    }
  }

  onFocus() {
    if (this.localSearch.length >= 3) {
      this.$emit("update:isOpen", true);
    }
  }

  onArrowDown() {
    if (this.arrowCounter < this.items.length - 1) {
      this.arrowCounter++;
    }
  }

  onArrowUp() {
    if (this.arrowCounter > 0) {
      this.arrowCounter--;
    }
  }

  onEnter() {
    if (this.arrowCounter >= 0 && this.arrowCounter < this.items.length) {
      this.selectResult(this.items[this.arrowCounter]);
    }
  }
}
</script>

<style scoped lang="scss">
.autocomplete-navbar {
  position: relative;
  width: 400px;

  input {
    box-sizing: border-box;
    width: 100%;
    padding: 0.75rem 1rem;
    border: none;
    border-radius: 25px;
    font-size: 1rem;
    background: linear-gradient(135deg, #ffffff, #f0f0f0);
    color: #333;
    outline: none;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }

  .autocomplete-results {
    box-sizing: border-box;
    position: absolute;
    width: 600px;
    top: calc(100% + 5px);
    right: 0;
    z-index: 10;
    background: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    max-height: 400px;
    overflow-y: auto;

    .autocomplete-result {
      display: flex;
      align-items: center;
      padding: 10px;
      gap: 20px;
      transition: background-color 0.3s;
      cursor: pointer;

      &:hover {
        background-color: #f9f9f9;
      }

      &.is-active {
        background-color: #f1f1f1;
      }

      img.result-image {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 4px;
      }

      .result-info {
        flex: 1;

        h4 {
          font-size: 1rem;
          margin: 0 0 5px 0;
          color: #333;
        }

        p {
          margin: 0;
          font-size: 0.875rem;
          color: #666;
        }

        .price {
          margin-top: 20px;
        }
      }
    }

    .no-results {
      text-align: center;
      padding: 10px;
      color: #ff0000;
      font-size: 0.9rem;
    }
  }

  @media (max-width: 810px) {
    .autocomplete-results {
      width: 100%;
      max-height: 300px;

      .autocomplete-result {
        padding: 8px;
        gap: 10px;

        img.result-image {
          width: 50px;
          height: 50px;
        }

        .result-info {
          h4 {
            font-size: 0.95rem;
          }

          p {
            font-size: 0.85rem;
          }
        }
      }
    }
  }

  @media (max-width: 520px) {
    .autocomplete-results {
      left: 0;
      width: 120%;
    }
  }
}
</style>
