import { orderAPI } from "@/plugins/shopAxios";
import { Module } from "vuex";

interface OrderState {
  orders: any[];
  orderSummary: any | null;
  error: string | null;
  loading: boolean;
}

const orderModule: Module<OrderState, any> = {
  namespaced: true,

  state: {
    orders: [],
    orderSummary: null,
    error: null,
    loading: false,
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
  },

  actions: {
    async fetchOrders({ commit }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);
      try {
        const response = await orderAPI.getOrders();
        commit("SET_ORDERS", response.data);
      } catch (error) {
        console.error("Error fetching orders:", error);
        commit("SET_ERROR", "Failed to fetch orders.");
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
  },
};

export default orderModule;
