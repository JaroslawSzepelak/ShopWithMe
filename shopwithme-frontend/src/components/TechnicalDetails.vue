<template>
  <div class="technical-details">
    <h2>Dane techniczne</h2>
    <div v-if="Object.keys(technicalDetails).length === 0">
      <p class="no-data-message">Brak danych technicznych dla tego produktu.</p>
    </div>
    <table v-else class="technical-table">
      <tbody>
        <tr v-for="(value, key) in technicalDetails" :key="key">
          <td class="key-column">{{ key }}</td>
          <td class="value-column">{{ value }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class TechnicalDetails extends Vue {
  productId: number | null = null;
  technicalDetails: Record<string, string> = {};

  async created() {
    this.productId = Number(this.$route.params.id);

    if (this.productId) {
      try {
        await this.$store.dispatch("products/fetchProduct", this.productId);
        const product = this.$store.getters["products/selectedProduct"];

        if (product && product.technicalData) {
          this.technicalDetails = JSON.parse(
            product.technicalData.replace(/\\r\\n/g, "").trim()
          );
        }
      } catch (error) {
        console.error("Błąd podczas pobierania danych technicznych:", error);
      }
    }
  }
}
</script>

<style scoped lang="scss">
.technical-details {
  padding: 2rem;
  margin-top: 30px;
  margin-bottom: 30px;
  width: 70%;
  background-color: #f9f9f9;

  h2 {
    font-size: 2rem;
    margin-bottom: 1.5rem;
    color: #333;
    text-align: center;
  }

  .no-data-message {
    font-size: 1.2rem;
    color: #555;
    text-align: center;
    margin-top: 1rem;
  }

  .technical-table {
    width: 100%;
    margin: 0 auto;
    border-collapse: collapse;
    background-color: white;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);

    td {
      padding: 15px 20px;
      border: 1px solid #ddd;
      font-size: 1rem;
      color: #555;
      text-align: left;
    }

    .key-column {
      font-weight: bold;
      background-color: #f2f2f2;
      width: 35%;
    }

    .value-column {
      color: #333;
      width: 65%;
    }
  }
}
</style>
