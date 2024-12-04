<template>
  <div class="login-container">
    <div class="login-card">
      <h1>Logowanie</h1>
      <form @submit.prevent="validateForm">
        <div class="form-group" :class="{ 'has-error': errors.username }">
          <label for="username">Login</label>
          <input
            type="text"
            id="username"
            v-model="username"
            class="form-control"
            placeholder="Wprowadź swój login"
          />
          <span v-if="errors.username" class="error-message">
            {{ errors.username }}
          </span>
        </div>
        <div class="form-group" :class="{ 'has-error': errors.password }">
          <label for="password">Hasło</label>
          <input
            type="password"
            id="password"
            v-model="password"
            class="form-control"
            placeholder="Wprowadź swoje hasło"
          />
          <span v-if="errors.password" class="error-message">
            {{ errors.password }}
          </span>
        </div>
        <div v-if="formError" class="form-error">{{ formError }}</div>
        <div class="form-actions">
          <button type="submit" class="btn btn-primary">Zaloguj się</button>
        </div>
        <div class="form-links">
          <router-link to="/register"
            >Nie masz konta? Zarejestruj się!</router-link
          >
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class Login extends Vue {
  username = "";
  password = "";
  formError: string | null = null;

  errors = {
    username: "",
    password: "",
  };

  validateForm() {
    this.errors.username = this.username ? "" : "Login jest wymagany.";
    this.errors.password = this.password ? "" : "Hasło jest wymagane.";

    if (!this.errors.username && !this.errors.password) {
      this.login();
    }
  }

  async login() {
    try {
      console.log("Dane wprowadzane przez użytkownika:", {
        username: this.username,
        password: this.password,
      });
      await this.$store.dispatch("admin/adminAccount/login", {
        username: this.username,
        password: this.password,
      });
      this.$router.push("/");
    } catch (error: any) {
      console.error("Błąd logowania:", error);
      alert(
        this.$store.getters["admin/adminAccount/accountError"] ||
          "Niepoprawny login lub hasło."
      );
    }
  }
}
</script>

<style scoped lang="scss">
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f5f5f5;
}

.login-card {
  background: #ffffff;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  text-align: center;
  max-width: 400px;
  width: 100%;
}

h1 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
  text-align: left;

  label {
    display: block;
    margin-bottom: 0.5rem;
  }

  input {
    width: 100%;
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-size: 1rem;
  }
}

.form-actions {
  margin-top: 1.5rem;

  .btn {
    background-color: #d9534f;
    color: white;
    padding: 0.5rem 1rem;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1rem;

    &:hover {
      background-color: #c9302c;
    }
  }
}

.form-links {
  margin-top: 1rem;

  a {
    color: #007bff;
    text-decoration: none;

    &:hover {
      text-decoration: underline;
    }
  }
}

.has-error input {
  border-color: red;
}

.error-message {
  color: red;
  font-size: 12px;
  margin-top: 5px;
}
</style>
