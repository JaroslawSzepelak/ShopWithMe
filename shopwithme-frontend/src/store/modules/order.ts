import { orderAPI } from "@/plugins/axios";
import { Module } from "vuex";

interface OrderState {
  orders: any[];
  orderSummary: any | null;
}

const orderModule: Module<OrderState, any> = {
  namespaced: true,

  state: {
    orders: [],
    orderSummary: null,
  },

  mutations: {
    SET_ORDERS(state, orders) {
      state.orders = orders;
    },
    SET_ORDER_SUMMARY(state, summary) {
      state.orderSummary = summary;
    },
  },

  actions: {
    async fetchOrders({ commit }) {
      const response = await orderAPI.getOrders();
      commit("SET_ORDERS", response.data);
    },
    async fetchOrderSummary({ commit }) {
      const response = await orderAPI.getOrderSummary();
      commit("SET_ORDER_SUMMARY", response.data);
    },
    async createOrder() {
      await orderAPI.createOrder();
    },
    async deleteOrder({ dispatch }, id: number) {
      await orderAPI.deleteOrder(id);
      dispatch("fetchOrders");
    },
  },
};

export default orderModule;
