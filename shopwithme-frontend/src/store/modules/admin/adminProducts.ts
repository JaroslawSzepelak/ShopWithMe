import { Module } from "vuex";
import { productAPI } from "@/plugins/adminAxios";

export interface AdminProductState {
  products: any[];
  selectedProduct: any | null;
  loading: boolean;
  error: string | null;
}

const adminProductsModule: Module<AdminProductState, any> = {
  namespaced: true,

  state: {
    products: [],
    selectedProduct: null,
    loading: false,
    error: null,
  },

  mutations: {
    SET_PRODUCTS(state, products: any[]) {
      state.products = products;
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
  },

  actions: {
    async fetchProducts({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await productAPI.getProducts();
        commit("SET_PRODUCTS", response.data);
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

    async fetchProduct({ commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await productAPI.getProduct(id);
        commit("SET_SELECTED_PRODUCT", response.data);
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
  },

  getters: {
    allProducts(state) {
      return state.products || [];
    },
    selectedProduct(state) {
      return state.selectedProduct;
    },
    isLoading(state) {
      return state.loading;
    },
    error(state) {
      return state.error;
    },
  },
};

export default adminProductsModule;
