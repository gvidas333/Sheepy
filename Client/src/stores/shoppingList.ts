import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import apiClient from '@/services/api';
import { useRouter } from "vue-router";
import { useProductsStore } from './products';

export interface ShoppingListItem {
  productId: string;
  productName: string;
  quantity: number;
  isChecked: boolean;
}

export interface ShoppingList {
  id: string;
  name: string;
  createdAt: string;
  items: ShoppingListItem[];
}

export type GroupedShoppingList = Record<string, ShoppingListItem[]>;
export const useShoppingListStore = defineStore('shoppingList', () => {
  const API_ENDPOINT = '/shoppinglists';
  const router = useRouter();
  const productsStore = useProductsStore();

  const selectedMealIds = ref<string[]>([]);
  const selectedProductIds = ref<string[]>([]);
  const hasSelection = computed(() => selectedMealIds.value.length > 0 || selectedProductIds.value.length > 0);
  const currentList = ref<ShoppingList | null>(null);

  const itemsGroupedByCategory = computed(() => {
    if (!currentList.value) return {};

    const grouped: Record<string, ShoppingListItem[]> = {};

    for (const item of currentList.value.items) {
      const fullProduct = productsStore.products.find(p => p.id === item.productId);
      const categoryName = fullProduct?.categoryName || 'Uncategorized';

      if (!grouped[categoryName]) {
        grouped[categoryName] = [];
      }
      grouped[categoryName].push(item);
    }
    return grouped;
  });

  async function fetchMostRecentList() {
    try {
      const response = await apiClient.get<GroupedShoppingList>(`${API_ENDPOINT}/latest`);
      currentList.value = response.data || {};
    } catch (error) {
      console.error('Failed to fetch shopping list:', error);
      currentList.value = null;
    }
  }

  function toggleMealSelection(mealId: string) {
    const index = selectedMealIds.value.indexOf(mealId);
    if (index > -1) {
      selectedMealIds.value.splice(index, 1);
    } else {
      selectedMealIds.value.push(mealId);
    }
  }

  function toggleProductSelection(productId: string) {
    const index = selectedProductIds.value.indexOf(productId);
    if (index > -1) {
      selectedProductIds.value.splice(index, 1);
    } else {
      selectedProductIds.value.push(productId);
    }
  }

  async function generateList() {
    if (!hasSelection.value) return;

    try {
      const response = await apiClient.post(`${API_ENDPOINT}/generate`, {
        mealIds: selectedMealIds.value,
        productIds: selectedProductIds.value,
        listName: 'New Shopping List'
      });
      currentList.value = response.data || null;
      selectedMealIds.value = [];
      selectedProductIds.value = [];

      router.push({ name: 'shopping-list'});
    } catch (error) {
      console.error('Failed to generate shopping list:', error);
      throw error;
    }
  }

  function toggleItemCompleted(itemToToggle: ShoppingListItem) {
    if (!currentList.value) return;
    const item = currentList.value.items.find(i => i.productId === itemToToggle.productId);
    if (item) {
      item.isChecked = !item.isChecked;
    }
  }

  return {
    currentList,
    selectedMealIds,
    selectedProductIds,
    hasSelection,
    itemsGroupedByCategory,
    fetchMostRecentList,
    toggleMealSelection,
    toggleProductSelection,
    generateList,
    toggleItemCompleted
  };
});
