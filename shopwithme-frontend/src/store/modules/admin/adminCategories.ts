import { Module } from "vuex";
import { categoryAPI } from "@/plugins/adminAxios";

export interface AdminCategoryState {
  categories: any[];
  selectedCategory: any | null;
  loading: boolean;
  error: string | null;
}

const adminCategoriesModule: Module<AdminCategoryState, any> = {
  namespaced: true,

  state: {
    categories: [],
    selectedCategory: null,
    loading: false,
    error: null,
  },

  mutations: {
    SET_CATEGORIES(state, categories: any[]) {
      state.categories = categories;
    },
    SET_SELECTED_CATEGORY(state, category: any) {
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
      try {
        const response = await categoryAPI.getCategories();
        commit("SET_CATEGORIES", response.data);
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
        commit("SET_ERROR", "Nie udało się pobrać kategorii");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async fetchCategory({ commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await categoryAPI.getCategory(id);
        commit("SET_SELECTED_CATEGORY", response.data);
      } catch (error) {
        console.error(`Error fetching category with id ${id}:`, error);
        commit("SET_ERROR", "Failed to fetch category.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async createCategory({ dispatch, commit }, category: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await categoryAPI.createCategory(category);
        await dispatch("fetchCategories");
      } catch (error) {
        console.error("Error creating category:", error);
        commit("SET_ERROR", "Failed to create category.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateCategory({ dispatch, commit }, category: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await categoryAPI.updateCategory(category.id, category.name);
        await dispatch("fetchCategories");
      } catch (error) {
        console.error("Error updating category:", error);
        commit("SET_ERROR", "Failed to update category.");
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
        console.error(`Error deleting category with id ${id}:`, error);
        commit("SET_ERROR", "Failed to delete category.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
  },

  getters: {
    allCategories(state) {
      return state.categories || [];
    },
    selectedCategory(state) {
      return state.selectedCategory;
    },
    isLoading(state) {
      return state.loading;
    },
    error(state) {
      return state.error;
    },
  },
};

export default adminCategoriesModule;
