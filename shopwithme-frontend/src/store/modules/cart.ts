import { cartAPI } from "@/plugins/axios";
import { Module } from "vuex";

export interface CartItem {
  productId: number;
  name: string;
  quantity: number;
  price: number;
}

interface CartState {
  items: CartItem[];
  total: number;
}

const cartModule: Module<CartState, any> = {
  namespaced: true,

  state: {
    items: [],
    total: 0,
  },

  getters: {
    cartItems: (state) => state.items,
    cartTotal: (state) => state.total,
  },

  mutations: {
    SET_CART(state, lines) {
      console.log("Przetwarzanie odpowiedzi lines:", lines);
      state.items = lines.map((line: any) => ({
        productId: line.product.id,
        name: line.product.name,
        quantity: line.quantity,
        price: line.product.price,
      }));
      state.total = lines.reduce(
        (sum: number, line: any) => sum + line.product.price * line.quantity,
        0
      );
    },
    UPDATE_ITEM_QUANTITY(state, { productId, quantity }) {
      const item = state.items.find((item) => item.productId === productId);
      if (item) {
        item.quantity = quantity;
        state.total = state.items.reduce(
          (sum, item) => sum + item.price * item.quantity,
          0
        );
      }
    },
  },

  actions: {
    async fetchCart({ commit }) {
      try {
        const response = await cartAPI.getCart();
        console.log("Odpowiedź z fetchCart:", response.data);
        commit("SET_CART", response.data.lines || []);
      } catch (error) {
        console.error("Błąd podczas pobierania koszyka:", error);
      }
    },
    async addItem({ dispatch }, productId: number) {
      try {
        console.log(
          "Dodawanie produktu do koszyka z przekazanym productId:",
          productId
        );
        await cartAPI.addItemToCart(productId);
        await dispatch("fetchCart");
      } catch (error) {
        console.error("Błąd podczas dodawania do koszyka:", error);
      }
    },
    async removeItem({ dispatch }, productId: number) {
      try {
        console.log(
          "Usuwanie produktu z koszyka o przekazanym productId:",
          productId
        );
        await cartAPI.removeItemFromCart(productId);
        await dispatch("fetchCart");
      } catch (error) {
        console.error("Błąd podczas usuwania z koszyka:", error);
      }
    },
    async updateItem({ commit }, { productId, quantity }) {
      try {
        console.log(
          "Aktualizacja ilości produktu:",
          productId,
          "do ilości:",
          quantity
        );
        commit("UPDATE_ITEM_QUANTITY", { productId, quantity });
      } catch (error) {
        console.error("Błąd podczas aktualizacji ilości w koszyku:", error);
      }
    },
  },
};

export default cartModule;
