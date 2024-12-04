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

      <span v-for="page in visiblePages" :key="page">
        <button
          v-if="page !== -1"
          class="btn"
          :class="{
            'btn-primary': page === currentPage,
            'btn-secondary': page !== currentPage,
          }"
          @click="goToPage(page)"
        >
          {{ page }}
        </button>
        <span v-else class="dots">...</span>
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
    const maxVisible = 5;
    const pages: number[] = [];

    pages.push(1);

    if (this.pageCount <= maxVisible) {
      for (let i = 2; i <= this.pageCount; i++) {
        pages.push(i);
      }
    } else {
      const minPage = Math.max(2, this.currentPage - 2);
      const maxPage = Math.min(this.pageCount - 1, this.currentPage + 2);

      if (minPage > 2) pages.push(-1);

      for (let i = minPage; i <= maxPage; i++) {
        pages.push(i);
      }

      if (maxPage < this.pageCount - 1) pages.push(-1);

      pages.push(this.pageCount);
    }

    return pages;
  }

  goToPage(page: number) {
    if (page > 0 && page !== this.currentPage) {
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
  margin: 20px 0;

  .page-controls {
    display: flex;
    gap: 10px;

    .btn {
      padding: 10px 15px;
      background-color: #555;
      color: #fff;
      border-radius: 5px;
      cursor: pointer;
      font-weight: bold;

      &:hover {
        background-color: #333;
      }

      &.btn-primary {
        background-color: #111;
        border-color: #000;
      }

      &:disabled {
        background-color: #888;
        cursor: not-allowed;
      }
    }

    .dots {
      padding: 10px 15px;
      color: #777;
      font-weight: bold;
      cursor: default;
    }
  }
}
</style>
