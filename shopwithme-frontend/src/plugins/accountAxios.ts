import axios from "axios";

const accountAxios = axios.create({
  baseURL: "http://localhost:5000/api/Account",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const accountAPI = {
  async getUser() {
    return accountAxios.get("/get-user");
  },

  async login(credentials: { name: string; password: string }) {
    return accountAxios.post("/login", credentials);
  },

  async logout() {
    return accountAxios.post("/logout");
  },

  async register(registerData: {
    userName: string;
    email: string;
    password: string;
    repeatPassword: string;
  }) {
    try {
      return await accountAxios.post("/register", registerData);
    } catch (error: any) {
      if (error.response && error.response.status === 422) {
        throw error.response.data;
      }
      throw error;
    }
  },
};

export default accountAxios;
