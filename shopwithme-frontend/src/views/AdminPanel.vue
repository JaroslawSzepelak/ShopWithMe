<template>
  <div class="admin-panel">
    <header class="admin-header text-white p-4">
      <h1 class="navbar-brand">Panel Administracyjny</h1>
    </header>

    <div class="admin-container">
      <aside class="admin-sidebar p-4">
        <router-link
          to="/admin/products"
          class="btn btn-secondary btn-block mb-3"
          active-class="active"
        >
          Produkty
        </router-link>
        <router-link
          to="/admin/categories"
          class="btn btn-secondary btn-block mb-3"
          active-class="active"
        >
          Kategorie
        </router-link>
        <router-link
          to="/admin/orders"
          class="btn btn-secondary btn-block mb-3"
          active-class="active"
        >
          Zamówienia
        </router-link>
        <router-link
          to="/admin/users"
          class="btn btn-secondary btn-block"
          active-class="active"
        >
          Użytkownicy
        </router-link>
      </aside>

      <main class="admin-content p-4">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class AdminPanel extends Vue {
  get isAdminLoggedIn(): boolean {
    return this.$store.getters["admin/adminAccount/isAdminLoggedIn"];
  }

  async created() {
    try {
      await this.$store.dispatch("admin/adminAccount/fetchAdminUser");

      if (!this.isAdminLoggedIn) {
        this.$router.push("/admin/login");
      }
    } catch (error) {
      console.error(
        "Błąd podczas sprawdzania statusu logowania administratora:",
        error
      );
    }
  }
}
</script>

<style scoped lang="scss">
.admin-panel {
  display: flex;
  flex-direction: column;
  height: 100vh;
  font-family: Arial, sans-serif;
  font-size: 16px;
}

.admin-header {
  text-align: center;
  background-color: #646464;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.admin-header h1 {
  font-size: 2rem;
  font-weight: bold;
  margin: 0;
}

.admin-container {
  display: flex;
  flex-grow: 1;
  overflow: hidden;
}

.admin-sidebar {
  flex: 1;
  max-width: 300px;
  min-width: 300px;
  background-color: #918f8f;
}

.admin-sidebar .btn {
  box-sizing: border-box;
  width: 100%;
  text-align: left;
  font-size: 1.2rem;
  padding: 15px 20px;
  background-color: #72777c;
  color: #ffffff;
  transition: background-color 0.3s ease;
}

.btn:hover {
  background-color: #495057;
}

.admin-sidebar .active {
  background-color: #495057;
  font-weight: bold;
}

.admin-content {
  flex: 3;
  background-color: #f4f4f4;
  border-left: 1px solid #ddd;
  overflow-y: auto;
  font-size: 1.1rem;
}

body {
  margin: 0;
  padding: 0;
}

h1,
h4 {
  margin: 0;
}

@media (max-width: 1200px) {
  .admin-sidebar {
    max-width: 250px;
    min-width: 250px;

    .btn {
      font-size: 1rem;
      padding: 12px 18px;
    }
  }

  .admin-header h1 {
    font-size: 1.8rem;
  }
}

@media (max-width: 800px) {
  .admin-container {
    flex-direction: column;
  }

  .admin-sidebar {
    display: flex;
    flex-direction: column;
    align-items: center;
    max-width: 100%;
    min-width: 100%;
    max-height: 270px;
    padding: 10px;

    .btn {
      box-sizing: border-box;
      width: 60%;
      text-align: center;
      font-size: 0.9rem;
      padding: 10px 15px;
    }
  }

  .admin-content {
    flex: 1;
    padding: 10px;
    border-left: none;
    border-top: 1px solid #ddd;
  }
}

@media (max-width: 500px) {
  .admin-header h1 {
    font-size: 1.5rem;
  }

  .admin-sidebar {
    .btn {
      font-size: 0.8rem;
      padding: 8px 12px;
    }
  }

  .admin-content {
    font-size: 1rem;
  }
}
</style>
