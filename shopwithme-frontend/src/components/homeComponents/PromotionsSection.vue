<template>
  <div class="promotions-section">
    <h2>Okazje i oferty na czasie</h2>
    <div class="promotions-container">
      <div
        class="promotion"
        v-for="promotion in promotions"
        :key="promotion.id"
      >
        <div class="discount-circle">{{ promotion.discount }}</div>
        <p class="promotion-description">{{ promotion.description }}</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class PromotionsSection extends Vue {
  promotions = [
    { id: 1, discount: "-40%", description: "Na trzeci produkt" },
    { id: 2, discount: "-20%", description: "Inna promocja" },
    { id: 3, discount: "-10%", description: "Specjalna oferta" },
    { id: 4, discount: "-25%", description: "Wybrane produkty" },
    { id: 5, discount: "-50%", description: "Promocja tygodnia" },
    { id: 6, discount: "-15%", description: "Na zakupy powyżej 100 zł" },
  ];
}
</script>

<style scoped lang="scss">
.promotions-section {
  text-align: center;
  padding: 2rem 1rem;

  h2 {
    font-size: 2.5rem;
    margin-bottom: 3rem;
    color: #333;
  }

  .promotions-container {
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
    gap: 2rem;
  }

  .promotion {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 120px;

    .discount-circle {
      width: 120px;
      height: 120px;
      border-radius: 50%;
      background-color: #c70a0a;
      color: #fff;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 2rem;
      font-weight: bold;
      margin-bottom: 0.5rem;
    }

    .promotion-description {
      font-size: 1.2rem;
      color: #666;
      text-align: center;
    }
  }

  @media (max-width: 1200px) {
    h2 {
      font-size: 2rem;
    }

    .promotion {
      width: 100px;

      .discount-circle {
        width: 100px;
        height: 100px;
        font-size: 1.8rem;
      }

      .promotion-description {
        font-size: 1rem;
      }
    }
  }

  @media (max-width: 992px) {
    .promotion {
      width: 90px;

      .discount-circle {
        width: 90px;
        height: 90px;
        font-size: 1.6rem;
      }

      .promotion-description {
        font-size: 0.9rem;
      }
    }
  }

  @media (max-width: 768px) {
    h2 {
      font-size: 1.8rem;
    }

    .promotion {
      width: 80px;

      .discount-circle {
        width: 80px;
        height: 80px;
        font-size: 1.4rem;
      }

      .promotion-description {
        font-size: 0.8rem;
      }
    }
  }

  @media (max-width: 480px) {
    h2 {
      font-size: 1.5rem;
    }

    .promotion {
      width: 70px;

      .discount-circle {
        width: 70px;
        height: 70px;
        font-size: 1.2rem;
      }

      .promotion-description {
        font-size: 0.7rem;
      }
    }
  }
}
</style>
