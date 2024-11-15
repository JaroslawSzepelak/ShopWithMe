<template>
  <nav class="navbar">
    <div class="navbar-left">
      <router-link to="/" class="navbar-logo">
        <img src="@/assets/logo.svg" alt="ShopWithMe Logo" class="logo-image" />
      </router-link>
      <button class="navbar-category btn btn-dark">
        <i class="fas fa-bars"></i> Kategorie
      </button>
    </div>

    <div class="navbar-right">
      <div class="navbar-search">
        <input type="text" placeholder="Szukaj" class="form-control" />
        <button class="search-button">
          <i class="fas fa-search"></i>
        </button>
      </div>

      <i class="fas fa-shopping-cart cart-icon"></i>

      <div class="user-menu">
        <i class="fas fa-user user-icon"></i>
        <div class="user-dropdown">
          <router-link to="/login">Zaloguj się</router-link>
          <router-link to="/profile">Profil</router-link>
          <router-link to="/logout">Wyloguj się</router-link>
        </div>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class Navbar extends Vue {}
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

  .navbar-category {
    background-color: #333;
    color: #fff;
    border: none;
    font-size: 1rem;
    padding: 0.5rem 1rem;
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

      a {
        color: #333;
        text-decoration: none;
        display: block;
        padding: 0.75rem 1rem;
        margin: 0.25rem 0;
        border-radius: 4px;
        font-weight: 500;
        transition: background-color 0.3s, color 0.3s;

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
</style>
