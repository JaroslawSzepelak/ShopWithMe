<template>
  <div class="about-section">
    <h2>O nas</h2>
    <div class="features-container">
      <div class="feature" v-for="feature in features" :key="feature.id">
        <i :class="feature.icon" class="feature-icon"></i>
        <div class="feature-text">
          <h3>{{ feature.title }}</h3>
          <p>{{ feature.description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class AboutSection extends Vue {
  features = [
    {
      id: 1,
      icon: "fas fa-trophy",
      title: "Wysoka jakość",
      description: "Produkty z najwyższą jakością",
    },
    {
      id: 2,
      icon: "fas fa-shield-alt",
      title: "Ochronna gwarancja",
      description: "Nawet na 2 lata",
    },
    {
      id: 3,
      icon: "fas fa-truck",
      title: "Darmowa dostawa",
      description: "Dla zamówień powyżej 200 zł",
    },
    {
      id: 4,
      icon: "fas fa-headset",
      title: "Wsparcie 24/7",
      description: "Całodobowa infolinia",
    },
  ];
}
</script>

<style scoped lang="scss">
.about-section {
  text-align: center;
  padding: 3rem 1rem;

  h2 {
    font-size: 2.5rem;
    margin-bottom: 3rem;
    color: #333;
  }

  .features-container {
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
    gap: 1.5rem;
  }

  .feature {
    display: flex;
    flex-direction: column;
    align-items: center;
    max-width: 150px;
    text-align: center;

    .feature-icon {
      font-size: 2.5rem;
      color: #333;
      margin-bottom: 0.5rem;
    }

    .feature-text {
      h3 {
        font-size: 1.1rem;
        font-weight: bold;
        color: #333;
        margin-bottom: 0.2rem;
      }

      p {
        font-size: 0.9rem;
        color: #666;
        margin: 0;
      }
    }
  }
}
</style>
