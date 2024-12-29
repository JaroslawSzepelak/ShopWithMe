import { Module } from "vuex";
import { productAPI, storageAPI } from "@/plugins/adminAxios";

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
      state.products = products.map((product: any) => ({
        ...product,
        mainImage: product.mainImage
          ? {
              id: product.mainImage.id,
              name: product.mainImage.name,
              path: product.mainImage.path,
              contentType: product.mainImage.contentType,
            }
          : null,
      }));
    },
    SET_AUTOCOMPLETE_PRODUCTS(state, products: any[]) {
      state.autocompleteProducts = products;
    },
    SET_SELECTED_PRODUCT(state, product: any) {
      state.selectedProduct = product
        ? {
            ...product,
            mainImage: product.mainImage
              ? {
                  id: product.mainImage.id,
                  name: product.mainImage.name,
                  path: product.mainImage.path,
                  contentType: product.mainImage.contentType,
                  url: null,
                }
              : null,
          }
        : null;
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
    SET_MAIN_IMAGE_URL(
      state,
      { productId, url }: { productId: number; url: string }
    ) {
      const product = state.products.find((p) => p.id === productId);
      if (product && product.mainImage) {
        product.mainImage.url = url;
      }
    },
    SET_SELECTED_PRODUCT_IMAGE_URL(state, url: string) {
      if (state.selectedProduct && state.selectedProduct.mainImage) {
        state.selectedProduct.mainImage.url = url;
      }
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

        for (const product of result) {
          if (product.mainImage?.name) {
            const fileResponse = await storageAPI.getFile(
              product.mainImage.name
            );
            const url = window.URL.createObjectURL(
              new Blob([fileResponse.data])
            );
            commit("SET_MAIN_IMAGE_URL", { productId: product.id, url });
          }
        }

        commit("SET_PAGER_INFO", pager);
      } catch (error: any) {
        console.error("Error fetching products:", error);
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

        const processedProduct = {
          ...product,
          mainImage: product.mainImage
            ? {
                id: product.mainImage.id,
                name: product.mainImage.name,
                path: product.mainImage.path,
                contentType: product.mainImage.contentType,
                url: null,
              }
            : null,
        };

        commit("SET_SELECTED_PRODUCT", processedProduct);

        if (processedProduct.mainImage?.name) {
          const fileResponse = await storageAPI.getFile(
            processedProduct.mainImage.name
          );
          const url = window.URL.createObjectURL(new Blob([fileResponse.data]));
          commit("SET_SELECTED_PRODUCT_IMAGE_URL", url);
        }
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

      const payload = {
        ...product,
        mainImage: null,
      };

      try {
        await productAPI.createProduct(payload);
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

      const payload = {
        ...product,
        mainImage: null,
      };

      try {
        await productAPI.updateProduct(payload);
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
