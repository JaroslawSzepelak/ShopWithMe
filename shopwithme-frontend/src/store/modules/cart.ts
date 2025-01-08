import { cartAPI, storageAPI } from "@/plugins/shopAxios";
import { Module } from "vuex";

export interface CartItem {
  productId: number;
  name: string;
  quantity: number;
  price: number;
  salePrice: number;
  mainImageId?: number | null;
  image?: string | null;
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
        salePrice: line.product.salePrice,
        mainImageId: line.product.mainImageId || null,
        image: null,
      }));
      state.total = state.items.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0
      );
    },
    SET_PRODUCT_IMAGE(state, { productId, image }) {
      const item = state.items.find((item) => item.productId === productId);
      if (item) {
        item.image = image;
      }
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
    CLEAR_CART(state) {
      state.items = [];
      state.total = 0;
    },
  },

  actions: {
    async fetchCart({ commit }) {
      try {
        const response = await cartAPI.getCart();
        if (response.data && response.data.lines) {
          commit("SET_CART", response.data.lines);

          for (const line of response.data.lines) {
            if (line.product.mainImage?.name) {
              try {
                const imageResponse = await storageAPI.getFile(
                  line.product.mainImage.name
                );
                const imageUrl = URL.createObjectURL(
                  new Blob([imageResponse.data])
                );
                commit("SET_PRODUCT_IMAGE", {
                  productId: line.product.id,
                  image: imageUrl,
                });
              } catch (error) {
                console.error(
                  `Błąd podczas pobierania zdjęcia dla produktu ${line.product.id}:`,
                  error
                );
              }
            }
          }
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
    async clearCart({ commit }) {
      try {
        await cartAPI.clearCart();
        commit("CLEAR_CART");
      } catch (error) {
        console.error("Błąd podczas czyszczenia koszyka:", error);
      }
    },
  },
};

export default cartModule;
