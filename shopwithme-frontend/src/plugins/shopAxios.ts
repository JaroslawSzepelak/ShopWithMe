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
      .delete("/Cart", {
        data: productId,
      })
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
  clearCart() {
    return apiClient.delete("/Cart/clear").then((response) => response);
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories(pageIndex: number, pageSize: number, includeProducts = false) {
    return apiClient.get("/ProductCategories", {
      params: {
        pageIndex,
        pageSize,
        includeProducts,
      },
    });
  },

  getAllCategories(withProducts = false) {
    return apiClient.get("/ProductCategories/all", {
      params: { withProducts },
    });
  },

  getCategory(id: number) {
    return apiClient.get(`/ProductCategories/${id}`);
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
  getOrders(pageIndex: number, pageSize: number) {
    return apiClient.get("/Orders", {
      params: { pageIndex, pageSize },
    });
  },
  getOrderById(id: number) {
    return apiClient.get(`/Orders/${id}`);
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
