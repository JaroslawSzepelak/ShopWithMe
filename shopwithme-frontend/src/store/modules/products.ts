import { Module } from "vuex";
import { productAPI } from "@/plugins/axios";

interface Product {
  id: number;
  name: string;
  lead: string;
  price: number;
  categoryId: number;
  description?: string;
  technicalDetails?: string;
  images: string[];
}

interface ProductsState {
  products: Product[];
  paginatedProducts: Product[];
  selectedProduct: Product | null;
  loading: boolean;
  error: string | null;
  currentPage: number;
  pageSize: number;
  totalRows: number;
  totalPages: number;
}

const productsModule: Module<ProductsState, any> = {
  namespaced: true,
  state: {
    products: [],
    paginatedProducts: [],
    selectedProduct: null,
    loading: false,
    error: null,
    currentPage: Number(sessionStorage.getItem("currentPage")) || 1,
    pageSize: Number(sessionStorage.getItem("pageSize")) || 10,
    totalRows: 0,
    totalPages: 0,
  },
  mutations: {
    SET_PRODUCTS(state, products: Product[]) {
      state.products = products;
    },
    SET_PAGINATED_PRODUCTS(state, products: Product[]) {
      state.paginatedProducts = products;
    },
    SET_SELECTED_PRODUCT(state, product: Product | null) {
      state.selectedProduct = product;
    },
    SET_LOADING(state, loading: boolean) {
      state.loading = loading;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_CURRENT_PAGE(state, page: number) {
      state.currentPage = page;
      sessionStorage.setItem("currentPage", String(page));
    },
    SET_PAGE_SIZE(state, size: number) {
      state.pageSize = size;
      sessionStorage.setItem("pageSize", String(size));
    },
    SET_PAGER_INFO(state, pager: { totalRows: number; totalPages: number }) {
      state.totalRows = pager.totalRows;
      state.totalPages = pager.totalPages;
    },
  },
  actions: {
    async fetchProducts({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProducts(
          state.currentPage,
          state.pageSize
        );
        const { result, pager } = response.data;

        commit("SET_PAGINATED_PRODUCTS", result);
        commit("SET_PAGER_INFO", {
          totalRows: pager.totalRows,
          totalPages: pager.totalPages,
        });
      } catch (error) {
        console.error("Error fetching products:", error);
        commit("SET_ERROR", "Failed to fetch products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async fetchProduct({ commit }, productId: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProduct(productId);
        commit("SET_SELECTED_PRODUCT", response.data);
      } catch (error) {
        console.error("Error fetching product details:", error);
        commit("SET_ERROR", `Failed to fetch product with ID ${productId}`);
        commit("SET_SELECTED_PRODUCT", null);
      } finally {
        commit("SET_LOADING", false);
      }
    },
    setCurrentPage({ commit, dispatch }, page: number) {
      commit("SET_CURRENT_PAGE", page);
      dispatch("fetchProducts");
    },
    setPageSize({ commit, dispatch }, size: number) {
      commit("SET_PAGE_SIZE", size);
      commit("SET_CURRENT_PAGE", 1);
      dispatch("fetchProducts");
    },
    updatePaginatedProducts({ state, commit }) {
      const pageSize = Number(state.pageSize);
      const currentPage = Number(state.currentPage);

      const start = (currentPage - 1) * pageSize;
      const end = start + pageSize;

      const paginated = state.products.slice(start, end);

      commit("SET_PAGINATED_PRODUCTS", paginated);
    },
  },
  getters: {
    paginatedProducts(state): Product[] {
      return state.paginatedProducts;
    },
    selectedProduct(state): Product | null {
      return state.selectedProduct;
    },
    isLoading(state) {
      return state.loading;
    },
    error(state) {
      return state.error;
    },
    currentPage(state) {
      return state.currentPage;
    },
    pageSize(state) {
      return state.pageSize;
    },
    totalRows(state) {
      return state.totalRows;
    },
    totalPages(state) {
      return state.totalPages;
    },
  },
};

export default productsModule;
