import apiClient from "@/plugins/axios";

export default {
  getProducts() {
    return apiClient.get("/products");
  },
};
