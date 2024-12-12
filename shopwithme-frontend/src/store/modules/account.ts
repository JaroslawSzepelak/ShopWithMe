import { Module } from "vuex";
import { accountAPI } from "@/plugins/accountAxios";

export interface AccountState {
  loggedIn: boolean;
  error: string | null;
  user: any | null;
}

const accountModule: Module<AccountState, any> = {
  namespaced: true,

  state: {
    loggedIn: JSON.parse(sessionStorage.getItem("loggedIn") || "false"),
    error: null,
    user: null,
  },

  mutations: {
    SET_LOGGED_IN(state, status: boolean) {
      state.loggedIn = status;
      sessionStorage.setItem("loggedIn", JSON.stringify(status));
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_USER(state, user: any) {
      state.user = user;
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
        await this.dispatch("account/fetchUser");
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
        commit("SET_USER", null);
      } catch (error) {
        console.error("Error logging out:", error);
        commit("SET_ERROR", "Logout failed.");
      }
    },

    async fetchUser({ commit }) {
      commit("SET_ERROR", null);
      try {
        const response = await accountAPI.getUser();
        commit("SET_USER", response.data);
      } catch (error) {
        console.error("Error fetching user data:", error);
        commit("SET_ERROR", "Failed to fetch user data.");
      }
    },

    async register(
      { commit },
      {
        userName,
        email,
        password,
        repeatPassword,
      }: {
        userName: string;
        email: string;
        password: string;
        repeatPassword: string;
      }
    ) {
      commit("SET_ERROR", null);
      try {
        await accountAPI.register({
          userName,
          email,
          password,
          repeatPassword,
        });
        commit("SET_LOGGED_IN", false);
      } catch (error: any) {
        console.error("Error registering user:", error);
        commit(
          "SET_ERROR",
          "Registration failed. Please check your input and try again."
        );
        throw error;
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
    currentUser(state) {
      return state.user;
    },
  },
};

export default accountModule;
