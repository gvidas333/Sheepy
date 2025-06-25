import { ref } from 'vue'
import { defineStore } from 'pinia'
import apiClient from '@/services/api';

export interface Product {
  id: string;
  name: string;
  description: string;
}

export const useProductsStore = defineStore('products', () => {
  const products = ref<Product[]>([]);
  const API_ENDPOINT = '/products';

  async function fetchProducts() {
    try {
      const response = await apiClient.get<Product[]>(API_ENDPOINT);
      products.value = response.data
    } catch (error) {
      console.error('Failed to fetch products:', error)
    }
  }

  return {
    products,
    fetchProducts
  }
})
