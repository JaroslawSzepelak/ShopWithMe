import axios from "axios";

const accountAxios = axios.create({
  baseURL: "http://localhost:5000/api/account",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const accountAPI = {
  getUser() {
    return accountAxios.get("/get-user");
  },

  login(credentials: { name: string; password: string }) {
    return accountAxios.post("/login", credentials);
  },

  logout() {
    return accountAxios.post("/logout");
  },

  register(registerData: {
    userName: string;
    email: string;
    password: string;
    repeatPassword: string;
  }) {
    return accountAxios.post("/register", registerData);
  },
};

export default accountAxios;
