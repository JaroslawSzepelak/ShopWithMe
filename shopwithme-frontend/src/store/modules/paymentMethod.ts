import { Module, Commit } from "vuex";

// Definicja stanu
interface PaymentMethodState {
  paymentMethod: string | null;
}

// Domyślny stan
const state: PaymentMethodState = {
  paymentMethod: null,
};

const mutations = {
  SET_PAYMENT_METHOD(state: PaymentMethodState, method: string) {
    state.paymentMethod = method;
  },
};

const actions = {
  setPaymentMethod({ commit }: { commit: Commit }, method: string): void {
    commit("SET_PAYMENT_METHOD", method);
  },
};

const getters = {
  getPaymentMethod: (state: PaymentMethodState): string | null =>
    state.paymentMethod,
};

const paymentMethodModule: Module<PaymentMethodState, any> = {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};

export default paymentMethodModule;
