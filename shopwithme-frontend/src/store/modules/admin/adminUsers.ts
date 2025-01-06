import { Module } from "vuex";
import { accountAPI } from "@/plugins/adminAxios";

export interface AdminUserState {
  users: any[];
  selectedUser: any | null;
  loading: boolean;
  error: string | null;
  totalRows: number;
  totalPages: number;
  pageSize: number;
  pageIndex: number;
}

const adminUsersModule: Module<AdminUserState, any> = {
  namespaced: true,

  state: {
    users: [],
    selectedUser: null,
    loading: false,
    error: null,
    totalRows: 0,
    totalPages: 0,
    pageSize: Number(sessionStorage.getItem("adminUserPageSize")) || 10,
    pageIndex: Number(sessionStorage.getItem("adminUserPageIndex")) || 1,
  },

  mutations: {
    SET_USERS(state, users: any[]) {
      state.users = users;
    },
    SET_SELECTED_USER(state, user: any) {
      state.selectedUser = user;
    },
    SET_LOADING(state, loading: boolean) {
      state.loading = loading;
    },
    SET_ERROR(state, error: string | null) {
      state.error = error;
    },
    SET_PAGER_INFO(state, pager: { totalRows: number; totalPages: number }) {
      state.totalRows = pager.totalRows;
      state.totalPages = pager.totalPages;
    },
    SET_PAGE_INDEX(state, pageIndex: number) {
      state.pageIndex = pageIndex;
      sessionStorage.setItem("adminUserPageIndex", String(pageIndex));
    },
    SET_PAGE_SIZE(state, pageSize: number) {
      state.pageSize = pageSize;
      sessionStorage.setItem("adminUserPageSize", String(pageSize));
    },
  },

  actions: {
    async fetchUsers({ commit, state }) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        const response = await accountAPI.getUsers(
          state.pageIndex,
          state.pageSize
        );
        const { result, pager } = response.data;

        commit("SET_USERS", result);
        commit("SET_PAGER_INFO", pager);
      } catch (error: any) {
        console.error("Error fetching users:", error);
        commit("SET_ERROR", "Failed to fetch users.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async createUser({ dispatch, commit }, user: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await accountAPI.createUser(user);
        await dispatch("fetchUsers");
      } catch (error) {
        console.error("Error creating user:", error);
        commit("SET_ERROR", "Failed to create user.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async updateUser({ dispatch, commit }, user: any) {
      commit("SET_LOADING", true);
      commit("SET_ERROR", null);

      try {
        await accountAPI.updateUser(user);
        await dispatch("fetchUsers");
      } catch (error) {
        console.error("Error updating user:", error);
        commit("SET_ERROR", "Failed to update user.");
      } finally {
        commit("SET_LOADING", false);
      }
    },

    async changePage({ commit, dispatch }, pageIndex: number) {
      commit("SET_PAGE_INDEX", pageIndex);
      await dispatch("fetchUsers");
    },

    async changePageSize({ commit, dispatch }, pageSize: number) {
      commit("SET_PAGE_SIZE", pageSize);
      commit("SET_PAGE_INDEX", 1);
      await dispatch("fetchUsers");
    },
  },

  getters: {
    allUsers(state) {
      return state.users || [];
    },
    selectedUser(state) {
      return state.selectedUser || {};
    },
    isLoading(state) {
      return state.loading;
    },
    error(state) {
      return state.error;
    },
    totalRows(state) {
      return state.totalRows;
    },
    totalPages(state) {
      return state.totalPages;
    },
    pageSize(state) {
      return state.pageSize;
    },
    pageIndex(state) {
      return state.pageIndex;
    },
  },
};

export default adminUsersModule;
