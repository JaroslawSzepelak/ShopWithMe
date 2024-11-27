import { Module } from "vuex";
import { productAPI } from "@/plugins/axios";

interface Product {
  id: number;
  name: string;
  price: number;
  categoryId: number;
  description?: string;
  images: string[];
}

interface ProductsState {
  products: Product[];
  paginatedProducts: Product[];
  selectedProduct: Product | null;
  loading: boolean;
  error: string | null;
  defaultImages: string[];
  currentPage: number;
  pageSize: number;
}

const productsModule: Module<ProductsState, any> = {
  namespaced: true,
  state: {
    products: [],
    paginatedProducts: [],
    selectedProduct: null,
    loading: false,
    error: null,
    defaultImages: [
      "https://placehold.co/400/abb7e7/7b8277",
      "https://placehold.co/400/abd4c7/7b8277",
      "https://placehold.co/400/e6dcae/7b8277",
      "https://placehold.co/400",
    ],
    currentPage: Number(sessionStorage.getItem("currentPage")) || 1,
    pageSize: Number(sessionStorage.getItem("pageSize")) || 10,
  },
  mutations: {
    SET_PRODUCTS(state, products: Product[]) {
      state.products = products;
    },
    SET_PAGINATED_PRODUCTS(state, products: Product[]) {
      state.paginatedProducts = products;
      console.log(
        "SET_PAGINATED_PRODUCTS:",
        `Updated products: ${products.length} (Expected: ${state.pageSize})`
      );
    },
    SET_SELECTED_PRODUCT(state, product: Product) {
      state.selectedProduct = product;
    },
    SET_LOADING(state, loading: boolean) {
      state.loading = loading;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_CURRENT_PAGE(state, page: number) {
      state.currentPage = Number(page);
      sessionStorage.setItem("currentPage", String(page));
      console.log("SET_CURRENT_PAGE:", {
        currentPage: state.currentPage,
        type: typeof state.currentPage,
      });
    },
    SET_PAGE_SIZE(state, size: number) {
      state.pageSize = Number(size);
      sessionStorage.setItem("pageSize", String(size));
      console.log("SET_PAGE_SIZE:", {
        pageSize: state.pageSize,
        type: typeof state.pageSize,
      });
    },
  },
  actions: {
    async fetchProducts({ commit, dispatch }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProducts();
        console.log("Fetched products:", response.data);

        commit("SET_PRODUCTS", response.data);

        dispatch("updatePaginatedProducts");
      } catch (error) {
        console.error("Error fetching products:", error);
        commit("SET_ERROR", "Failed to fetch products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    setCurrentPage({ commit, dispatch }, page: number) {
      commit("SET_CURRENT_PAGE", page);
      dispatch("updatePaginatedProducts");
      console.log("Action setCurrentPage triggered:", page);
    },
    setPageSize({ commit, dispatch }, size: number) {
      commit("SET_PAGE_SIZE", size);
      commit("SET_CURRENT_PAGE", 1);
      dispatch("updatePaginatedProducts");
      console.log("Action setPageSize triggered:", size);
    },
    updatePaginatedProducts({ state, commit }) {
      const pageSize = Number(state.pageSize);
      const currentPage = Number(state.currentPage);

      const start = (currentPage - 1) * pageSize;
      const end = start + pageSize;

      console.log("updatePaginatedProducts:", {
        currentPage,
        pageSize,
        start,
        end,
      });

      const paginated = state.products.slice(start, end);

      console.log("Products sliced:", paginated);

      commit("SET_PAGINATED_PRODUCTS", paginated);
    },
  },
  getters: {
    allProducts(state) {
      return state.products;
    },
    paginatedProducts(state): Product[] {
      const start = (state.currentPage - 1) * state.pageSize;
      const end = start + state.pageSize;
      return state.products.slice(start, end);
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
    pageCount(state) {
      return Math.ceil(state.products.length / state.pageSize);
    },
  },
};

export default productsModule;
