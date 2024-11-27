<template>
  <div class="pagination">
    <div class="page-controls">
      <button
        :disabled="currentPage === 1"
        @click="goToPage(currentPage - 1)"
        class="btn btn-secondary"
      >
        Poprzednia
      </button>
      <span v-if="currentPage > 4">
        <button @click="goToPage(1)" class="btn btn-secondary">1</button>
        <span class="dots">...</span>
      </span>
      <span>
        <button
          v-for="i in visiblePages"
          :key="i"
          class="btn"
          :class="{
            'btn-primary': i === currentPage,
            'btn-secondary': i !== currentPage,
          }"
          @click="goToPage(i)"
        >
          {{ i }}
        </button>
      </span>
      <span v-if="currentPage <= pageCount - 4">
        <span class="dots">...</span>
        <button @click="goToPage(pageCount)" class="btn btn-secondary">
          {{ pageCount }}
        </button>
      </span>
      <button
        :disabled="currentPage === pageCount"
        @click="goToPage(currentPage + 1)"
        class="btn btn-secondary"
      >
        Następna
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component
export default class Pagination extends Vue {
  @Prop({ required: true }) currentPage!: number;
  @Prop({ required: true }) pageCount!: number;

  get visiblePages(): number[] {
    if (this.pageCount <= 5) {
      return Array.from({ length: this.pageCount }, (_, i) => i + 1);
    } else if (this.currentPage <= 3) {
      return [1, 2, 3, 4, 5];
    } else if (this.currentPage > this.pageCount - 3) {
      return Array.from({ length: 5 }, (_, i) => this.pageCount - 4 + i);
    } else {
      return [
        this.currentPage - 2,
        this.currentPage - 1,
        this.currentPage,
        this.currentPage + 1,
        this.currentPage + 2,
      ];
    }
  }

  goToPage(page: number) {
    if (page !== this.currentPage) {
      console.log("Emitting page change:", page);
      this.$emit("changePage", page);
    }
  }
}
</script>

<style scoped lang="scss">
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 15px;
  margin-top: 20px;

  .page-controls {
    display: flex;
    gap: 10px;

    .btn {
      padding: 10px 15px;
      border: 1px solid #ccc;
      background-color: #f9f9f9;
      border-radius: 5px;
      cursor: pointer;

      &:hover {
        background-color: #e9e9e9;
      }

      &.btn-primary {
        background-color: #333;
        color: #fff;
        font-weight: bold;
      }

      &:disabled {
        background-color: #ddd;
        cursor: not-allowed;
      }
    }

    .dots {
      margin: 0 10px;
      font-size: 1.2rem;
    }
  }
}
</style>
