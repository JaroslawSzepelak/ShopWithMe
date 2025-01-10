<template>
  <div v-if="visible" class="modal-overlay">
    <div class="modal-content">
      <h2>Już jesteś zalogowany</h2>
      <p>Nie możesz przejść do tej strony, ponieważ jesteś już zalogowany.</p>
      <button class="close-modal-btn" @click="goBack">Wróć</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "LoggedInModal",
  props: {
    visible: {
      type: Boolean,
      required: true,
    },
  },
  emits: ["close"],
  methods: {
    goBack() {
      this.$router.go(-1);
      this.$emit("close");
    },
  },
});
</script>

<style scoped>
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
  max-width: 400px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.close-modal-btn {
  background-color: #c70a0a;
  color: #fff;
  border: none;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border-radius: 4px;
  cursor: pointer;
}

.close-modal-btn:hover {
  background-color: #a00000;
}
</style>
