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
        <div class="form-actions">
          <button type="submit" class="btn btn-primary">Zaloguj się</button>
        </div>
      </form>
      <div class="form-links">
        <p>
          Nie masz jeszcze konta?
          <router-link to="/register" class="register-link"
            >Załóż je tutaj!</router-link
          >
        </p>
      </div>
    </div>
    <!-- Modal for error -->
    <AppModal
      v-if="showErrorModal"
      :visible="showErrorModal"
      :message="formError"
      @close="closeModal"
    />
    <!-- Modal for already logged-in users -->
    <LoggedInModal
      :visible="showAlreadyLoggedInModal"
      @close="handleLoggedInModalClose"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import AppModal from "@/components/modals/AppModal.vue";
import LoggedInModal from "@/components/modals/LoggedInModal.vue";

@Component({
  components: {
    AppModal,
    LoggedInModal,
  },
})
export default class Login extends Vue {
  username = "";
  password = "";
  formError: string | null = null;
  showErrorModal = false;
  showAlreadyLoggedInModal = false;

  errors = {
    username: "",
    password: "",
  };

  async created() {
    const isLoggedIn = this.$store.getters["account/isLoggedIn"];
    if (isLoggedIn) {
      this.showAlreadyLoggedInModal = true;
    }
  }

  validateForm() {
    this.errors.username = this.username ? "" : "Login jest wymagany.";
    this.errors.password = this.password ? "" : "Hasło jest wymagane.";

    if (!this.errors.username && !this.errors.password) {
      this.login();
    }
  }

  async login() {
    try {
      await this.$store.dispatch("account/login", {
        username: this.username,
        password: this.password,
      });

      await this.$store.dispatch("account/fetchUser");

      this.$router.push("/");
    } catch (error: any) {
      console.error("Błąd logowania:", error);
      this.formError =
        this.$store.getters["account/accountError"] ||
        "Niepoprawny login lub hasło.";
      this.showErrorModal = true;
    }
  }

  closeModal() {
    this.showErrorModal = false;
  }

  handleLoggedInModalClose() {
    this.showAlreadyLoggedInModal = false;
    this.$router.go(-1);
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
  box-sizing: border-box;
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
    box-sizing: border-box;
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

  p {
    font-size: 0.9rem;
    color: #555;

    .register-link {
      color: #007bff;
      text-decoration: none;

      &:hover {
        text-decoration: underline;
      }
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

@media (max-width: 480px) {
  .login-container {
    box-sizing: border-box;
    height: auto;
    padding: 20px;
    align-items: flex-start;
  }

  .login-card {
    padding: 1.5rem;
    max-width: 100%;
    border-radius: 8px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  }

  h1 {
    font-size: 1.8rem;
  }

  .form-group input {
    font-size: 0.9rem;
    padding: 0.4rem;
  }

  .form-actions .btn {
    font-size: 0.9rem;
    padding: 0.4rem 0.8rem;
  }

  .form-links p {
    font-size: 0.8rem;
  }
}
</style>
