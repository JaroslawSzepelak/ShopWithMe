import { Module } from "vuex";
import adminProductsModule, { AdminProductState } from "./adminProducts";
import adminCategoriesModule, { AdminCategoryState } from "./adminCategories";
import adminAccountModule, { AdminAccountState } from "./adminAccount";
import adminOrdersModule from "./adminOrders";

interface AdminState {
  adminProducts: AdminProductState;
  adminCategories: AdminCategoryState;
  adminAccount: AdminAccountState;
}

const adminModule: Module<AdminState, any> = {
  namespaced: true,
  modules: {
    adminProducts: adminProductsModule,
    adminCategories: adminCategoriesModule,
    adminAccount: adminAccountModule,
    adminOrders: adminOrdersModule,
  },
};

export default adminModule;
