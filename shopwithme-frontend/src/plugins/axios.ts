import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5000/api",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

// API dla produktów
export const productAPI = {
  getProduct(id: number) {
    return apiClient.get(`/Products/${id}`);
  },
  getProducts() {
    return apiClient.get("/Products");
  },
};

// API dla koszyka
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

// API dla kategorii produktów
export const categoryAPI = {
  getCategories() {
    return apiClient.get("/ProductCategories");
  },
  getCategory(id: number) {
    return apiClient.get(`/ProductCategories/${id}`);
  },
  createCategory(name: string) {
    return apiClient
      .post("/ProductCategories", { name })
      .then((response) => {
        console.log("Odpowiedź na żądanie POST /ProductCategories:", response);
        return response;
      })
      .catch((error) => {
        console.error("Błąd podczas tworzenia kategorii:", error);
        throw error;
      });
  },
  updateCategory(id: number, name: string) {
    return apiClient
      .put("/ProductCategories", { id, name })
      .then((response) => {
        console.log("Odpowiedź na żądanie PUT /ProductCategories:", response);
        return response;
      })
      .catch((error) => {
        console.error("Błąd podczas aktualizacji kategorii:", error);
        throw error;
      });
  },
  deleteCategory(id: number) {
    return apiClient
      .delete(`/ProductCategories/${id}`)
      .then((response) => {
        console.log(
          "Odpowiedź na żądanie DELETE /ProductCategories:",
          response
        );
        return response;
      })
      .catch((error) => {
        console.error("Błąd podczas usuwania kategorii:", error);
        throw error;
      });
  },
};

export default apiClient;
