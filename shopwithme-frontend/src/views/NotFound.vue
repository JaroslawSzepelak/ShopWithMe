<template>
  <div class="not-found">
    <h1>Błąd 404 - Strona nie została odnaleziona</h1>
    <p>
      Niestety, strona, której szukasz, nie istnieje. Być może wpisałeś błędny
      adres URL lub strona została usunięta.
    </p>
    <div class="button-group">
      <button class="btn btn-secondary" @click="goBack">
        Powrót do poprzedniej strony
      </button>
      <router-link to="/" class="btn btn-primary"
        >Przejdź na stronę główną</router-link
      >
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class NotFound extends Vue {
  goBack() {
    this.$router.go(-1);
  }
}
</script>

<style scoped lang="scss">
.not-found {
  text-align: center;
  padding: 50px;
  font-family: Arial, sans-serif;

  h1 {
    font-size: 2.5rem;
    color: #dc3545;
    margin-bottom: 20px;
  }

  p {
    font-size: 1.2rem;
    color: #6c757d;
    margin-bottom: 30px;
  }

  .button-group {
    display: flex;
    justify-content: center;
    gap: 20px;

    .btn {
      font-size: 1rem;
      padding: 10px 20px;
    }

    .btn-primary {
      background-color: #007bff;
      border-color: #007bff;
      color: #fff;
    }

    .btn-secondary {
      background-color: #6c757d;
      border-color: #6c757d;
      color: #fff;
    }
  }
}
</style>
