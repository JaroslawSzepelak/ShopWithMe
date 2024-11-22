import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
import "@fortawesome/fontawesome-free/css/all.css";
import "./assets/styles.scss";
import { BootstrapVue } from "bootstrap-vue";

Vue.use(BootstrapVue);
store.dispatch("cart/initializeCart");

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
