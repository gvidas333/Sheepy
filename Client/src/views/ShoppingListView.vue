<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { useShoppingListStore, type ShoppingListItem } from '@/stores/shoppingList';
import { useAuthStore } from '@/stores/auth';
import Button from 'primevue/button';
import Menu from 'primevue/menu';

const shoppingListStore = useShoppingListStore();
const router = useRouter();
const authStore = useAuthStore();
const mainMenu = ref();
const mainMenuItems = ref([
  { label: 'Meals', icon: 'pi pi-book', command: () => router.push({ name: 'meals' }) },
  { label: 'Products', icon: 'pi pi-shopping-cart', command: () => router.push({ name: 'products' }) },
  { label: 'Categories', icon: 'pi pi-tags', command: () => router.push({ name: 'categories' }) },
  { label: 'Shopping List', icon: 'pi pi-list', command: () => router.push({ name: 'shopping-list' }) },
  { separator: true },
  { label: 'Log Out', icon: 'pi pi-sign-out', command: () => handleLogout() }
]);

const formattedDate = computed(() => {
  if (!shoppingListStore.currentList?.createdAt) return '';
  return new Date(shoppingListStore.currentList.createdAt).toLocaleDateString('en-US', {
    year: 'numeric', month: 'long', day: 'numeric'
  });
});

onMounted(() => {
  if (!shoppingListStore.currentList) {
    shoppingListStore.fetchMostRecentList();
  }
});

function toggleMainMenu(event: Event) { mainMenu.value.toggle(event); }
function handleLogout() { authStore.clearToken(); router.push('/login'); }
</script>

<template>
  <div class="meals-layout">
    <header class="meals-header">
      <div class="header-left">
        <h1 class="page-title">Shopping List</h1>
      </div>
      <div class="header-right">
        <Menu ref="mainMenu" :model="mainMenuItems" :popup="true" />
        <Button icon="pi pi-bars" @click="toggleMainMenu" text rounded aria-label="Menu" />
      </div>
    </header>

    <main class="meals-content">
      <div v-if="shoppingListStore.currentList" class="list-info">
        <span>Created on: {{ formattedDate }}</span>
      </div>

      <div v-if="shoppingListStore.currentList && Object.keys(shoppingListStore.itemsGroupedByCategory).length > 0">
        <div v-for="(items, categoryName) in shoppingListStore.itemsGroupedByCategory" :key="categoryName" class="category-group">
          <h2 class="category-header">{{ categoryName }}</h2>
          <div class="list-item"
               v-for="item in items"
               :key="item.productId"
               @click="shoppingListStore.toggleItemCompleted(item)"
               :class="{ 'completed': item.isChecked }"
          >
            <span>{{ item.productName }}</span>
            <strong>{{ item.quantity }}</strong>
          </div>
        </div>
      </div>

      <div v-else class="empty-message">
        <p>Your shopping list is empty.</p>
        <p>Go to the Meals or Products page to generate a new one.</p>
      </div>
    </main>
  </div>
</template>

<style scoped>
.meals-layout { max-width: 1000px; margin: 0 auto; padding: 1.5rem; }
.meals-header { display: flex; justify-content: space-between; align-items: center; padding-bottom: 1rem; margin-bottom: 2rem; border-bottom: 1px solid #444; }
.header-left { display: flex; align-items: center; }
.page-title { color: white; font-size: 1.75rem; margin: 0; }
.empty-message { text-align: center; color: #888; margin-top: 4rem; line-height: 1.6; }
.category-group { margin-bottom: 2rem; }
.category-header { font-size: 1.25rem; color: #ccc; margin-bottom: 1rem; padding-bottom: 0.5rem; border-bottom: 1px solid #333; }
.list-item {
  display: flex;
  justify-content: space-between;
  padding: 1rem;
  background-color: #1e1e1e;
  border-radius: 8px;
  margin-bottom: 0.5rem;
  cursor: pointer;
  transition: all 0.2s;
  color: white;
}
.list-item:hover {
  background-color: #2a2a2a;
}
.list-item.completed {
  color: #666;
  background-color: #161616;
  text-decoration: line-through;
}
.list-item.completed strong {
  color: #666;
}
.list-info {
  margin-bottom: 2rem;
  color: #aaa;
  font-style: italic;
}

</style>
