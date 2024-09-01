import { createRouter, createWebHistory } from 'vue-router';
import LoginComponent from '../components/LoginComponent.vue';
import HomeComponent from '../components/HomeComponent.vue';
import ProductRegisterComponent from '../components/ProductRegisterComponent.vue';
import RegisterComponent from '../components/RegisterComponent.vue';
import DialogComponent from '../components/DialogComponent.vue';

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
    path: '/cadastro-usuario',
    name: 'Cadastro Usuario',
    component: RegisterComponent,
  },
  {
    path: '/home',
    name: 'Home',
    meta: { requiresAuth: true },
    component: HomeComponent,
  },
  {
    path: '/dialog',
    name: 'Dialog',
    component: DialogComponent,
  },
  {
    path: '/cadastro/:id?',
    name: 'Cadastro',
    meta: { requiresAuth: true },
    component: ProductRegisterComponent,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = !!localStorage.getItem('user');
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login');
  } else {
    next();
  }
});

export default router;
