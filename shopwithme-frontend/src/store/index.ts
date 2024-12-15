import Vue from "vue";
import Vuex from "vuex";
import cartModule from "./modules/cart";
import contactData from "@/store/modules/contactData";
import orderModule from "@/store/modules/order";
import paymentMethod from "@/store/modules/paymentMethod";
import deliveryMethods from "@/store/modules/deliveryMethods";
import categoryModule from "@/store/modules/categories";
import products from "./modules/products";
import account from "./modules/account";
import adminModule from "./modules/admin";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    products,
    cart: cartModule,
    categories: categoryModule,
    contactData,
    order: orderModule,
    paymentMethod,
    deliveryMethods,
    account,
    admin: adminModule,
  },
});
