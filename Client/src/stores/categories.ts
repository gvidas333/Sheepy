import { ref } from 'vue'
import { defineStore } from 'pinia'
import apiClient from '@/services/api'

export interface Category {
  id: string;
  name: string;
}

export const useCategoriesStore = defineStore('categories', () => {
  const categories = ref<Category[]>([])
  const API_ENDPOINT = '/categorytypes'

  async function fetchCategories() {
    try {
      const response = await apiClient.get<Category[]>(API_ENDPOINT);
      categories.value = response.data
    } catch (error) {
      console.error('Failed to fetch categories:', error)
    }
  }

  async function addCategory(newCategory: { name: string }) {
    try {
      await apiClient.post(API_ENDPOINT, newCategory);
      await fetchCategories();
    } catch (error) {
      console.error('Failed to add category:', error);
      throw error;
    }
  }

  async function deleteCategory(categoryId: string) {
    try {
      await apiClient.delete(`${API_ENDPOINT}/${categoryId}`);
      await fetchCategories();
    } catch (error) {
      console.error('Failed to delete category:', error);
      throw error;
    }
  }

  return {
    categories,
    fetchCategories,
    addCategory,
    deleteCategory
  }
})
