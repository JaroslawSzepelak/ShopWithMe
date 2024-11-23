<template>
  <div class="order-progress">
    <div class="step">
      <div
        class="circle"
        :class="{ completed: currentStep > 1, active: currentStep === 1 }"
      >
        <span>1</span>
      </div>
      <p>Koszyk</p>
    </div>

    <div class="line" :class="{ completed: currentStep > 1 }"></div>

    <div class="step">
      <div
        class="circle"
        :class="{ completed: currentStep > 2, active: currentStep === 2 }"
      >
        <span>2</span>
      </div>
      <p>Dostawa</p>
    </div>

    <div class="line" :class="{ completed: currentStep > 2 }"></div>

    <div class="step">
      <div
        class="circle"
        :class="{ completed: currentStep > 3, active: currentStep === 3 }"
      >
        <span>3</span>
      </div>
      <p>Płatność</p>
    </div>

    <div class="line" :class="{ completed: currentStep > 3 }"></div>

    <div class="step">
      <div
        class="circle"
        :class="{ completed: currentStep > 4, active: currentStep === 4 }"
      >
        <span>4</span>
      </div>
      <p>Podsumowanie</p>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    currentStep: {
      type: Number,
      required: true,
    },
  },
};
</script>

<style scoped>
.order-progress {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  max-width: 1000px;
  margin: 0 auto;
  margin-bottom: 40px;
  padding-top: 20px;
}

.step {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  flex: 1;
}

.circle {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  border: 4px solid #000;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  font-weight: bold;
  background-color: white;
  transition: background-color 0.3s, border-color 0.3s;
}

.circle.completed {
  background-color: #c70a0a;
  color: white;
  border-color: #c70a0a;
}

.circle.active {
  border-color: #c70a0a;
}

.step p {
  margin-top: 10px;
  font-size: 16px;
  font-weight: bold;
  color: #333;
}

.line {
  position: relative;
  bottom: 15px;
  flex: 1;
  height: 4px;
  background-color: #000;
  margin: 0 10px;
  transition: background-color 0.3s;
  display: block;
}

.line.completed {
  background-color: #c70a0a;
}

.order-progress .line + .step {
  margin-left: 0;
}
</style>
