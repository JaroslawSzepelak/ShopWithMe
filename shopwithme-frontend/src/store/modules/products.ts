import { Module } from "vuex";
import { productAPI, storageAPI } from "@/plugins/shopAxios";

interface Product {
  id: number;
  name: string;
  lead: string;
  description: string;
  price: number;
  salePrice?: number;
  dateSaleFrom?: Date;
  dateSaleTo?: Date;
  isSaleOn: boolean;
  categoryId: number | null;
  technicalData?: string;
  mainImage?: {
    id: number;
    name: string;
    path: string;
    contentType: string;
    url?: string;
  };
  images: Array<{
    id: number;
    name: string;
    path: string;
    contentType: string;
    url?: string;
  }>;
}

interface ProductsState {
  products: Product[];
  autocompleteProducts: any[];
  paginatedProducts: Product[];
  suggestedProducts: Product[];
  selectedProduct: Product | null;
  loading: boolean;
  error: string | null;
  currentPage: number;
  pageSize: number;
  suggestedPageSize: number;
  totalRows: number;
  totalPages: number;
  totalSuggestedRows: number;
  filters: {
    categoryId: number | null;
    search: string | null;
  };
}

const productsModule: Module<ProductsState, any> = {
  namespaced: true,
  state: {
    products: [],
    autocompleteProducts: [],
    paginatedProducts: [],
    suggestedProducts: [],
    selectedProduct: null,
    loading: false,
    error: null,
    currentPage: Number(sessionStorage.getItem("currentPage")) || 1,
    pageSize: Number(sessionStorage.getItem("pageSize")) || 10,
    suggestedPageSize: 5,
    totalSuggestedRows: 0,
    totalRows: 0,
    totalPages: 0,
    filters: {
      categoryId: sessionStorage.getItem("categoryId")
        ? Number(sessionStorage.getItem("categoryId"))
        : null,
      search: sessionStorage.getItem("search") || null,
    },
  },
  mutations: {
    SET_PRODUCTS(state, products: Product[]) {
      state.products = products;
    },
    SET_PAGINATED_PRODUCTS(state, products: Product[]) {
      state.paginatedProducts = products;
    },
    SET_AUTOCOMPLETE_PRODUCTS(state, products: any[]) {
      state.autocompleteProducts = products;
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
    SET_SUGGESTED_PRODUCTS(state, products: Product[]) {
      state.suggestedProducts = products;
    },
    SET_TOTAL_SUGGESTED_ROWS(state, total: number) {
      state.totalSuggestedRows = total;
    },
    SET_MAIN_IMAGE_URL(
      state,
      { productId, url }: { productId: number; url: string }
    ) {
      const product = state.products.find((p) => p.id === productId);
      if (product && product.mainImage) {
        product.mainImage.url = url;
      }
      if (
        state.selectedProduct?.id === productId &&
        state.selectedProduct.mainImage
      ) {
        state.selectedProduct.mainImage.url = url;
      }
    },
    SET_FILTERS(
      state,
      filters: { categoryId: number | null; search: string | null }
    ) {
      state.filters = filters;
      sessionStorage.setItem("categoryId", String(filters.categoryId || ""));
      sessionStorage.setItem("search", filters.search || "");
    },
  },
  actions: {
    async fetchProducts({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProducts(
          state.currentPage,
          state.pageSize,
          state.filters.categoryId,
          state.filters.search
        );
        const { result, pager } = response.data;

        const processedProducts = result.map((product: any) => ({
          ...product,
          mainImage: product.mainImage
            ? {
                id: product.mainImage.id,
                name: product.mainImage.name,
                path: product.mainImage.path,
                contentType: product.mainImage.contentType,
              }
            : null,
          images: product.images || [],
        }));

        commit("SET_PAGINATED_PRODUCTS", processedProducts);
        commit("SET_PAGER_INFO", {
          totalRows: pager.totalRows,
          totalPages: pager.totalPages,
        });

        await Promise.all(
          processedProducts.map(async (product) => {
            if (product.mainImage?.name) {
              try {
                const imageResponse = await storageAPI.getFile(
                  product.mainImage.name
                );
                const imageUrl = window.URL.createObjectURL(
                  new Blob([imageResponse.data])
                );
                commit("SET_MAIN_IMAGE_URL", {
                  productId: product.id,
                  url: imageUrl,
                });
              } catch (error) {
                console.error(
                  `Error fetching image for product ${product.id}:`,
                  error
                );
              }
            }
          })
        );
      } catch (error) {
        console.error("Error fetching products:", error);
        commit("SET_ERROR", "Failed to fetch products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async fetchAutocomplete({ commit }, search: string) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProductsAutocomplete(search);
        commit("SET_AUTOCOMPLETE_PRODUCTS", response.data);
        return response.data;
      } catch (error) {
        console.error("Error fetching autocomplete products:", error);
        commit("SET_ERROR", "Failed to fetch autocomplete products.");
        throw error;
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async fetchProduct({ commit }, productId: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProduct(productId);

        const product = response.data;
        const processedProduct: Product = {
          ...product,
          mainImage: product.mainImage
            ? {
                id: product.mainImage.id,
                name: product.mainImage.name,
                path: product.mainImage.path,
                contentType: product.mainImage.contentType,
              }
            : null,
          images: product.images || [],
        };

        commit("SET_SELECTED_PRODUCT", processedProduct);

        if (processedProduct.mainImage?.name) {
          const imageResponse = await storageAPI.getFile(
            processedProduct.mainImage.name
          );
          const imageUrl = window.URL.createObjectURL(
            new Blob([imageResponse.data])
          );
          commit("SET_MAIN_IMAGE_URL", { productId, url: imageUrl });
        }
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
    setFilters(
      { commit, dispatch },
      filters: { categoryId: number | null; search: string | null }
    ) {
      commit("SET_FILTERS", filters);
      dispatch("fetchProducts");
    },
    async fetchSuggestedProducts({ commit, state }, pageIndex: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await productAPI.getProducts(
          pageIndex,
          state.suggestedPageSize
        );
        const { result, pager } = response.data;

        const processedProducts = result.map((product: any) => ({
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

        commit("SET_SUGGESTED_PRODUCTS", processedProducts);
        commit("SET_TOTAL_SUGGESTED_ROWS", pager.totalRows);
      } catch (error) {
        console.error("Error fetching suggested products:", error);
        commit("SET_ERROR", "Failed to fetch suggested products.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async refreshSuggestedProducts({ dispatch }) {
      await dispatch("fetchSuggestedProducts", 1);
    },
  },
  getters: {
    paginatedProducts(state): Product[] {
      return state.paginatedProducts;
    },
    autocompleteProducts(state) {
      return state.autocompleteProducts || [];
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
    suggestedProducts(state): Product[] {
      return state.suggestedProducts;
    },
    suggestedPageSize(state) {
      return state.suggestedPageSize;
    },
    totalSuggestedPages(state): number {
      return Math.ceil(state.totalSuggestedRows / state.suggestedPageSize);
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
    filters(state) {
      return state.filters;
    },
  },
};

export default productsModule;
