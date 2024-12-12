import { Module } from "vuex";
import { categoryAPI } from "@/plugins/shopAxios";

export interface Category {
  id: number;
  name: string;
}

interface CategoryState {
  categories: Category[];
  selectedCategory: Category | null;
  loading: boolean;
  error: string | null;
  totalCategories: number;
  pageIndex: number;
  pageSize: number;
}

const categoryModule: Module<CategoryState, any> = {
  namespaced: true,

  state: {
    categories: [],
    selectedCategory: null,
    loading: false,
    error: null,
    totalCategories: 0,
    pageIndex: 1,
    pageSize: 10,
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
    totalCategories(state): number {
      return state.totalCategories;
    },
    pageIndex(state): number {
      return state.pageIndex;
    },
    pageSize(state): number {
      return state.pageSize;
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
    SET_TOTAL_CATEGORIES(state, total: number) {
      state.totalCategories = total;
    },
    SET_PAGE_INDEX(state, pageIndex: number) {
      state.pageIndex = pageIndex;
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
    },
  },

  actions: {
    async fetchCategories({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await categoryAPI.getCategories(
          state.pageIndex,
          state.pageSize
        );
        const { result, pager } = response.data;

        commit("SET_CATEGORIES", result);
        commit("SET_TOTAL_CATEGORIES", pager.totalRows);
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
        commit("SET_ERROR", "Nie udało się pobrać kategorii produktów.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async fetchAllCategories({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await categoryAPI.getAllCategories();
        commit("SET_CATEGORIES", response.data);
      } catch (error) {
        console.error("Błąd podczas pobierania wszystkich kategorii:", error);
        commit("SET_ERROR", "Nie udało się pobrać wszystkich kategorii.");
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

    async changePage(
      { commit, dispatch },
      { pageIndex, pageSize }: { pageIndex: number; pageSize: number }
    ) {
      commit("SET_PAGE_INDEX", pageIndex);
      commit("SET_PAGE_SIZE", pageSize);
      await dispatch("fetchCategories");
    },
  },
};

export default categoryModule;
