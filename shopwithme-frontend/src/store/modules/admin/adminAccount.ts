import { Module } from "vuex";
import { accountAPI } from "@/plugins/adminAxios";

export interface AdminAccountState {
  loggedIn: boolean;
  adminLoggedIn: boolean;
  error: string | null;
}

const adminAccountModule: Module<AdminAccountState, any> = {
  namespaced: true,

  state: {
    loggedIn: JSON.parse(sessionStorage.getItem("loggedIn") || "false"),
    adminLoggedIn: JSON.parse(
      sessionStorage.getItem("adminLoggedIn") || "false"
    ),
    error: null,
  },

  mutations: {
    SET_LOGGED_IN(
      state,
      { userType, status }: { userType: string; status: boolean }
    ) {
      if (userType === "admin") {
        state.adminLoggedIn = status;
        sessionStorage.setItem("adminLoggedIn", JSON.stringify(status));
      }
      state.loggedIn = status;
      sessionStorage.setItem("loggedIn", JSON.stringify(status));
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
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
      } catch (error) {
        console.error("Error logging out:", error);
        commit("SET_ERROR", "Logout failed.");
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
  },
};

export default adminAccountModule;
