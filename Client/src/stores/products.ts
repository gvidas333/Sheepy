import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import apiClient from '@/services/api';

export interface Product {
  id: string;
  name: string;
  description: string;
  categoryTypeId: string;
  categoryName: string;
}

export interface NewProductPayload {
  name: string;
  categoryTypeId: string;
}

export const useProductsStore = defineStore('products', () => {
  const products = ref<Product[]>([]);
  const API_ENDPOINT = '/products';

  const productsByCategory = computed(() => {
    const grouped: { [key: string]: Product[] } = {};
    const sortedProducts = [...products.value].sort((a, b) => a.name.localeCompare(b.name));

    for (const product of sortedProducts) {
      const categoryName = product.categoryName || 'Uncategorized';
      if (!grouped[categoryName]) {
        grouped[categoryName] = [];
      }
      grouped[categoryName].push(product);
    }
    return grouped;
  });

  async function fetchProducts() {
    try {
      const response = await apiClient.get<Product[]>(API_ENDPOINT);
      products.value = response.data
    } catch (error) {
      console.error('Failed to fetch products:', error)
    }
  }

  async function addProduct(newProduct: NewProductPayload) {
    try {
      await apiClient.post(API_ENDPOINT, newProduct);
      await fetchProducts();
    } catch (error) {
      console.error('Failed to add product:', error);
      throw error;
    }
  }

  async function deleteProduct(productId: string) {
    try {
      await apiClient.delete(`${API_ENDPOINT}/${productId}`);
      await fetchProducts();
    } catch (error) {
      console.error('Failed to delete product:', error);
      throw error;
    }
  }

  return {
    products,
    productsByCategory,
    fetchProducts,
    addProduct,
    deleteProduct
  }
})
