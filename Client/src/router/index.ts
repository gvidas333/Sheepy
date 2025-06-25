import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import MealsView from '../views/MealsView.vue'
import AddMealView from '../views/AddMealView.vue'
import CategoriesView from '../views/CategoriesView.vue'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/login'
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    },
    {
      path: '/meals',
      name: 'meals',
      component: MealsView,
      meta: { requiresAuth: true}
    },
    // {
    //   path: 'products',
    //   name: 'products',
    //   component: () => import('../views/ProductsView.vue'),
    //   meta: { requiresAuth: true }
    // },
    {
      path: '/categories',
      name: 'categories',
      component: CategoriesView,
      meta: { requiresAuth: true }
    },
    // {
    //   path: '/shopping-list',
    //   name: 'shopping-list',
    //   // You will need to create this Vue component file later
    //   component: () => import('../views/ShoppingListView.vue'),
    //   meta: { requiresAuth: true }
    // },
    {
      path: '/meals/new',
      name: 'add-meal',
      component: AddMealView,
      meta: {requiresAuth: true}
    },
    {
      path: '/meals/:id/edit',
      name: 'edit-meal',
      component: AddMealView,
      meta: {requiresAuth: true}
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ name: 'login' });
  } else {
    next();
  }
});

export default router
