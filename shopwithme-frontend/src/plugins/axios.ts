import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5000/api",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const productAPI = {
  getProduct(id: number) {
    return apiClient.get(`/Products/${id}`);
  },
  getProducts() {
    return apiClient.get("/Products");
  },
};

export const cartAPI = {
  getCart() {
    return apiClient.get("/Cart");
  },
  addItemToCart(productId: number) {
    return apiClient
      .post("/Cart", productId)
      .then((response) => {
        console.log("Odpowiedź na żądanie POST /Cart:", response);
        return response;
      })
      .catch((error) => {
        throw error;
      });
  },
  removeItemFromCart(productId: number) {
    console.log("Wysyłanie żądania DELETE do /Cart z productId:", productId);
    return apiClient
      .delete("/Cart", {
        data: productId,
      })
      .then((response) => {
        console.log("Odpowiedź na żądanie DELETE /Cart:", response);
        return response;
      })
      .catch((error) => {
        console.error("Błąd podczas wysyłania żądania DELETE /Cart:", error);
        throw error;
      });
  },
};

export default apiClient;
