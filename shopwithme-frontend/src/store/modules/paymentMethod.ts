import { Module, Commit } from "vuex";

interface PaymentMethodState {
  selectedMethod: string | null;
  cardNumber: string;
  accountNumber: string;
}

const state: PaymentMethodState = {
  selectedMethod: null,
  cardNumber: "",
  accountNumber: "",
};

const mutations = {
  SET_PAYMENT_METHOD(
    state: PaymentMethodState,
    payload: Partial<PaymentMethodState>
  ) {
    if (payload.selectedMethod !== undefined) {
      state.selectedMethod = payload.selectedMethod;
    }
    if (payload.cardNumber !== undefined) {
      state.cardNumber = payload.cardNumber;
    }
    if (payload.accountNumber !== undefined) {
      state.accountNumber = payload.accountNumber;
    }
  },
};

const actions = {
  setPaymentMethod(
    { commit }: { commit: Commit },
    payload: Partial<PaymentMethodState>
  ): void {
    commit("SET_PAYMENT_METHOD", payload);
    sessionStorage.setItem("paymentMethod", JSON.stringify(payload));
  },
  fetchPaymentMethod({ commit }: { commit: Commit }): void {
    const storedData = sessionStorage.getItem("paymentMethod");
    if (storedData) {
      commit("SET_PAYMENT_METHOD", JSON.parse(storedData));
    }
  },
};

const getters = {
  getPaymentMethod: (state: PaymentMethodState): string | null =>
    state.selectedMethod,
  getCardNumber: (state: PaymentMethodState): string => state.cardNumber,
  getAccountNumber: (state: PaymentMethodState): string => state.accountNumber,
};

const paymentMethodModule: Module<PaymentMethodState, any> = {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};

export default paymentMethodModule;
