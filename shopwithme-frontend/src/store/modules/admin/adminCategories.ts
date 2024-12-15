import { Module } from "vuex";
import { categoryAPI } from "@/plugins/adminAxios";

export interface AdminCategoryState {
  categories: any[];
  selectedCategory: any | null;
  loading: boolean;
  error: string | null;
  totalRows: number;
  totalPages: number;
  pageSize: number;
  pageIndex: number;
}

const adminCategoriesModule: Module<AdminCategoryState, any> = {
  namespaced: true,

  state: {
    categories: [],
    selectedCategory: null,
    loading: false,
    error: null,
    totalRows: 0,
    totalPages: 0,
    pageSize: Number(sessionStorage.getItem("adminCategoryPageSize")) || 10,
    pageIndex: Number(sessionStorage.getItem("adminCategoryPageIndex")) || 1,
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
    SET_PAGER_INFO(state, pager: { totalRows: number; totalPages: number }) {
      state.totalRows = pager.totalRows;
      state.totalPages = pager.totalPages;
    },
    SET_PAGE_INDEX(state, pageIndex: number) {
      state.pageIndex = pageIndex;
      sessionStorage.setItem("adminCategoryPageIndex", String(pageIndex));
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
      sessionStorage.setItem("adminCategoryPageSize", String(pageSize));
    },
  },

  actions: {
    async fetchCategories({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await categoryAPI.getCategories({
          pageIndex: state.pageIndex,
          pageSize: state.pageSize,
        });
        const { result, pager } = response.data;

        commit("SET_CATEGORIES", result);
        commit("SET_PAGER_INFO", {
          totalRows: pager.totalRows,
          totalPages: pager.totalPages,
        });
      } catch (error) {
        console.error("Błąd podczas pobierania kategorii:", error);
        commit("SET_ERROR", "Nie udało się pobrać kategorii.");
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
        console.error(`Błąd podczas pobierania kategorii o ID ${id}:`, error);
        commit("SET_ERROR", "Nie udało się pobrać szczegółów kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async createCategory({ dispatch, commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await dispatch("fetchCategories");
      } catch (error) {
        if (error instanceof Error) {
          console.error(
            "Błąd podczas tworzenia kategorii:",
            (error as any)?.response?.data || error.message
          );
        } else {
          console.error("Nieznany błąd:", error);
        }
        commit("SET_ERROR", "Nie udało się utworzyć kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateCategory(
      { dispatch, commit },
      category: { id: number; name: string }
    ) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await categoryAPI.updateCategory(category);
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
        console.error(`Błąd podczas usuwania kategorii o ID ${id}:`, error);
        commit("SET_ERROR", "Nie udało się usunąć kategorii.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async changePage({ commit, dispatch }, pageIndex: number) {
      commit("SET_PAGE_INDEX", pageIndex);
      await dispatch("fetchCategories");
    },

    async changePageSize({ commit, dispatch }, pageSize: number) {
      commit("SET_PAGE_SIZE", pageSize);
      commit("SET_PAGE_INDEX", 1);
      await dispatch("fetchCategories");
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
    totalRows(state) {
      return state.totalRows;
    },
    totalPages(state) {
      return state.totalPages;
    },
    pageSize(state) {
      return state.pageSize;
    },
    pageIndex(state) {
      return state.pageIndex;
    },
  },
};

export default adminCategoriesModule;
