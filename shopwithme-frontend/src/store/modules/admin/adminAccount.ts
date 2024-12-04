import { Module } from "vuex";
import { accountAPI } from "@/plugins/adminAxios";

export interface AdminAccountState {
  loggedIn: boolean;
  error: string | null;
}

const adminAccountModule: Module<AdminAccountState, any> = {
  namespaced: true,

  state: {
    loggedIn: JSON.parse(sessionStorage.getItem("loggedIn") || "false"),
    error: null,
  },

  mutations: {
    SET_LOGGED_IN(state, status: boolean) {
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
      { username, password }: { username: string; password: string }
    ) {
      commit("SET_ERROR", null);
      try {
        await accountAPI.login({ name: username, password });
        commit("SET_LOGGED_IN", true);
      } catch (error: any) {
        console.error("Error logging in:", error);
        commit("SET_ERROR", "Login failed. Please check your credentials.");
        throw error;
      }
    },

    async logout({ commit }) {
      try {
        await accountAPI.logout();
        commit("SET_LOGGED_IN", false);
      } catch (error) {
        console.error("Error logging out:", error);
        commit("SET_ERROR", "Logout failed.");
      }
    },
  },

  getters: {
    isLoggedIn(state) {
      return state.loggedIn;
    },
    accountError(state) {
      return state.error;
    },
  },
};

export default adminAccountModule;
