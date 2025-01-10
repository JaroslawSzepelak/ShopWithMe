<template>
  <div class="category-editor">
    <h4>{{ editMode ? "Edytuj kategorię" : "Utwórz kategorię" }}</h4>
    <div v-if="errorMessages.length" class="error-messages">
      <ul>
        <li v-for="(error, index) in errorMessages" :key="index">
          {{ error }}
        </li>
      </ul>
    </div>
    <div class="form-container">
      <div class="form-group">
        <label>Nazwa kategorii</label>
        <input class="form-control" v-model.trim="category.name" />
      </div>
    </div>
    <div class="button-group">
      <router-link to="/admin/categories" class="btn back-btn"
        >Anuluj</router-link
      >
      <button class="btn save-btn" @click="handleSave">
        {{ editMode ? "Zapisz zmiany" : "Utwórz kategorię" }}
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class CategoryEditor extends Vue {
  category = { id: null, name: "" };
  errorMessages: string[] = [];

  get editMode(): boolean {
    return this.$route.name === "CategoryEdit";
  }

  async created() {
    if (this.editMode) {
      const categoryId = this.$route.params.id;
      try {
        await this.$store.dispatch(
          "admin/adminCategories/fetchCategory",
          categoryId
        );
        const fetchedCategory =
          this.$store.getters["admin/adminCategories/selectedCategory"];
        this.category = { id: fetchedCategory.id, name: fetchedCategory.name };
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
      }
    }
  }

  validateForm() {
    this.errorMessages = [];
    if (!this.category.name) {
      this.errorMessages.push("Pole 'Nazwa kategorii' jest wymagane.");
    }
    return this.errorMessages.length === 0;
  }

  async handleSave() {
    if (!this.validateForm()) {
      console.warn("Niepoprawne dane formularza:", this.category);
      return;
    }

    const action = this.editMode ? "updateCategory" : "createCategory";

    try {
      await this.$store.dispatch(
        `admin/adminCategories/${action}`,
        this.category
      );
      this.$router.push("/admin/categories");
    } catch (error) {
      console.error("Błąd podczas zapisywania kategorii:", error);
    }
  }
}
</script>

<style scoped lang="scss">
.category-editor {
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
  .category-editor {
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
  .category-editor {
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
  .category-editor {
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
