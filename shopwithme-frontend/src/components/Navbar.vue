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
      <div class="user-menu">
        <i class="fas fa-user user-icon"></i>
        <div class="user-dropdown">
          <router-link v-if="!isLoggedIn" to="/login">Zaloguj się</router-link>
          <router-link v-else to="/profile">Profil</router-link>
          <button v-if="isLoggedIn" class="logout-button" @click="handleLogout">
            Wyloguj się
          </button>
        </div>
      </div>
    </div>
    <!-- Modal po wylogowaniu -->
    <div v-if="showLogoutModal" class="modal-overlay" @click="closeModal">
      <div class="modal-content" @click.stop>
        <h2>Wylogowano pomyślnie</h2>
        <p>Twoja sesja została zakończona.</p>
        <button @click="closeModal" class="btn btn-primary">Zamknij</button>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import CategoryDropdown from "@/components/CategoryDropdown.vue";
import { mapGetters, mapActions } from "vuex";

@Component({
  components: {
    CategoryDropdown,
  },
  computed: {
    ...mapGetters("admin/adminAccount", ["isLoggedIn"]),
  },
  methods: {
    ...mapActions("admin/adminAccount", ["logout"]),
  },
})
export default class Navbar extends Vue {
  isDropdownVisible = false;
  showLogoutModal = false;

  get isLoggedIn(): boolean {
    return this.$store.getters["admin/adminAccount/isLoggedIn"];
  }

  toggleDropdown(isOpen: boolean) {
    this.isDropdownVisible = isOpen;
  }

  goToCart() {
    if (this.$route.path !== "/cart") {
      this.$router.push("/cart");
    }
  }

  async handleLogout() {
    try {
      await this.$store.dispatch("admin/adminAccount/logout");
      this.showLogoutModal = true;

      setTimeout(() => {
        location.reload();
      }, 1000);
    } catch (error) {
      console.error("Błąd podczas wylogowywania:", error);
    }
  }

  closeModal() {
    this.showLogoutModal = false;
  }
}
</script>

<style scoped lang="scss">
.navbar {
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

    .user-dropdown {
      display: none;
      position: absolute;
      min-width: 200px;
      top: 1.5rem;
      right: 0;
      background-color: #ffffff;
      color: #000000;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      padding: 0.5rem;
      border: 1px solid #ddd;

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
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background-color: #fff;
  padding: 2rem;
  border-radius: 8px;
  text-align: center;
  max-width: 300px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);

  h2 {
    font-size: 1.5rem;
    margin-bottom: 1rem;
  }

  p {
    font-size: 1.2rem;
    color: #333;
    margin-bottom: 1rem;
  }

  .btn {
    background-color: #c70a0a;
    color: #fff;
    border: none;
    padding: 0.5rem 1rem;
    font-size: 1rem;
    border-radius: 4px;
    cursor: pointer;

    &:hover {
      background-color: #a50e0e;
    }
  }
}
</style>
