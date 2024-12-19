<template>
  <nav class="navbar">
    <div class="navbar-left">
      <router-link to="/" class="navbar-logo">
        <img src="@/assets/logo.svg" alt="ShopWithMe Logo" class="logo-image" />
      </router-link>
      <CategoryDropdown @clicked="toggleDropdown" />
    </div>
    <div class="navbar-right">
      <div class="navbar-search">
        <input type="text" placeholder="Szukaj" class="form-control" />
        <button class="search-button">
          <i class="fas fa-search"></i>
        </button>
      </div>
      <i class="fas fa-shopping-cart cart-icon" @click="goToCart"></i>
      <div class="user-menu" @mouseleave="hideDropdown">
        <i
          class="fas fa-user user-icon"
          @mouseenter="showDropdown"
          @click="toggleDropdownVisible"
        ></i>
        <div
          v-if="isDropdownVisible"
          class="user-dropdown"
          @mouseenter="keepDropdownVisible"
          @mouseleave="hideDropdown"
        >
          <template v-if="isLoggedIn || isAdminLoggedIn">
            <div class="dropdown-header">
              <span v-if="isAdminLoggedIn">
                Witaj, {{ currentAdmin?.userName || "Administratorze" }}
              </span>
              <span v-else>
                Witaj, {{ currentUser?.name || "Użytkowniku" }}
              </span>
            </div>
            <button v-if="!isAdminLoggedIn" @click="openProfileModal">
              Profil
            </button>
            <button class="logout-button" @click="handleLogout">
              Wyloguj się
            </button>
          </template>
          <template v-else>
            <router-link to="/login">Zaloguj się</router-link>
          </template>
        </div>
      </div>
    </div>
    <UserProfileModal
      :visible="isProfileModalVisible"
      @close="isProfileModalVisible = false"
    />
    <!-- Modal po wylogowaniu -->
    <AppModal
      :visible="showLogoutModal"
      message="Wylogowano pomyślnie. Twoja sesja została zakończona."
      @close="closeModal"
    />
  </nav>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import CategoryDropdown from "@/components/homeComponents/CategoryDropdown.vue";
import AppModal from "@/components/modals/AppModal.vue";
import UserProfileModal from "@/components/modals/UserProfileModal.vue";

@Component({
  components: {
    CategoryDropdown,
    AppModal,
    UserProfileModal,
  },
})
export default class Navbar extends Vue {
  isDropdownVisible = false;
  showLogoutModal = false;
  isProfileModalVisible = false;

  get isLoggedIn(): boolean {
    return this.$store.getters["account/isLoggedIn"];
  }

  get isAdminLoggedIn(): boolean {
    return this.$store.getters["admin/adminAccount/isAdminLoggedIn"];
  }

  get currentUser() {
    return this.$store.getters["account/currentUser"];
  }

  get currentAdmin() {
    return this.$store.getters["admin/adminAccount/adminUser"];
  }

  toggleDropdown(isOpen: boolean) {
    this.isDropdownVisible = isOpen;
  }

  toggleDropdownVisible() {
    this.isDropdownVisible = !this.isDropdownVisible;
  }

  showDropdown() {
    this.isDropdownVisible = true;
  }

  hideDropdown() {
    this.isDropdownVisible = false;
  }

  keepDropdownVisible() {
    this.isDropdownVisible = true;
  }

  goToCart() {
    if (this.$route.path !== "/cart") {
      this.$router.push("/cart");
    }
  }

  async handleLogout() {
    try {
      if (this.isAdminLoggedIn) {
        await this.$store.dispatch("admin/adminAccount/logout", "admin");
      } else {
        await this.$store.dispatch("account/logout");
      }
      this.showLogoutModal = true;

      setTimeout(() => {
        location.reload();
      }, 1000);
    } catch (error) {
      console.error("Błąd podczas wylogowywania:", error);
    }
  }

  openProfileModal() {
    this.isProfileModalVisible = true;
  }

  async mounted() {
    if (this.isLoggedIn) {
      try {
        await this.$store.dispatch("account/fetchUser");
      } catch (error) {
        console.error("Błąd podczas pobierania danych użytkownika:", error);
      }
    }

    if (this.isAdminLoggedIn) {
      try {
        await this.$store.dispatch("admin/adminAccount/fetchAdminUser");
      } catch (error) {
        console.error("Błąd podczas pobierania danych administratora:", error);
      }
    }
  }

  closeModal() {
    this.showLogoutModal = false;
  }
}
</script>

<style scoped lang="scss">
.navbar {
  box-sizing: border-box;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #000;
  padding: 1rem 2rem;
  width: 100%;

  .navbar-left {
    display: flex;
    align-items: center;
    gap: 2rem;
  }

  .navbar-right {
    display: flex;
    align-items: center;
    gap: 2rem;
  }

  .navbar-logo {
    .logo-image {
      height: 50px;
    }
  }

  .navbar-search {
    display: flex;
    align-items: center;
    position: relative;
    width: 320px;
    background: linear-gradient(135deg, #ffffff, #f0f0f0);
    border-radius: 25px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    overflow: hidden;

    input {
      flex: 1;
      padding: 0.5rem 1rem;
      border: none;
      border-radius: 25px;
      outline: none;
      font-size: 1rem;
      background: transparent;
      color: #333;

      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
    }

    .search-button {
      background: none;
      border: none;
      color: red;
      font-size: 1.2rem;
      padding-right: 10px;
      cursor: pointer;
      position: absolute;
      right: 15px;
      top: 50%;
      transform: translateY(-50%);
    }
  }

  .cart-icon,
  .user-icon {
    color: #fff;
    font-size: 1.5rem;
    cursor: pointer;
  }

  .user-menu {
    position: relative;
    z-index: 1000; /* Ensure dropdown is always on top */

    .user-dropdown {
      display: none;
      position: absolute;
      min-width: 250px; /* Widen dropdown */
      top: 1.5rem;
      right: 0;
      background-color: #ffffff;
      color: #000000;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      padding: 0.5rem;
      border: 1px solid #ddd;

      .dropdown-header {
        font-weight: bold;
        color: #333;
        margin-bottom: 10px;
      }

      a,
      button {
        width: 100%;
        color: #333;
        text-decoration: none;
        display: block;
        padding: 0.75rem 1rem;
        margin: 0.25rem 0;
        border-radius: 4px;
        font-weight: 500;
        transition: background-color 0.3s, color 0.3s;
        background: none;
        border: none;
        cursor: pointer;
        text-align: left;

        &:hover {
          background-color: #f0f0f0;
          color: #000;
        }
      }
    }

    &:hover .user-dropdown {
      display: block; /* Ensure dropdown remains visible */
    }
  }
}
</style>
