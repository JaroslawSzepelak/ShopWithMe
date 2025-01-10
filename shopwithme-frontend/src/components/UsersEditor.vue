<template>
  <div class="user-editor">
    <h4>{{ editMode ? "Edytuj użytkownika" : "Dodaj użytkownika" }}</h4>
    <div v-if="errorMessages.length" class="error-messages">
      <ul>
        <li v-for="(error, index) in errorMessages" :key="index">
          {{ error }}
        </li>
      </ul>
    </div>
    <div class="form-container">
      <div class="form-group">
        <label>Nazwa użytkownika</label>
        <input class="form-control" v-model.trim="user.userName" />
      </div>
      <div class="form-group">
        <label>Email</label>
        <input class="form-control" v-model.trim="user.email" />
      </div>
      <div class="form-group">
        <label v-if="editMode">Nowe hasło</label>
        <label v-else>Hasło</label>
        <input
          type="password"
          class="form-control"
          v-model.trim="user.password"
        />
      </div>
      <div class="form-group" v-if="user.password">
        <label>Powtórz hasło</label>
        <input
          type="password"
          class="form-control"
          v-model.trim="user.repeatPassword"
        />
      </div>
    </div>
    <div class="button-group">
      <router-link to="/admin/users" class="btn back-btn">Anuluj</router-link>
      <button class="btn save-btn" @click="handleSave">
        {{ editMode ? "Zapisz zmiany" : "Dodaj użytkownika" }}
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class UsersEditor extends Vue {
  user: any = { userName: "", email: "", password: "", repeatPassword: "" };
  originalUser: any = null;
  errorMessages: string[] = [];

  get editMode(): boolean {
    return this.$route.name === "UserEdit";
  }

  async created() {
    if (this.editMode) {
      const userId = this.$route.params.id;
      try {
        if (!this.$store.getters["admin/adminUsers/allUsers"].length) {
          await this.$store.dispatch("admin/adminUsers/fetchUsers");
        }

        const fetchedUser = this.$store.getters[
          "admin/adminUsers/allUsers"
        ].find((u: any) => u.id === userId);

        if (fetchedUser) {
          this.originalUser = { ...fetchedUser };
          this.user = {
            ...fetchedUser,
            password: "",
            repeatPassword: "",
          };
        } else {
          console.error("Nie znaleziono użytkownika o podanym ID.");
          this.$router.push("/admin/users");
        }
      } catch (error) {
        console.error("Błąd podczas pobierania użytkownika:", error);
      }
    }
  }

  validateForm() {
    this.errorMessages = [];
    if (!this.user.userName)
      this.errorMessages.push("Pole 'Nazwa użytkownika' jest wymagane.");
    if (!this.user.email)
      this.errorMessages.push("Pole 'Email' jest wymagane.");

    if (this.user.password) {
      if (this.user.password !== this.user.repeatPassword) {
        this.errorMessages.push("Hasła muszą się zgadzać.");
      }
    }

    return this.errorMessages.length === 0;
  }

  async handleSave() {
    if (!this.validateForm()) return;

    const action = this.editMode ? "updateUser" : "createUser";

    const payload = this.editMode
      ? {
          ...this.originalUser,
          ...this.user,
        }
      : { ...this.user };

    if (this.editMode && !this.user.password) {
      delete payload.password;
      delete payload.repeatPassword;
    }

    try {
      await this.$store.dispatch(`admin/adminUsers/${action}`, payload);
      this.$router.push("/admin/users");
    } catch (error) {
      console.error("Błąd podczas zapisywania użytkownika:", error);
    }
  }
}
</script>

<style scoped lang="scss">
.user-editor {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f2f2f2;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  font-family: Arial, sans-serif;

  h4 {
    font-size: 1.8rem;
    font-weight: bold;
    color: #fff;
    background-color: #c70a0a;
    padding: 15px;
    text-align: center;
    border-radius: 8px 8px 0 0;
  }

  .form-container {
    display: flex;
    flex-direction: column;
    gap: 15px;
    margin-top: 20px;
  }

  .form-group {
    display: flex;
    flex-direction: column;

    label {
      font-size: 1rem;
      font-weight: bold;
      margin-bottom: 5px;
    }

    .form-control {
      padding: 10px;
      font-size: 1rem;
      border: 1px solid #ccc;
      border-radius: 4px;
    }
  }

  .button-group {
    display: flex;
    justify-content: flex-end;
    gap: 15px;
    margin-top: 20px;

    .btn {
      padding: 10px 20px;
      font-size: 1rem;
      font-weight: bold;
      border-radius: 4px;
      cursor: pointer;
      text-align: center;
    }

    .back-btn {
      background-color: #666;
      color: white;
      border: none;

      &:hover {
        background-color: #555;
      }
    }

    .save-btn {
      background-color: #c70a0a;
      color: white;
      border: none;

      &:hover {
        background-color: #a60a0a;
      }
    }
  }

  .error-messages {
    background-color: #ffefef;
    border: 1px solid #ff4d4d;
    color: #c70a0a;
    padding: 10px;
    border-radius: 4px;
    margin-bottom: 15px;

    ul {
      list-style: none;
      padding: 0;
      margin: 0;

      li {
        font-size: 0.9rem;
      }
    }
  }
}

@media (max-width: 1200px) {
  .user-editor {
    max-width: 700px;

    h4 {
      font-size: 1.6rem;
    }

    .form-control {
      font-size: 0.95rem;
    }
  }
}

@media (max-width: 768px) {
  .user-editor {
    padding: 15px;

    h4 {
      font-size: 1.4rem;
      padding: 10px;
    }

    .form-container {
      gap: 10px;
    }

    .form-control {
      font-size: 0.9rem;
      padding: 8px;
    }

    .button-group {
      flex-direction: column;
      align-items: stretch;

      .btn {
        font-size: 0.9rem;
        padding: 8px;
      }
    }

    .error-messages {
      font-size: 0.9rem;

      ul li {
        font-size: 0.85rem;
      }
    }
  }
}

@media (max-width: 480px) {
  .user-editor {
    padding: 10px;

    h4 {
      font-size: 1.2rem;
    }

    .form-control {
      font-size: 0.85rem;
      padding: 6px;
    }

    .button-group {
      gap: 10px;

      .btn {
        font-size: 0.8rem;
        padding: 6px;
      }
    }

    .error-messages {
      font-size: 0.8rem;

      ul li {
        font-size: 0.75rem;
      }
    }
  }
}
</style>
