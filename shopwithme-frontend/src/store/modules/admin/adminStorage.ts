import { Module } from "vuex";
import { storageAPI } from "@/plugins/adminAxios";

export interface AdminStorageState {
  uploadProgress: number | null;
  error: string | null;
}

const adminStorageModule: Module<AdminStorageState, any> = {
  namespaced: true,

  state: {
    uploadProgress: null,
    error: null,
  },

  mutations: {
    SET_UPLOAD_PROGRESS(state, progress: number | null) {
      state.uploadProgress = progress;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
  },

  actions: {
    async uploadFile({ commit }, file: File) {
      commit("SET_UPLOAD_PROGRESS", null);
      commit("SET_ERROR", null);

      try {
        const response = await storageAPI.uploadFile(file, (progressEvent) => {
          const progress = Math.round(
            (progressEvent.loaded / progressEvent.total) * 100
          );
          commit("SET_UPLOAD_PROGRESS", progress);
        });

        return response.data;
      } catch (error) {
        console.error("Error uploading file:", error);
        commit("SET_ERROR", "Błąd podczas przesyłania pliku.");
        throw error;
      } finally {
        commit("SET_UPLOAD_PROGRESS", null);
      }
    },

    async downloadFile(_, fileName: string) {
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
        throw error;
      }
    },
  },

  getters: {
    uploadProgress(state): number | null {
      return state.uploadProgress;
    },
    error(state): string | null {
      return state.error;
    },
  },
};

export default adminStorageModule;
