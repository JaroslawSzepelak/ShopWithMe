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
  getProducts(pageIndex: number, pageSize: number) {
    return apiClient.get(`/Products`, {
      params: {
        pageIndex,
        pageSize,
      },
    });
  },
  createProduct(product: {
    name: string;
    price: number;
    categoryId: number;
    description?: string;
    technicalData?: string;
    image?: string;
  }) {
    return apiClient.post("/Products", product);
  },
  updateProduct(product: {
    id: number;
    name: string;
    price: number;
    categoryId: number;
    description?: string;
    technicalData?: string;
    image?: string;
  }) {
    return apiClient.put("/Products", product);
  },
  deleteProduct(id: number) {
    return apiClient.delete(`/Products/${id}`);
  },
};

// API dla koszyka
export const cartAPI = {
  getCart() {
    return apiClient.get("/Cart");
  },
  addItemToCart(item: { productId: number; quantity: number }) {
    return apiClient
      .post("/Cart", item)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        throw error;
      });
  },
  removeItemFromCart(productId: number) {
    return apiClient
      .delete(`/Cart/${productId}`)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        throw error;
      });
  },
  updateItemInCart(item: { productId: number; quantity: number }) {
    return apiClient
      .put("/Cart", item)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        throw error;
      });
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories(pageIndex: number, pageSize: number) {
    return apiClient.get("/ProductCategories", {
      params: {
        pageIndex,
        pageSize,
      },
    });
  },
  getAllCategories() {
    return apiClient.get("/ProductCategories/all");
  },
  getCategory(id: number) {
    return apiClient.get(`/ProductCategories/${id}`);
  },
  createCategory(name: string) {
    return apiClient
      .post("/ProductCategories", { name })
      .then((response) => {
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

export const contactDataAPI = {
  getContactData() {
    return apiClient.get("/ContactData");
  },
  updateContactData(data: any) {
    return apiClient.put("/ContactData", data);
  },
  clearContactData() {
    return apiClient.delete("/ContactData");
  },
};

export const orderAPI = {
  getOrders() {
    return apiClient.get("/Orders");
  },
  createOrder() {
    return apiClient.post("/Orders");
  },
  getOrderSummary() {
    return apiClient.get("/Orders/summary");
  },
  deleteOrder(id: number) {
    return apiClient.delete(`/Orders/${id}`);
  },
};

export default apiClient;
