import { ref } from 'vue'
import { defineStore } from 'pinia'
import apiClient from '@/services/api';
import type { Product } from './products'

export interface MealProduct {
  productId: string;
  productName: string;
  quantity: number;
}

export interface Meal {
  id: string;
  name: string;
  description: string;
  products: MealProduct[];
}

export interface NewMealPayload {
  name: string;
  description: string;
  products: MealProduct[];
}

export const useMealsStore = defineStore('meals', () => {
  const meals = ref<Meal[]>([]);
  const API_ENDPOINT = '/meals';

  async function fetchMeals() {
    try {
      const response = await apiClient.get<Meal[]>(API_ENDPOINT);
      meals.value = response.data
    } catch (error) {
      console.error('Failed to fetch meals:', error)
    }
  }

  async function fetchMealById(mealId: string): Promise<Meal | null> {
    try {
      const response = await apiClient.get<Meal>(`${API_ENDPOINT}/${mealId}`);
      return response.data;
    } catch (error) {
      console.error(`Failed to fetch meal with id ${mealId}:`, error);
      return null;
    }
  }

  async function addMeal(newMeal: NewMealPayload) {
    try {
      await apiClient.post(API_ENDPOINT, newMeal);
      await fetchMeals()
    } catch (error) {
      console.error('Failed to add meal:', error)
      throw error
    }
  }

  async function updateMeal(mealId: string, payload: NewMealPayload) {
    try {
      await apiClient.put(`${API_ENDPOINT}/${mealId}`, payload);
      await fetchMeals();
    } catch (error) {
      console.error('Failed to update meal:', error);
      throw error;
    }
  }

  async function deleteMeal(mealId: string) {
    try {
      await apiClient.delete(`${API_ENDPOINT}/${mealId}`);
      await fetchMeals();
    } catch (error) {
      console.error('Failed to delete meal:', error);
      throw error;
    }
  }

  return {
    meals,
    fetchMeals,
    fetchMealById,
    addMeal,
    updateMeal,
    deleteMeal
  }
})
