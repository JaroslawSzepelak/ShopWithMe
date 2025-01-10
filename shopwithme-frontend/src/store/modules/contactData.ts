import { contactDataAPI } from "@/plugins/shopAxios";
import { Module } from "vuex";

interface ContactDataState {
  contactData: any | null;
}

const contactDataModule: Module<ContactDataState, any> = {
  namespaced: true,

  state: {
    contactData: null,
  },

  mutations: {
    SET_CONTACT_DATA(state, data) {
      state.contactData = data;
    },
    CLEAR_CONTACT_DATA(state) {
      state.contactData = null;
    },
  },

  actions: {
    async fetchContactData({ commit }) {
      const response = await contactDataAPI.getContactData();
      commit("SET_CONTACT_DATA", response.data);
    },
    async updateContactData({ commit }, data) {
      const response = await contactDataAPI.updateContactData(data);
      commit("SET_CONTACT_DATA", response.data);
    },
    async clearContactData({ commit }) {
      await contactDataAPI.clearContactData();
      commit("CLEAR_CONTACT_DATA");
    },
  },
};

export default contactDataModule;
