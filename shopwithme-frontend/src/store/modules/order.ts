import { orderAPI } from "@/plugins/shopAxios";
import { Module } from "vuex";

interface OrderState {
  orders: any[];
  orderSummary: any | null;
  error: string | null;
  loading: boolean;
  selectedOrder: any | null;
  totalRows: number;
  totalPages: number;
  pageSize: number;
  pageIndex: number;
}

const orderModule: Module<OrderState, any> = {
  namespaced: true,

  state: {
    orders: [],
    orderSummary: null,
    error: null,
    loading: false,
    selectedOrder: null,
    totalRows: 0,
    totalPages: 0,
    pageSize: 10,
    pageIndex: 1,
  },

  mutations: {
    SET_ORDERS(state, orders) {
      state.orders = orders;
    },
    SET_ORDER_SUMMARY(state, summary) {
      state.orderSummary = summary;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_LOADING(state, loading: boolean) {
      state.loading = loading;
    },
    SET_SELECTED_ORDER(state, order) {
      state.selectedOrder = order;
    },
    SET_PAGER_INFO(state, pager) {
      state.totalRows = pager.totalRows;
      state.totalPages = pager.totalPages;
    },
    SET_PAGE_INDEX(state, pageIndex: number) {
      state.pageIndex = pageIndex;
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
    },
  },

  actions: {
    async fetchOrders({ commit, state }) {
      commit("SET_LOADING", true);
      try {
        const response = await orderAPI.getOrders(
          state.pageIndex,
          state.pageSize
        );
        const { result, pager } = response.data;
        commit("SET_ORDERS", result);
        commit("SET_PAGER_INFO", pager);
      } catch (error) {
        commit("SET_ERROR", "Nie udało się pobrać zamówień.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async fetchOrderSummary({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await orderAPI.getOrderSummary();
        commit("SET_ORDER_SUMMARY", response.data);
      } catch (error) {
        console.error("Error fetching order summary:", error);
        commit("SET_ERROR", "Failed to fetch order summary.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async fetchOrderById({ commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await orderAPI.getOrderById(id);
        commit("SET_SELECTED_ORDER", response.data);
      } catch (error) {
        console.error("Error fetching order by ID:", error);
        commit("SET_ERROR", "Failed to fetch order by ID.");
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
      await dispatch("fetchOrders");
    },
    async createOrder({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await orderAPI.createOrder();
        commit("SET_ORDER_SUMMARY", response.data);
      } catch (error) {
        console.error("Error creating order:", error);
        commit("SET_ERROR", "Failed to create order.");
        throw error;
      } finally {
        commit("SET_LOADING", false);
      }
    },
    async deleteOrder({ dispatch, commit }, id: number) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        await orderAPI.deleteOrder(id);
        await dispatch("fetchOrders");
      } catch (error) {
        console.error("Error deleting order:", error);
        commit("SET_ERROR", "Failed to delete order.");
      } finally {
        commit("SET_LOADING", false);
      }
    },
    selectOrder({ commit }, order) {
      commit("SET_SELECTED_ORDER", order);
    },
  },

  getters: {
    allOrders(state) {
      return state.orders;
    },
    orderSummary(state) {
      return state.orderSummary;
    },
    orderError(state) {
      return state.error;
    },
    isOrderLoading(state) {
      return state.loading;
    },
    selectedOrder: (state) => state.selectedOrder,
    totalPages: (state) => state.totalPages,
    totalRows: (state) => state.totalRows,
    pageIndex: (state) => state.pageIndex,
    pageSize: (state) => state.pageSize,
  },
};

export default orderModule;
