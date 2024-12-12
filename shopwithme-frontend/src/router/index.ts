import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";
import AdminLogin from "@/views/AdminLogin.vue";
import ProductList from "@/views/ProductList.vue";
import ProductDetails from "@/views/ProductDetails.vue";
import Cart from "@/views/Cart.vue";
import DeliveryForm from "@/views/DeliveryForm.vue";
import PaymentMethod from "@/views/PaymentMethod.vue";
import SummaryView from "@/views/SummaryView.vue";
import AdminPanel from "@/views/AdminPanel.vue";
import ProductEditor from "@/components/ProductEditor.vue";
import CategoryEditor from "@/components/CategoryEditor.vue";
import ProductAdmin from "@/components/ProductAdmin.vue";
import CategoryAdmin from "@/components/CategoryAdmin.vue";
import OrderAdmin from "@/components/OrderAdmin.vue";
import OrderDetailsAdmin from "@/components/OrderDetailsAdmin.vue";
import NotFound from "@/views/NotFound.vue";

Vue.use(Router);

const routes = [
  { path: "/", component: Home },
  { path: "/products", component: ProductList },
  { path: "*", component: NotFound },
  { path: "/admin/login", component: AdminLogin },
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
  {
    path: "/admin",
    redirect: "/admin/products",
    component: AdminPanel,
    children: [
      {
        path: "products",
        name: "ProductAdmin",
        component: ProductAdmin,
      },
      {
        path: "categories",
        name: "CategoryAdmin",
        component: CategoryAdmin,
      },
      {
        path: "orders",
        name: "OrderAdmin",
        component: OrderAdmin,
      },
      {
        path: "orders/details/:id",
        name: "OrderDetailsAdmin",
        component: OrderDetailsAdmin,
        props: true,
      },
      {
        path: "products/create",
        name: "ProductCreate",
        component: ProductEditor,
        props: { op: "create" },
      },
      {
        path: "products/edit/:id",
        name: "ProductEdit",
        component: ProductEditor,
        props: { op: "edit" },
      },
      {
        path: "categories/create",
        name: "CategoryCreate",
        component: CategoryEditor,
        props: { op: "create" },
      },
      {
        path: "categories/edit/:id",
        name: "CategoryEdit",
        component: CategoryEditor,
        props: { op: "edit" },
      },
    ],
  },
];

const router = new Router({
  mode: "history",
  routes,
});

export default router;
