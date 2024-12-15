<template>
  <div class="login-container">
    <div class="login-card">
      <h1>Logowanie administratora</h1>
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
    </div>
    <AppModal
      :visible="showErrorModal"
      :message="errorMessage"
      @close="closeModal"
    />
    <LoggedInModal
      :visible="showAlreadyLoggedInModal"
      @close="handleModalClose"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import AppModal from "@/components/modals/AppModal.vue";
import LoggedInModal from "@/components/modals/LoggedInModal.vue";

@Component({
  components: { AppModal, LoggedInModal },
})
export default class AdminLogin extends Vue {
  username = "";
  password = "";
  showErrorModal = false;
  errorMessage = "";
  showAlreadyLoggedInModal = false;

  errors = {
    username: "",
    password: "",
  };

  async created() {
    const isAdminLoggedIn =
      this.$store.getters["admin/adminAccount/isAdminLoggedIn"];
    if (isAdminLoggedIn) {
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
      await this.$store.dispatch("admin/adminAccount/login", {
        username: this.username,
        password: this.password,
      });

      await this.$store.dispatch("admin/adminAccount/fetchAdminUser");

      this.$router.push("/admin");
    } catch (error: any) {
      console.error("Błąd logowania:", error);
      this.errorMessage =
        this.$store.getters["admin/adminAccount/accountError"] ||
        "Niepoprawny login lub hasło.";
      this.showErrorModal = true;
    }
  }

  closeModal() {
    this.showErrorModal = false;
  }

  handleModalClose() {
    this.showAlreadyLoggedInModal = false;
    this.$router.push("/admin");
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

.has-error input {
  border-color: red;
}

.error-message {
  color: red;
  font-size: 12px;
  margin-top: 5px;
}
</style>
