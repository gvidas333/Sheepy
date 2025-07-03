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
  mealNames: string[];
  items: ShoppingListItem[];
}

export interface ShoppingListResponse {
  list: ShoppingList;
  categoryOrder: string[];
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
  const categoryOrder = ref<string[]>([]);
  const isLoading = ref(false);

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

  const orderedCategoryGroups = computed(() => {
    if (!categoryOrder.value || !Object.keys(itemsGroupedByCategory.value).length) {
      return [];
    }
    return categoryOrder.value.map(categoryName => ({
      name: categoryName,
      items: itemsGroupedByCategory.value[categoryName] || []
    })).filter(group => group.items.length > 0);
  });

  async function fetchMostRecentList() {
    isLoading.value = true;
    try {
      const response = await apiClient.get<ShoppingListResponse>(`${API_ENDPOINT}/latest`);

      if (response.data) {
        currentList.value = response.data.list;
        categoryOrder.value = response.data.categoryOrder;
      } else {
        currentList.value = null;
        categoryOrder.value = [];
      }
    } catch (error) {
      console.error('Failed to fetch shopping list:', error);
      currentList.value = null;
      categoryOrder.value = [];
    } finally {
      isLoading.value = false;
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
      const response = await apiClient.post<ShoppingListResponse>(`${API_ENDPOINT}/generate`, {
        mealIds: selectedMealIds.value,
        productIds: selectedProductIds.value,
        listName: 'New Shopping List'
      });

      if (response.data) {
        currentList.value = response.data.list;
        categoryOrder.value = response.data.categoryOrder;
      }

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

  async function updateCategoryOrder(newOrder: string[]) {
    try {
      categoryOrder.value = newOrder;
      await apiClient.put('/users/category-order', { categoryOrder: newOrder });
    } catch (error) {
      console.error('Failed to update category order:', error);
    }
  }

  return {
    isLoading,
    currentList,
    selectedMealIds,
    selectedProductIds,
    hasSelection,
    itemsGroupedByCategory,
    categoryOrder,
    orderedCategoryGroups,
    fetchMostRecentList,
    toggleMealSelection,
    toggleProductSelection,
    generateList,
    toggleItemCompleted,
    updateCategoryOrder
  };
});
