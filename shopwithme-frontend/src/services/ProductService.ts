import apiClient from "@/plugins/shopAxios";

export default {
  getProducts() {
    return apiClient.get("/products");
  },
};
