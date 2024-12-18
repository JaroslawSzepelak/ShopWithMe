import { Module } from "vuex";
import { productAPI } from "@/plugins/adminAxios";

export interface AdminProductState {
  products: any[];
  autocompleteProducts: any[];
  selectedProduct: any | null;
  loading: boolean;
  error: string | null;
  totalRows: number;
  totalPages: number;
  pageSize: number;
  pageIndex: number;
}

const adminProductsModule: Module<AdminProductState, any> = {
  namespaced: true,

  state: {
    products: [],
    autocompleteProducts: [],
    selectedProduct: null,
    loading: false,
    error: null,
    totalRows: 0,
    totalPages: 0,
    pageSize: Number(sessionStorage.getItem("adminPageSize")) || 10,
    pageIndex: Number(sessionStorage.getItem("adminPageIndex")) || 1,
  },

  mutations: {
    SET_PRODUCTS(state, products: any[]) {
      state.products = products;
    },
    SET_AUTOCOMPLETE_PRODUCTS(state, products: any[]) {
      state.autocompleteProducts = products;
    },
    SET_SELECTED_PRODUCT(state, product: any) {
      state.selectedProduct = product;
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
      sessionStorage.setItem("adminPageIndex", String(pageIndex));
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
      sessionStorage.setItem("adminPageSize", String(pageSize));
    },
  },

  actions: {
    async fetchProducts({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProducts({
          pageIndex: state.pageIndex,
          pageSize: state.pageSize,
        });
        const { result, pager } = response.data;

        commit("SET_PRODUCTS", result);
        commit("SET_PAGER_INFO", pager);
      } catch (error: any) {
        if (error.response?.status === 404) {
          console.error("Lista produktów nie została znaleziona (404).");
          throw error;
        }
        commit("SET_ERROR", "Failed to fetch products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async fetchAutocompleteProducts({ commit }, search: string) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProductsAutocomplete(search);
        commit("SET_AUTOCOMPLETE_PRODUCTS", response.data);
      } catch (error) {
        console.error("Error fetching autocomplete products:", error);
        commit("SET_ERROR", "Failed to fetch autocomplete products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async fetchProduct({ commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await productAPI.getProduct(id);
        const product = response.data;

        commit("SET_SELECTED_PRODUCT", product);
      } catch (error) {
        console.error(`Error fetching product with id ${id}:`, error);
        commit("SET_ERROR", "Failed to fetch product.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async createProduct({ dispatch, commit }, product: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await productAPI.createProduct(product);
        await dispatch("fetchProducts");
      } catch (error) {
        console.error("Error creating product:", error);
        commit("SET_ERROR", "Failed to create product.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateProduct({ dispatch, commit }, product: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await productAPI.updateProduct(product);
        await dispatch("fetchProducts");
      } catch (error) {
        console.error("Error updating product:", error);
        commit("SET_ERROR", "Failed to update product.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async deleteProduct({ dispatch, commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await productAPI.deleteProduct(id);
        await dispatch("fetchProducts");
      } catch (error) {
        console.error(`Error deleting product with id ${id}:`, error);
        commit("SET_ERROR", "Failed to delete product.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async changePage({ commit, dispatch }, pageIndex: number) {
      commit("SET_PAGE_INDEX", pageIndex);
      await dispatch("fetchProducts");
    },

    async changePageSize({ commit, dispatch }, pageSize: number) {
      commit("SET_PAGE_SIZE", pageSize);
      commit("SET_PAGE_INDEX", 1);
      await dispatch("fetchProducts");
    },
  },

  getters: {
    allProducts(state) {
      return state.products || [];
    },
    autocompleteProducts(state) {
      return state.autocompleteProducts || [];
    },
    selectedProduct(state) {
      return state.selectedProduct || {};
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

export default adminProductsModule;
