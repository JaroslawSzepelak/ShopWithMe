import { Module } from "vuex";
import { storageAPI } from "@/plugins/shopAxios";

export interface PublicStorageState {
  error: string | null;
}

const publicStorageModule: Module<PublicStorageState, any> = {
  namespaced: true,

  state: {
    error: null,
  },

  mutations: {
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
  },

  actions: {
    async downloadFile({ commit }, fileName: string) {
      commit("SET_ERROR", null);

      try {
        const response = await storageAPI.getFile(fileName);
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.URL.revokeObjectURL(url);
      } catch (error) {
        console.error("Error downloading file:", error);
        commit("SET_ERROR", "Błąd podczas pobierania pliku.");
      }
    },
  },

  getters: {
    error(state) {
      return state.error;
    },
  },
};

export default publicStorageModule;
