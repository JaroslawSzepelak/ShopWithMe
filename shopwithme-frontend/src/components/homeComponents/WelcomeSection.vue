<template>
  <section class="welcome-section">
    <div class="content">
      <h1>Witamy w sklepie <span class="highlight">ShopWithMe!</span></h1>
      <p>
        Odkryj szeroki wybór produktów w najnowszych kategoriach:
        <strong>RTV</strong>, <strong>AGD</strong>,
        <span class="highlight">Elektronika</span>,
        <strong>Dom i Ogród</strong>, oraz wiele więcej.
      </p>
      <p>
        <strong>ShopWithMe</strong> to miejsce, gdzie
        <span class="highlight">jakość</span> spotyka się z
        <span class="highlight">przystępnymi cenami</span>. Każdego dnia
        oferujemy:
      </p>
      <ul class="features-list">
        <li>Wyjątkowe <strong>promocje</strong> i rabaty</li>
        <li><span class="highlight">Szybką dostawę</span> pod Twój adres</li>
        <li>Bezpieczne i wygodne <strong>metody płatności</strong></li>
        <li>Niezrównaną <span class="highlight">obsługę klienta</span></li>
      </ul>
      <p>
        Nie czekaj – sprawdź
        <strong class="highlight">najnowsze hity sprzedażowe</strong> i stwórz
        swoją idealną listę zakupów <strong>już dziś!</strong>
      </p>
    </div>
    <div class="image-container">
      <img
        src="@/assets/banners/Najlepszy sprzęt RTV i AGD.png"
        alt="Shop Banner"
      />
    </div>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class WelcomeSection extends Vue {}
</script>

<style scoped lang="scss">
.welcome-section {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 40px 100px;
  background-color: #f8f9fa;
  border-radius: 8px;
  margin-top: 20px;

  .content {
    max-width: 60%;
    h1 {
      font-size: 2.8rem;
      color: #333;
      margin-bottom: 20px;

      .highlight {
        font-weight: bold;
      }
    }

    p {
      font-size: 1.2rem;
      color: #555;
      margin-bottom: 20px;
      line-height: 1.6;
    }

    .highlight {
      font-weight: bold;
    }

    .features-list {
      margin: 20px 0;
      padding-left: 20px;
      list-style-type: disc;

      li {
        font-size: 1.2rem;
        color: #555;
        margin-bottom: 10px;

        strong {
          font-weight: bold;
        }
      }
    }
  }

  .image-container {
    max-width: 40%;
    img {
      width: 100%;
      border-radius: 16px;
      box-shadow: 0 16px 30px rgba(0, 0, 0, 0.1);
    }
  }
}
</style>
