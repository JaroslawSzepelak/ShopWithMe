import { Module } from "vuex";
import adminProductsModule, { AdminProductState } from "./adminProducts";
import adminCategoriesModule, { AdminCategoryState } from "./adminCategories";
import adminAccountModule, { AdminAccountState } from "./adminAccount";
import adminOrdersModule from "./adminOrders";
import adminStorageModule, { AdminStorageState } from "./adminStorage";
import adminUsersModule from "./adminUsers";

interface AdminState {
  adminProducts: AdminProductState;
  adminCategories: AdminCategoryState;
  adminAccount: AdminAccountState;
  adminStorage: AdminStorageState;
}

const adminModule: Module<AdminState, any> = {
  namespaced: true,
  modules: {
    adminProducts: adminProductsModule,
    adminCategories: adminCategoriesModule,
    adminAccount: adminAccountModule,
    adminOrders: adminOrdersModule,
    adminStorage: adminStorageModule,
    adminUsers: adminUsersModule,
  },
};

export default adminModule;
