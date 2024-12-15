<template>
  <div class="register-container">
    <div class="register-card">
      <h1>Rejestracja</h1>
      <form @submit.prevent="validateForm">
        <div class="form-group" :class="{ 'has-error': errors.userName }">
          <label for="userName">Nazwa użytkownika</label>
          <input
            type="text"
            id="userName"
            v-model="userName"
            class="form-control"
            placeholder="Wprowadź nazwę użytkownika"
          />
          <span v-if="errors.userName" class="error-message">
            {{ errors.userName }}
          </span>
        </div>
        <div class="form-group" :class="{ 'has-error': errors.email }">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="email"
            class="form-control"
            placeholder="Wprowadź email"
          />
          <span v-if="errors.email" class="error-message">
            {{ errors.email }}
          </span>
        </div>
        <div class="form-group" :class="{ 'has-error': errors.password }">
          <label for="password">Hasło</label>
          <input
            type="password"
            id="password"
            v-model="password"
            class="form-control"
            placeholder="Wprowadź hasło"
          />
          <span v-if="errors.password" class="error-message">
            {{ errors.password }}
          </span>
        </div>
        <div class="form-group" :class="{ 'has-error': errors.repeatPassword }">
          <label for="repeatPassword">Powtórz hasło</label>
          <input
            type="password"
            id="repeatPassword"
            v-model="repeatPassword"
            class="form-control"
            placeholder="Powtórz hasło"
          />
          <span v-if="errors.repeatPassword" class="error-message">
            {{ errors.repeatPassword }}
          </span>
        </div>
        <div class="form-actions">
          <button type="submit" class="btn btn-primary">Zarejestruj się</button>
          <button type="button" class="btn btn-secondary" @click="goToLogin">
            Powrót do logowania
          </button>
        </div>
      </form>
      <!-- Success Modal -->
      <AppModal
        v-if="showSuccessModal"
        :visible="showSuccessModal"
        :message="'Rejestracja zakończona pomyślnie. Przejdź do logowania.'"
        @close="goToLogin"
      />
      <!-- Error Modal -->
      <AppModal
        v-if="showErrorModal"
        :visible="showErrorModal"
        :message="formError"
        @close="closeModal"
      />
      <!-- Already Logged In Modal -->
      <LoggedInModal
        :visible="showAlreadyLoggedInModal"
        @close="handleLoggedInModalClose"
      />
    </div>
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
export default class Register extends Vue {
  userName = "";
  email = "";
  password = "";
  repeatPassword = "";
  formError: string | null = null;
  showErrorModal = false;
  showSuccessModal = false;
  showAlreadyLoggedInModal = false;

  errors = {
    userName: "",
    email: "",
    password: "",
    repeatPassword: "",
  };

  async created() {
    const isLoggedIn = this.$store.getters["account/isLoggedIn"];
    if (isLoggedIn) {
      this.showAlreadyLoggedInModal = true;
    }
  }

  validateForm() {
    this.errors.userName = this.userName
      ? ""
      : "Nazwa użytkownika jest wymagana.";
    this.errors.email = this.email ? "" : "Email jest wymagany.";
    this.errors.password = this.validatePassword(this.password);
    this.errors.repeatPassword =
      this.repeatPassword === this.password
        ? ""
        : "Hasła muszą być takie same.";

    if (
      !this.errors.userName &&
      !this.errors.email &&
      !this.errors.password &&
      !this.errors.repeatPassword
    ) {
      this.register();
    }
  }

  validatePassword(password: string): string {
    if (!password) {
      return "Hasło jest wymagane.";
    }
    if (password.length < 6) {
      return "Hasło musi mieć co najmniej 6 znaków.";
    }
    if (!/[a-z]/.test(password)) {
      return "Hasło musi zawierać co najmniej jedną małą literę.";
    }
    if (!/[A-Z]/.test(password)) {
      return "Hasło musi zawierać co najmniej jedną wielką literę.";
    }
    if (!/[^a-zA-Z0-9]/.test(password)) {
      return "Hasło musi zawierać co najmniej jeden znak specjalny.";
    }
    return "";
  }

  async register() {
    try {
      await this.$store.dispatch("account/register", {
        userName: this.userName,
        email: this.email,
        password: this.password,
        repeatPassword: this.repeatPassword,
      });
      this.showSuccessModal = true;
    } catch (error: any) {
      this.handleBackendErrors(error);
    }
  }

  handleBackendErrors(errors: any) {
    this.errors.userName = errors?.UserName || "";
    this.errors.email = errors?.Email || "";
    this.errors.repeatPassword = errors?.RepeatPassword || "";
    this.formError =
      errors?.[""] || "Wystąpił problem podczas rejestracji. Spróbuj ponownie.";
    this.showErrorModal = true;
  }

  closeModal() {
    this.showErrorModal = false;
  }

  goToLogin() {
    this.$router.push("/login");
  }

  handleLoggedInModalClose() {
    this.showAlreadyLoggedInModal = false;
    this.$router.go(-1);
  }
}
</script>

<style scoped lang="scss">
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f9f9f9;
}

.register-card {
  box-sizing: border-box;
  background: #ffffff;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  text-align: center;
  max-width: 500px;
  width: 100%;
}

h1 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #c70a0a;
}

.form-group {
  margin-bottom: 1rem;
  text-align: left;

  label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: bold;
    color: #333333;
  }

  input {
    box-sizing: border-box;
    width: 100%;
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-size: 1rem;
    background-color: #f9f9f9;

    &:focus {
      border-color: #c70a0a;
      outline: none;
      box-shadow: 0 0 3px rgba(199, 10, 10, 0.25);
    }
  }
}

.form-actions {
  margin-top: 1.5rem;
  text-align: center;

  .btn-primary {
    background-color: #c70a0a;
    color: white;
    padding: 0.5rem 1rem;
    margin: 10px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1rem;
    font-weight: bold;

    &:hover {
      background-color: #a50d0d;
    }
  }

  .btn-secondary {
    background-color: #6c757d;
    color: white;
    padding: 0.5rem 1rem;
    margin: 10px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1rem;
    font-weight: bold;

    &:hover {
      background-color: #5a6268;
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
