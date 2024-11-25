import { Module, Commit } from "vuex";
import { categoryAPI } from "@/plugins/axios";

export interface Category {
  id: number;
  name: string;
}

interface CategoryState {
  categories: Category[];
  selectedCategory: Category | null;
  loading: boolean;
  error: string | null;
}

const categoryModule: Module<CategoryState, any> = {
  namespaced: true,

  state: {
    categories: [],
    selectedCategory: null,
    loading: false,
    error: null,
  },

  getters: {
    allCategories(state): Category[] {
      return state.categories;
    },
    selectedCategory(state): Category | null {
      return state.selectedCategory;
    },
    isLoading(state): boolean {
      return state.loading;
    },
    errorMessage(state): string | null {
      return state.error;
    },
  },

  mutations: {
    SET_CATEGORIES(state, categories: Category[]) {
      state.categories = categories;
    },
    SET_SELECTED_CATEGORY(state, category: Category | null) {
      state.selectedCategory = category;
    },
    SET_LOADING(state, loading: boolean) {
      state.loading = loading;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
  },

  actions: {
    async fetchCategories({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await categoryAPI.getCategories();
        commit("SET_CATEGORIES", response.data);
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
        commit("SET_ERROR", "Nie udało się pobrać kategorii produktów.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async fetchCategory({ commit }, categoryId: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await categoryAPI.getCategory(categoryId);
        commit("SET_SELECTED_CATEGORY", response.data);
      } catch (error) {
        console.error(
          `Błąd podczas pobierania kategorii o ID ${categoryId}:`,
          error
        );
        commit("SET_ERROR", "Nie udało się pobrać szczegółów kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async createCategory({ dispatch, commit }, name: string) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await categoryAPI.createCategory(name);
        await dispatch("fetchCategories");
      } catch (error) {
        console.error("Błąd podczas tworzenia kategorii:", error);
        commit("SET_ERROR", "Nie udało się utworzyć nowej kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateCategory(
      { dispatch, commit },
      { id, name }: { id: number; name: string }
    ) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await categoryAPI.updateCategory(id, name);
        await dispatch("fetchCategories");
      } catch (error) {
        console.error("Błąd podczas aktualizacji kategorii:", error);
        commit("SET_ERROR", "Nie udało się zaktualizować kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async deleteCategory({ dispatch, commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await categoryAPI.deleteCategory(id);
        await dispatch("fetchCategories");
      } catch (error) {
        console.error("Błąd podczas usuwania kategorii:", error);
        commit("SET_ERROR", "Nie udało się usunąć kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
  },
};

export default categoryModule;
