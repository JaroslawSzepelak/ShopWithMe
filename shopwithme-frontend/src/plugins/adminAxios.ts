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
    return adminAxios.get(`/products/${id}`);
  },
  getProducts() {
    return adminAxios.get("/products");
  },
  createProduct(product: {
    name: string;
    price: number;
    categoryId: number;
    description?: string;
    image?: string;
  }) {
    return adminAxios.post("/products", product);
  },
  updateProduct(product: {
    id: number;
    name: string;
    price: number;
    categoryId: number;
    description?: string;
    image?: string;
  }) {
    return adminAxios.put("/products", product);
  },
  deleteProduct(id: number) {
    return adminAxios.delete(`/products/${id}`);
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories() {
    return adminAxios.get("/productcategories");
  },
  getCategory(id: number) {
    return adminAxios.get(`/productcategories/${id}`);
  },
  createCategory(name: string) {
    return adminAxios.post("/productcategories", { name });
  },
  updateCategory(id: number, name: string) {
    return adminAxios.put("/productcategories", { id, name });
  },
  deleteCategory(id: number) {
    return adminAxios.delete(`/productcategories/${id}`);
  },
};

// API dla kont administratora
export const accountAPI = {
  login(credentials: { name: string; password: string }) {
    console.log("Próba logowania z danymi:", credentials);
    return adminAxios.post("/Account/login", credentials);
  },
  logout() {
    return adminAxios.post("/Account/logout");
  },
};

export default adminAxios;
