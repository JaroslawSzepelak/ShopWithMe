<template>
  <div class="pagination">
    <div class="page-controls">
      <button
        :disabled="pageIndex === 1"
        @click="changePage(pageIndex - 1)"
        class="btn btn-secondary"
      >
        Poprzednia
      </button>

      <span v-for="page in visiblePages" :key="page">
        <button
          v-if="page !== -1"
          class="btn"
          :class="{
            'btn-primary': page === pageIndex,
            'btn-secondary': page !== pageIndex,
          }"
          @click="changePage(page)"
        >
          {{ page }}
        </button>
        <span v-else class="dots">...</span>
      </span>

      <button
        :disabled="pageIndex === totalPages"
        @click="changePage(pageIndex + 1)"
        class="btn btn-secondary"
      >
        Następna
      </button>
    </div>
    <div class="page-size-selector">
      <label for="pageSize">Ilość produktów na stronę:</label>
      <select
        id="pageSize"
        class="page-size-select"
        :value="pageSize"
        @change="updatePageSize($event.target.value)"
      >
        <option value="10">10 na stronę</option>
        <option value="20">20 na stronę</option>
        <option value="30">30 na stronę</option>
      </select>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component
export default class ProductPagination extends Vue {
  @Prop({ required: true }) pageIndex!: number;
  @Prop({ required: true }) pageSize!: number;
  @Prop({ required: true }) totalPages!: number;

  get visiblePages(): number[] {
    const maxVisible = 5;
    const pages: number[] = [];

    pages.push(1);

    if (this.totalPages <= maxVisible) {
      for (let i = 2; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      const minPage = Math.max(2, this.pageIndex - 2);
      const maxPage = Math.min(this.totalPages - 1, this.pageIndex + 2);

      if (minPage > 2) pages.push(-1);

      for (let i = minPage; i <= maxPage; i++) {
        pages.push(i);
      }

      if (maxPage < this.totalPages - 1) pages.push(-1);

      pages.push(this.totalPages);
    }

    return pages;
  }

  changePage(page: number) {
    if (page > 0 && page <= this.totalPages && page !== this.pageIndex) {
      this.$emit("changePage", page);
    }
  }

  updatePageSize(newPageSize: string) {
    this.$emit("changePageSize", parseInt(newPageSize, 10));
  }
}
</script>

<style scoped lang="scss">
.pagination {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;

  .page-controls {
    display: flex;
    gap: 5px;

    .btn {
      padding: 10px 15px;
      background-color: #555;
      color: #fff;
      border-radius: 5px;
      cursor: pointer;

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
    }
  }

  .page-size-selector {
    display: flex;
    align-items: center;
    gap: 5px;

    label {
      font-size: 1rem;
    }

    select {
      padding: 5px;
      border-radius: 5px;
    }
  }
}
</style>
