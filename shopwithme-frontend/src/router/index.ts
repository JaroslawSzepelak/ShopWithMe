import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";
import ProductList from "@/views/ProductList.vue";
import ProductDetails from "@/views/ProductDetails.vue";
import Cart from "@/views/Cart.vue";
import DeliveryForm from "@/views/DeliveryForm.vue";
import PaymentMethod from "@/views/PaymentMethod.vue";
import SummaryView from "@/views/SummaryView.vue";

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
  {
    path: "/delivery-form",
    name: "DeliveryForm",
    component: DeliveryForm,
  },
  { path: "/payment-method", component: PaymentMethod },
  {
    path: "/summary",
    name: "SummaryView",
    component: SummaryView,
  },
];

const router = new Router({
  mode: "history",
  routes,
});

export default router;
