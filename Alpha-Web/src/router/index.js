import { createRouter, createWebHistory } from 'vue-router';
import LoginComponent from '../components/LoginComponent.vue';
import HomeComponent from '../components/HomeComponent.vue';
import ProductRegisterComponent from '../components/ProductRegisterComponent.vue';

const routes = [
  {
    path: '/',
    redirect: '/login',
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginComponent,
  },
  {
    path: '/home',
    name: 'Home',
    component: HomeComponent,
  },
  {
    path: '/cadastro',
    name: 'Cadastro',
    component: ProductRegisterComponent,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
