import { defineStore } from 'pinia';
import apiClient from '@/services/api';

export const useShoppingListStore = defineStore('shoppingList', () => {
  const API_ENDPOINT = '/shoppinglists'

  async function generateList(mealIds: string[]) {
    try {
      await apiClient.post(`${API_ENDPOINT}/generate`, {mealIds});
    } catch (error) {
      console.error('Failed to generate shopping list:', error);
      throw error;
    }
  }

  return {
    generateList,
  };
});
