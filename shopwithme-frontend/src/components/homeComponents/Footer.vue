<template>
  <footer class="footer">
    <div class="footer-content">
      <div class="footer-left">
        <div class="logo">
          <img src="@/assets/logo.svg" alt="ShopWithMe Logo" />
        </div>
        <p>Pomóżemy znaleźć<br />Twoje wymarzone produkty</p>
        <div class="social-icons">
          <a href="#" aria-label="Facebook" class="icon"
            ><i class="fab fa-facebook-f"></i
          ></a>
          <a href="#" aria-label="Instagram" class="icon"
            ><i class="fab fa-instagram"></i
          ></a>
          <a href="#" aria-label="Twitter" class="icon"
            ><i class="fab fa-twitter"></i
          ></a>
        </div>
        <p class="footer-rights">2024 wszystkie prawa zastrzeżone ShopWithMe</p>
      </div>
      <div class="footer-right">
        <div class="footer-column">
          <h4>INFORMACJE</h4>
          <a href="#">O nas</a>
          <a href="#">Regulamin</a>
          <a href="#">Dla mediów</a>
        </div>
        <div class="footer-column">
          <h4>MOJE KONTO</h4>
          <a href="#">Moje zamówienia</a>
          <a href="#">Moje dane</a>
          <a href="#">Schowek</a>
        </div>
        <div class="footer-column">
          <h4>OBSŁUGA KLIENTA</h4>
          <a href="#">Kontakt</a>
          <a href="#">Status zamówienia</a>
          <a href="#">Reklamacje</a>
        </div>
      </div>
    </div>
  </footer>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class Footer extends Vue {}
</script>

<style scoped lang="scss">
.footer {
  background-color: #888;
  color: #333;
  padding: 2rem 1rem;
  display: flex;
  justify-content: center;
  font-family: Arial, sans-serif;

  .footer-content {
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
    width: 100%;
  }

  .footer-left {
    text-align: left;
    font-size: 1.2rem;
    margin-left: 50px;
    max-width: 500px;

    .logo img {
      width: 200px;
      margin-bottom: 1rem;
    }

    .social-icons {
      display: flex;
      gap: 15px;
      margin-top: 1rem;

      .icon {
        color: #333;
        font-size: 1.5rem;
        text-decoration: none;
      }
    }

    .footer-rights {
      margin-top: 2rem;
      font-size: 1.2rem;
      color: #666;
    }
  }

  .footer-right {
    display: flex;
    margin-right: 50px;
    gap: 3rem;
    flex-wrap: wrap;
    text-align: left;

    .footer-column {
      min-width: 150px;

      h4 {
        font-size: 1.3rem;
        color: #000;
        text-transform: uppercase;
        font-weight: bold;
        margin-bottom: 1rem;
      }

      a {
        display: block;
        font-size: 1.2rem;
        color: #555;
        text-decoration: none;
        margin-bottom: 0.8rem;

        &:hover {
          color: #000;
        }
      }
    }
  }

  @media (max-width: 1150px) {
    .footer-content {
      flex-direction: row;
      gap: 2rem;
    }

    .footer-left {
      font-size: 1rem;
      margin-left: 0;
      max-width: 100%;
      text-align: center;

      .logo img {
        width: 150px;
      }

      .social-icons {
        justify-content: center;
        gap: 10px;

        .icon {
          font-size: 1.2rem;
        }
      }

      .footer-rights {
        font-size: 1rem;
      }
    }

    .footer-right {
      justify-content: center;
      margin-right: 0;
      gap: 1rem;

      .footer-column {
        text-align: center;

        h4 {
          font-size: 1.1rem;
        }

        a {
          font-size: 1rem;
        }
      }
    }
  }

  @media (max-width: 920px) {
    .footer-content {
      flex-direction: column-reverse;
      align-items: center;
      gap: 2rem;
    }

    .footer-left,
    .footer-right {
      margin: 0;
      text-align: center;
      width: 100%;
    }

    .footer-right {
      flex-direction: column;
      gap: 1rem;

      .footer-column {
        width: 100%;
      }
    }
  }

  @media (max-width: 600px) {
    .footer-content {
      flex-direction: column-reverse;
      align-items: center;
      gap: 1.5rem;
    }

    .footer-left,
    .footer-right {
      margin: 0;
      text-align: center;
    }

    .footer-right {
      flex-direction: column;
      gap: 1rem;

      .footer-column {
        width: 100%;
      }
    }
  }
}
</style>
