import axios from "axios";

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
  createProduct(product: {
    name: string;
    lead: string;
    description: string;
    price: number;
    categoryId: number;
    technicalData?: string;
  }) {
    const formattedTechnicalData = product.technicalData
      ? JSON.stringify(JSON.parse(product.technicalData))
      : null;

    return adminAxios.post("/Products", {
      name: product.name,
      lead: product.lead,
      description: product.description,
      price: product.price,
      categoryId: product.categoryId,
      technicalData: formattedTechnicalData,
    });
  },
  updateProduct(product: {
    id: number;
    name: string;
    lead: string;
    price: number;
    categoryId: number;
    description?: string;
    technicalData?: string;
  }) {
    // Konwersja danych technicznych do JSON string
    const formattedTechnicalData = product.technicalData
      ? JSON.stringify(JSON.parse(product.technicalData))
      : null;

    return adminAxios.put("/Products", {
      id: product.id,
      name: product.name,
      lead: product.lead,
      price: product.price,
      categoryId: product.categoryId,
      description: product.description,
      technicalData: formattedTechnicalData,
    });
  },
  deleteProduct(id: number) {
    return adminAxios.delete(`/Products/${id}`);
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories(params?: { pageIndex?: number; pageSize?: number }) {
    if (params) {
      return adminAxios.get("/ProductCategories", {
        params: {
          pageIndex: params.pageIndex || 1,
          pageSize: params.pageSize || 10,
        },
      });
    }
    return adminAxios.get("/ProductCategories/all");
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
};

export default adminAxios;
