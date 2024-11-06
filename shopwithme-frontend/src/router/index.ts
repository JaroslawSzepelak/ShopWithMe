import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";
import ProductList from "@/views/ProductList.vue";
import ProductDetails from "@/views/ProductDetails.vue";
import Cart from "@/views/Cart.vue";

Vue.use(Router);

const routes = [
  { path: "/", component: Home },
  { path: "/products", component: ProductList },
  {
    path: "/products/:id",
    name: "ProductDetails",
    component: ProductDetails,
    props: true,
  },
  { path: "/cart", component: Cart },
];

const router = new Router({
  mode: "history",
  routes,
});

export default router;
