import { Module } from "vuex";
import adminProductsModule, { AdminProductState } from "./adminProducts";
import adminCategoriesModule, { AdminCategoryState } from "./adminCategories";
import adminAccountModule, { AdminAccountState } from "./adminAccount";

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
  },
};

export default adminModule;
