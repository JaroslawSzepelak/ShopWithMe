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
          class="btn btn-secondary btn-block"
          active-class="active"
        >
          Zamówienia
        </router-link>
      </aside>

      <main class="admin-content p-4">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";

@Component
export default class AdminPanel extends Vue {
  get isAdminLoggedIn(): boolean {
    return this.$store.getters["admin/adminProducts/isAdminLoggedIn"];
  }

  created() {
    if (this.$route.path === "/admin") {
      this.$router.push("/admin/products");
    }
  }

  @Watch("isAdminLoggedIn")
  onAdminLoggedInChange(newVal: boolean) {
    if (!newVal) {
      this.$router.push("/admin/login");
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
  background-color: #495057 !important;
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
</style>
