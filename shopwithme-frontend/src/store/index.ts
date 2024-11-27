import Vue from "vue";
import Vuex from "vuex";
import cartModule from "./modules/cart";
import contactData from "@/store/modules/contactData";
import order from "@/store/modules/order";
import paymentMethod from "@/store/modules/paymentMethod";
import deliveryMethods from "@/store/modules/deliveryMethods";
import categoryModule from "@/store/modules/categories";
import products from "./modules/products";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    products,
    cart: cartModule,
    categories: categoryModule,
    contactData,
    order,
    paymentMethod,
    deliveryMethods,
  },
});
