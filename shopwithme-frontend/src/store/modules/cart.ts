import { cartAPI } from "@/plugins/shopAxios";
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
    isProductInCart: (state) => (productId: number) =>
      state.items.some((item) => item.productId === productId),
  },

  mutations: {
    SET_CART(state, lines) {
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
        if (response.data && response.data.lines) {
          commit("SET_CART", response.data.lines);
        } else {
          console.warn("API zwróciło pusty lub nieoczekiwany format danych.");
          commit("SET_CART", []);
        }
      } catch (error) {
        console.error("Błąd podczas pobierania koszyka z API:", error);
        commit("SET_CART", []);
      }
    },
    async initializeCart({ dispatch }) {
      try {
        await dispatch("fetchCart");
      } catch (error) {
        console.error("Błąd podczas inicjalizacji koszyka:", error);
      }
    },
    async addItem(
      { dispatch },
      { productId, quantity }: { productId: number; quantity: number }
    ) {
      try {
        await cartAPI.addItemToCart({ productId, quantity });
        await dispatch("fetchCart");
      } catch (error) {
        console.error("Błąd podczas dodawania do koszyka:", error);
      }
    },
    async removeItem({ dispatch }, productId: number) {
      try {
        await cartAPI.removeItemFromCart(productId);
        await dispatch("fetchCart");
      } catch (error) {
        console.error("Błąd podczas usuwania z koszyka:", error);
      }
    },
    async updateItem(
      { commit },
      { productId, quantity }: { productId: number; quantity: number }
    ) {
      try {
        await cartAPI.updateItemInCart({ productId, quantity });
        commit("UPDATE_ITEM_QUANTITY", { productId, quantity });
      } catch (error) {
        console.error("Błąd podczas aktualizacji ilości w koszyku:", error);
      }
    },
  },
};

export default cartModule;
