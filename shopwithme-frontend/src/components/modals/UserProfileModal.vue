<template>
  <div v-if="visible" class="modal-overlay" @click="close">
    <div class="modal-content" @click.stop>
      <h2>Profil użytkownika</h2>
      <p><strong>Nazwa użytkownika:</strong> {{ user?.name || "Nieznany" }}</p>
      <p><strong>Email:</strong> {{ user?.email || "Nieznany" }}</p>
      <button class="close-modal-btn" @click="close">Zamknij</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { mapGetters } from "vuex";
import { AccountState } from "@/store/modules/account";

export default defineComponent({
  name: "UserProfileModal",
  props: {
    visible: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    ...mapGetters("account", {
      currentUser: "currentUser",
    }),
    user(): AccountState["user"] {
      return this.currentUser as AccountState["user"];
    },
  },
  emits: ["close"],
  methods: {
    close() {
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
  max-width: 300px;
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
