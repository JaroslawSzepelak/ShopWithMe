import Vue from "vue";
import Vuex from "vuex";
import cartModule from "./modules/cart";
import contactData from "@/store/modules/contactData";
import order from "@/store/modules/order";
import paymentMethod from "@/store/modules/paymentMethod";
import deliveryMethods from "@/store/modules/deliveryMethods";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    cart: cartModule,
    contactData,
    order,
    paymentMethod,
    deliveryMethods,
  },
});
