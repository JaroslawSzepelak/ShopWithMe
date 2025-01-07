<template>
  <nav class="navbar">
    <!-- Dla małych ekranów -->
    <div v-if="isSmallScreen" class="navbar-left">
      <router-link to="/" class="navbar-logo">
        <img src="@/assets/logo.svg" alt="ShopWithMe Logo" class="logo-image" />
      </router-link>
      <CategoryDropdownFullscreen @clicked="toggleDropdown" />
    </div>

    <!-- Dla dużych ekranów -->
    <div v-else class="navbar-left">
      <router-link to="/" class="navbar-logo">
        <img src="@/assets/logo.svg" alt="ShopWithMe Logo" class="logo-image" />
      </router-link>
      <CategoryDropdown @clicked="toggleDropdown" />
    </div>

    <div class="navbar-right">
      <!-- Komponent wyszukiwania -->
      <div class="navbar-search">
        <SearchAutocomplete
          :isAsync="true"
          :isLoading="isSearchLoading"
          :items="searchItems"
          :isOpen="isSearchOpen"
          :search="searchQuery"
          @update:search="updateSearchQuery"
          @select="goToProductDetails"
          @update:isOpen="updateIsSearchOpen"
          @update:items="updateSearchItems"
        />
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
            <button @click="viewOrderHistory">Historia zamówień</button>
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

    <!-- Modal profil -->
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
import CategoryDropdownFullscreen from "@/components/homeComponents/CategoryDropdownFullscreen.vue";
import AppModal from "@/components/modals/AppModal.vue";
import UserProfileModal from "@/components/modals/UserProfileModal.vue";
import SearchAutocomplete from "@/components/homeComponents/SearchAutocompleteNavbar.vue";

@Component({
  components: {
    CategoryDropdown,
    CategoryDropdownFullscreen,
    AppModal,
    UserProfileModal,
    SearchAutocomplete,
  },
})
export default class Navbar extends Vue {
  isDropdownVisible = false;
  showLogoutModal = false;
  isProfileModalVisible = false;
  isSearchLoading = false;
  searchQuery = "";
  searchItems = [];
  isSearchOpen = false;
  screenWidth = window.innerWidth;

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

  get isSmallScreen(): boolean {
    return this.screenWidth < 1150;
  }

  mounted() {
    window.addEventListener("resize", this.updateScreenWidth);
    this.updateScreenWidth();

    if (this.isLoggedIn) {
      this.$store.dispatch("account/fetchUser").catch((error) => {
        console.error("Błąd podczas pobierania danych użytkownika:", error);
      });
    }

    if (this.isAdminLoggedIn) {
      this.$store
        .dispatch("admin/adminAccount/fetchAdminUser")
        .catch((error) => {
          console.error(
            "Błąd podczas pobierania danych administratora: ",
            error
          );
        });
    }
  }

  destroyed() {
    window.removeEventListener("resize", this.updateScreenWidth);
  }

  updateScreenWidth() {
    this.screenWidth = window.innerWidth;
  }

  updateSearchQuery(value: string) {
    this.searchQuery = value;
  }

  updateSearchItems(items: any[]) {
    this.searchItems = items;
  }

  updateIsSearchOpen(isOpen: boolean) {
    this.isSearchOpen = isOpen;
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

  goToProductDetails(product: any) {
    if (product && product.id) {
      this.$router.push({
        name: "ProductDetails",
        params: { id: product.id.toString() },
      });
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

  viewOrderHistory() {
    this.$router.push("/orders/customer-history");
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

  .navbar-left,
  .navbar-right {
    display: flex;
    align-items: center;
    gap: 1.5rem;
  }

  .navbar-logo {
    .logo-image {
      height: 50px;
    }
  }

  .user-menu {
    position: relative;
    z-index: 1000;

    .user-dropdown {
      display: none;
      position: absolute;
      min-width: 250px;
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
      display: block;
    }
  }

  .cart-icon,
  .user-icon {
    color: #fff;
    font-size: 1.5rem;
    cursor: pointer;
  }

  @media (max-width: 870px) {
    padding: 1rem;

    .navbar-search {
      width: 100%;
      margin-bottom: 1rem;
      display: flex;
      justify-content: center;
    }

    .navbar-logo {
      .logo-image {
        height: 40px;
      }
    }

    .cart-icon,
    .user-icon {
      font-size: 1.2rem;
    }
  }

  @media (max-width: 700px) {
    .user-dropdown {
      min-width: 200px;
      padding: 0.4rem;
      font-size: 0.9rem;

      a,
      button {
        padding: 0.5rem 0.8rem;
        font-size: 0.9rem;
      }
    }
  }

  @media (max-width: 630px) {
    justify-content: flex-start;
    align-items: flex-start;
    gap: 1rem;

    .navbar-right {
      width: 100%;
      justify-content: center;
    }

    .navbar-search {
      width: 80%;
    }

    .navbar-logo {
      .logo-image {
        height: 40px;
      }
    }

    .cart-icon,
    .user-icon {
      font-size: 1.2rem;
    }
  }
}
</style>
