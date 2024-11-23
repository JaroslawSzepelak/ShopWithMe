const state = {
  deliveryMethod: null,
  deliveryCost: 0,
};

const mutations = {
  SET_DELIVERY_METHOD(
    state: any,
    { method, cost }: { method: string; cost: number }
  ) {
    state.deliveryMethod = method;
    state.deliveryCost = cost;
    sessionStorage.setItem(
      "deliveryMethod",
      JSON.stringify({ method: state.deliveryMethod, cost: state.deliveryCost })
    );
  },
  LOAD_FROM_STORAGE(state: any) {
    const storedData = sessionStorage.getItem("deliveryMethod");
    if (storedData) {
      const { method, cost } = JSON.parse(storedData);
      state.deliveryMethod = method;
      state.deliveryCost = cost;
    }
  },
};

const actions = {
  setDeliveryMethod(
    { commit }: any,
    { method, cost }: { method: string; cost: number }
  ) {
    commit("SET_DELIVERY_METHOD", { method, cost });
  },
  fetchDeliveryMethod({ commit }: any) {
    commit("LOAD_FROM_STORAGE");
  },
};

const getters = {
  deliveryMethod: (state: any) => state.deliveryMethod,
  deliveryCost: (state: any) => state.deliveryCost,
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};
