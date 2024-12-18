<template>
  <div class="autocomplete">
    <input
      type="text"
      v-model="search"
      @input="onChange"
      @keydown.down="onArrowDown"
      @keydown.up="onArrowUp"
      @keydown.enter="onEnter"
      class="form-control"
      placeholder="Wpisz nazwę produktu"
    />
    <ul v-if="isOpen" class="autocomplete-results">
      <li v-if="isLoading" class="autocomplete-result">Ładowanie wyników...</li>
      <li
        v-for="(result, i) in results"
        :key="result.id"
        @click="setResult(result)"
        class="autocomplete-result"
        :class="{ 'is-active': i === arrowCounter }"
      >
        {{ result.name }}
      </li>
      <li
        v-if="!isLoading && results.length === 0"
        class="autocomplete-result text-danger"
      >
        Brak wyników dla "{{ search }}"
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from "vue-property-decorator";

interface Product {
  id: number;
  name: string;
}

@Component
export default class SearchAutocomplete extends Vue {
  @Prop({ type: Boolean, default: false }) isAsync!: boolean;
  @Prop({ type: Array, default: () => [] }) items!: Product[];
  @Prop({ type: Boolean, default: false }) isLoading!: boolean;

  search = "";
  results: Product[] = [];
  isOpen = false;
  arrowCounter = -1;

  @Watch("items")
  onItemsChange(newItems: Product[]) {
    this.results = newItems;
  }

  onChange() {
    console.log("Zapytanie wyszukiwania:", this.search);

    if (this.search.length < 3) {
      this.isOpen = false;
      this.results = [];
      return;
    }

    this.isOpen = true;

    if (this.isAsync) {
      this.$emit("search", this.search);
    } else {
      this.filterResults();
    }
  }

  filterResults() {
    this.results = this.items.filter((item) =>
      item.name.toLowerCase().includes(this.search.toLowerCase())
    );
  }

  setResult(result: Product) {
    console.log("Wybrany produkt:", result);
    this.search = result.name;
    this.isOpen = false;
    this.$emit("select", result);
  }

  onArrowDown() {
    if (this.arrowCounter < this.results.length - 1) {
      this.arrowCounter++;
    }
  }

  onArrowUp() {
    if (this.arrowCounter > 0) {
      this.arrowCounter--;
    }
  }

  onEnter() {
    if (this.arrowCounter >= 0 && this.arrowCounter < this.results.length) {
      this.setResult(this.results[this.arrowCounter]);
    }
  }
}
</script>

<style scoped lang="scss">
.autocomplete {
  position: relative;
  width: 100%;

  .form-control {
    width: 100%;
    max-width: 400px;
  }

  .autocomplete-results {
    position: absolute;
    z-index: 10;
    background: white;
    border: 1px solid #ccc;
    border-radius: 4px;
    max-height: 200px;
    overflow-y: auto;
    padding: 0;
    margin: 0;
    width: 100%;
    list-style: none;

    .autocomplete-result {
      padding: 8px 12px;
      cursor: pointer;
      text-align: left;
      color: #333;

      &.is-active,
      &:hover {
        background-color: #d3d3d3;
        color: #000;
      }

      &.text-danger {
        color: red;
      }
    }
  }
}
</style>
