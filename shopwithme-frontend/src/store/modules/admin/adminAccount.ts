import { Module } from "vuex";
import { accountAPI } from "@/plugins/adminAxios";
import router from "@/router";

export interface AdminAccountState {
  loggedIn: boolean;
  adminLoggedIn: boolean;
  error: string | null;
  adminUser: any | null;
}

const adminAccountModule: Module<AdminAccountState, any> = {
  namespaced: true,

  state: {
    loggedIn: JSON.parse(sessionStorage.getItem("loggedIn") || "false"),
    adminLoggedIn: JSON.parse(
      sessionStorage.getItem("adminLoggedIn") || "false"
    ),
    error: null,
    adminUser: null,
  },

  mutations: {
    SET_LOGGED_IN(state, { status }: { userType: string; status: boolean }) {
      state.adminLoggedIn = status;
      sessionStorage.setItem("adminLoggedIn", JSON.stringify(status));
      state.loggedIn = status;
      sessionStorage.setItem("loggedIn", JSON.stringify(status));
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_ADMIN_USER(state, user: any) {
      state.adminUser = user;
    },
  },

  actions: {
    async login(
      { commit },
      {
        username,
        password,
        userType,
      }: { username: string; password: string; userType: string }
    ) {
      commit("SET_ERROR", null);
      try {
        await accountAPI.login({ name: username, password });
        commit("SET_LOGGED_IN", { userType, status: true });
      } catch (error: any) {
        console.error("Error logging in:", error);
        commit("SET_ERROR", "Login failed. Please check your credentials.");
        throw error;
      }
    },

    async logout({ commit }, userType: string) {
      try {
        await accountAPI.logout();
        commit("SET_LOGGED_IN", { userType, status: false });
        commit("SET_ADMIN_USER", null);
        router.push("/admin/login");
      } catch (error) {
        console.error("Error logging out:", error);
        commit("SET_ERROR", "Logout failed.");
      }
    },

    async fetchAdminUser({ commit }) {
      commit("SET_ERROR", null);
      try {
        const response = await accountAPI.getAdminUser();
        commit("SET_ADMIN_USER", response.data);
      } catch (error) {
        console.error("Error fetching admin user data:", error);
        commit("SET_ERROR", "Failed to fetch admin user data.");
      }
    },
  },

  getters: {
    isLoggedIn(state): boolean {
      return state.loggedIn;
    },
    isAdminLoggedIn(state): boolean {
      return state.adminLoggedIn;
    },
    accountError(state): string | null {
      return state.error;
    },
    adminUser(state): any | null {
      return state.adminUser;
    },
  },
};

export default adminAccountModule;
