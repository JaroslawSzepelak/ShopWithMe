import axios, { AxiosProgressEvent } from "axios";

const adminAxios = axios.create({
  baseURL: "http://localhost:5000/api/admin",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

// API dla produktów
export const productAPI = {
  getProduct(id: number) {
    return adminAxios.get(`/Products/${id}`);
  },
  getProducts(params: { pageIndex: number; pageSize: number }) {
    return adminAxios.get("/Products", {
      params: {
        pageIndex: params.pageIndex,
        pageSize: params.pageSize,
      },
    });
  },
  getProductsAutocomplete(search: string) {
    return adminAxios.get("/Products/get-autocomplete", {
      params: { search },
    });
  },

  createProduct(product: {
    name: string;
    lead: string;
    description: string;
    price: number;
    categoryId?: number;
    technicalData?: string;
    mainImageId?: number;
    salePrice?: number;
    dateSaleFrom?: string;
    dateSaleTo?: string;
    isSaleOn?: boolean;
    images?: { id: number }[];
  }) {
    const formattedTechnicalData = product.technicalData
      ? JSON.stringify(JSON.parse(product.technicalData))
      : null;

    return adminAxios.post("/Products", {
      ...product,
      technicalData: formattedTechnicalData,
      mainImage: null,
    });
  },
  updateProduct(product: {
    id: number;
    name: string;
    lead: string;
    price: number;
    categoryId?: number;
    description?: string;
    technicalData?: string;
    mainImageId?: number;
    salePrice?: number;
    dateSaleFrom?: string;
    dateSaleTo?: string;
    isSaleOn?: boolean;
    images?: { id: number }[];
  }) {
    const formattedTechnicalData = product.technicalData
      ? JSON.stringify(JSON.parse(product.technicalData))
      : null;

    const payload = {
      ...product,
      technicalData: formattedTechnicalData,
      mainImage: null,
    };

    return adminAxios
      .put("/Products", payload)
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        console.error("Wystąpił błąd podczas wysyłania żądania PUT.");
        if (error.response) {
          console.error("Kod statusu:", error.response.status);
          console.error("Treść odpowiedzi błędu:", error.response.data);
        } else if (error.request) {
          console.error("Brak odpowiedzi serwera. Żądanie:", error.request);
        } else {
          console.error("Błąd:", error.message);
        }
        throw error;
      });
  },

  deleteProduct(id: number) {
    return adminAxios.delete(`/Products/${id}`);
  },

  addImage(productId: number, imageId: number) {
    return adminAxios.post(`/Products/${productId}/images`, imageId);
  },

  removeImage(productId: number, imageId: number) {
    return adminAxios.delete(`/Products/${productId}/images/${imageId}`);
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories(pageIndex: number, pageSize: number, includeProducts = false) {
    return adminAxios.get("/ProductCategories", {
      params: {
        pageIndex,
        pageSize,
        includeProducts,
      },
    });
  },

  getAllCategories(withProducts = false) {
    return adminAxios.get("/ProductCategories/all", {
      params: { withProducts },
    });
  },
  getCategory(id: number) {
    return adminAxios.get(`/ProductCategories/${id}`);
  },
  createCategory(category: { name: string }) {
    return adminAxios
      .post("/ProductCategories", { name: category.name })
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.error(
          "Błąd podczas tworzenia kategorii:",
          error.response?.data || error.message
        );
        throw error;
      });
  },
  updateCategory(category: { id: number; name: string }) {
    return adminAxios.put("/ProductCategories", {
      id: category.id,
      name: category.name,
    });
  },
  deleteCategory(id: number) {
    return adminAxios.delete(`/ProductCategories/${id}`);
  },
};

// API dla zamówień
export const orderAPI = {
  getOrders(pageIndex: number, pageSize: number) {
    return adminAxios.get("/Orders", {
      params: { pageIndex, pageSize },
    });
  },
  getOrder(id: number) {
    return adminAxios.get(`/Orders/${id}`);
  },
  updateOrder(order: {
    id: number;
    cartLines: any[];
    contactData: any;
    totalAmount: number;
    status: number;
  }) {
    return adminAxios.put("/Orders", order);
  },
  addCartLine(cartLine: { productId: number; quantity: number }) {
    return adminAxios.post("/Orders/cart-line", cartLine);
  },
  removeCartLine(cartLine: { productId: number; orderId: number }) {
    return adminAxios.delete("/Orders/cart-line", {
      data: cartLine,
    });
  },
};

// API dla kont administratora
export const accountAPI = {
  login(credentials: { name: string; password: string }) {
    return adminAxios.post("/Account/login", credentials);
  },
  logout() {
    return adminAxios.post("/Account/logout");
  },
  getAdminUser() {
    return adminAxios.get("Account/get-user");
  },
  getUsers(pageIndex: number, pageSize: number) {
    return adminAxios.get("/Account", {
      params: { pageIndex, pageSize },
    });
  },

  createUser(user: {
    userName: string;
    email: string;
    password: string;
    repeatPassword: string;
  }) {
    return adminAxios.post("/Account", user);
  },

  updateUser(user: {
    id: string;
    userName: string;
    email: string;
    password?: string;
    repeatPassword?: string;
  }) {
    return adminAxios.put("/Account", user);
  },
};

// API dla zarządzania plikami w admin
export const storageAPI = {
  uploadFile(
    file: File,
    onUploadProgress?: (progressEvent: AxiosProgressEvent) => void
  ) {
    const formData = new FormData();
    formData.append("file", file);

    return adminAxios.post("/Storage", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      onUploadProgress,
    });
  },

  getFile(fileName: string) {
    return axios.get(`http://localhost:5000/api/Storage/${fileName}`, {
      responseType: "blob",
    });
  },
};

export default adminAxios;
