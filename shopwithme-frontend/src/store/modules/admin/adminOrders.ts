import { Module } from "vuex";
import { orderAPI } from "@/plugins/adminAxios";

export interface AdminOrderState {
  orders: any[];
  selectedOrder: any | null;
  loading: boolean;
  error: string | null;
  totalRows: number;
  totalPages: number;
  pageSize: number;
  pageIndex: number;
}

const adminOrdersModule: Module<AdminOrderState, any> = {
  namespaced: true,

  state: {
    orders: [],
    selectedOrder: null,
    loading: false,
    error: null,
    totalRows: 0,
    totalPages: 0,
    pageSize: Number(sessionStorage.getItem("adminOrdersPageSize")) || 10,
    pageIndex: Number(sessionStorage.getItem("adminOrdersPageIndex")) || 1,
  },

  mutations: {
    SET_ORDERS(state, orders: any[]) {
      state.orders = orders;
    },
    SET_SELECTED_ORDER(state, order: any) {
      state.selectedOrder = order;
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
      sessionStorage.setItem("adminOrdersPageIndex", String(pageIndex));
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
      sessionStorage.setItem("adminOrdersPageSize", String(pageSize));
    },
  },

  actions: {
    async fetchOrders({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await orderAPI.getOrders(
          state.pageIndex,
          state.pageSize
        );
        const { result, pager } = response.data;

        commit("SET_ORDERS", result);
        commit("SET_PAGER_INFO", pager);
      } catch (error: any) {
        console.error("Error fetching orders:", error);
        commit("SET_ERROR", "Nie udało się pobrać listy zamówień.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    selectOrder({ commit }, order: any) {
      commit("SET_SELECTED_ORDER", order);
    },

    async fetchOrder({ commit }, id: number) {
      commit("SET_SELECTED_ORDER", null);
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await orderAPI.getOrder(id);
        commit("SET_SELECTED_ORDER", response.data);
      } catch (error: any) {
        console.error(`Error fetching order with ID ${id}:`, error);
        commit("SET_ERROR", "Nie udało się pobrać szczegółów zamówienia.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateOrder({ dispatch, commit }, order: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await orderAPI.updateOrder(order);
        await dispatch("fetchOrders");
      } catch (error) {
        console.error("Error updating order:", error);
        commit("SET_ERROR", "Nie udało się zaktualizować zamówienia.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateCartLine({ dispatch, commit }, cartLine: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await orderAPI.addCartLine(cartLine);
        await dispatch("fetchOrder", cartLine.orderId);
      } catch (error) {
        console.error("Error updating cart line:", error);
        commit("SET_ERROR", "Nie udało się zaktualizować linii koszyka.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async removeCartLine({ dispatch, commit }, cartLine: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await orderAPI.removeCartLine(cartLine);
        await dispatch("fetchOrder", cartLine.orderId);
      } catch (error) {
        console.error("Error removing cart line:", error);
        commit("SET_ERROR", "Nie udało się usunąć linii koszyka.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async changePage({ commit, dispatch }, pageIndex: number) {
      commit("SET_PAGE_INDEX", pageIndex);
      await dispatch("fetchOrders");
    },

    async changePageSize({ commit, dispatch }, pageSize: number) {
      commit("SET_PAGE_SIZE", pageSize);
      commit("SET_PAGE_INDEX", 1);
      await dispatch("fetchOrders");
    },
  },

  getters: {
    allOrders(state) {
      return state.orders;
    },
    selectedOrder(state) {
      return state.selectedOrder;
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

export default adminOrdersModule;
