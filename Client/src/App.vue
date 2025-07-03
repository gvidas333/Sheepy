<script setup lang="ts">
import { RouterView } from 'vue-router';
import Toast from 'primevue/toast';
import ConfirmDialog from 'primevue/confirmdialog';
import { onMounted } from 'vue';
import { useAuthStore } from '@/stores/auth';
import { useProductsStore } from '@/stores/products';

const authStore = useAuthStore();
const productsStore = useProductsStore();

onMounted(() => {
  if (authStore.token) {
    productsStore.fetchProducts();
  }
});
</script>

<template>
  <RouterView />
  <Toast />
  <ConfirmDialog />
</template>

<style scoped>

</style>
