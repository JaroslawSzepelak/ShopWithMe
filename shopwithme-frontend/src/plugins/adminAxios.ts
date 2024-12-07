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
  getProducts() {
    return adminAxios.get("/Products");
  },
  createProduct(product: {
    name: string;
    lead: string;
    description: string;
    price: number;
    categoryId: number;
  }) {
    return adminAxios.post("/Products", {
      name: product.name,
      lead: product.lead,
      description: product.description,
      price: product.price,
      categoryId: product.categoryId,
    });
  },
  updateProduct(product: {
    id: number;
    name: string;
    price: number;
    categoryId: number;
    description?: string;
    image?: string;
  }) {
    return adminAxios.put("/Products", product);
  },
  deleteProduct(id: number) {
    return adminAxios.delete(`/Products/${id}`);
  },
};

// API dla kategorii produktów
export const categoryAPI = {
  getCategories() {
    return adminAxios.get("/Productcategories");
  },
  getCategory(id: number) {
    return adminAxios.get(`/Productcategories/${id}`);
  },
  createCategory(name: string) {
    return adminAxios.post("/Productcategories", { name });
  },
  updateCategory(id: number, name: string) {
    return adminAxios.put("/Productcategories", { id, name });
  },
  deleteCategory(id: number) {
    return adminAxios.delete(`/Productcategories/${id}`);
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
};

export default adminAxios;
