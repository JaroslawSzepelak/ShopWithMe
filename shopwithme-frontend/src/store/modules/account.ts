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
      { commit, dispatch },
      { username, password }: { username: string; password: string }
    ) {
      commit("SET_ERROR", null);
      try {
        await accountAPI.login({ name: username, password });
        commit("SET_LOGGED_IN", true);
        await dispatch("fetchUser");
      } catch (error: any) {
        console.error("Error logging in:", error);
        commit(
          "SET_ERROR",
          "Logowanie nie powiodło się. Sprawdź swoje dane uwierzytelniające."
        );
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
        commit("SET_ERROR", "Wylogowanie nie powiodło się");
      }
    },

    async fetchUser({ commit }) {
      commit("SET_ERROR", null);
      try {
        const response = await accountAPI.getUser();
        commit("SET_USER", {
          id: response.data.id,
          name: response.data.userName,
          email: response.data.email,
        });
      } catch (error) {
        console.error("Error fetching user data:", error);
        commit("SET_ERROR", "Nie udało się pobrać danych użytkownika.");
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
          error || "Rejestracja nie powiodła się. Spróbuj ponownie."
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
