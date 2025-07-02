<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useCategoriesStore, type Category } from '@/stores/categories';
import { useAuthStore } from '@/stores/auth';
import { useConfirm } from "primevue/useconfirm";
import { useToast } from 'primevue/usetoast';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Menu from 'primevue/menu';

const categoriesStore = useCategoriesStore();
const confirm = useConfirm();
const toast = useToast();
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

const isDialogVisible = ref(false);
const newCategoryName = ref('');

function toggleMainMenu(event: Event) {
  mainMenu.value.toggle(event);
}

function handleLogout() {
  authStore.clearToken();
  router.push('/login');
}

onMounted(() => {
  categoriesStore.fetchCategories();
});

async function handleAddCategory() {
  if (!newCategoryName.value) {
    toast.add({ severity: 'warn', summary: 'Warning', detail: 'Category name is required.', life: 3000 });
    return;
  }
  try {
    await categoriesStore.addCategory({ name: newCategoryName.value });
    isDialogVisible.value = false;
    newCategoryName.value = '';
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to create category.', life: 3000 });
  }
}

function confirmDelete(category: Category) {
  confirm.require({
    message: `Are you sure you want to delete the "${category.name}" category?`,
    header: 'Delete Confirmation',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-text',
    acceptLabel: 'Delete',
    rejectLabel: 'Cancel',
    accept: async () => {
      try {
        await categoriesStore.deleteCategory(category.id);
      } catch (error) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete category.', life: 3000 });
      }
    },
  });
}
</script>

<template>
  <div class="meals-layout">
    <header class="meals-header">
      <div class="header-left">
        <Button label="Create Category" icon="pi pi-plus" @click="isDialogVisible = true" />
      </div>
      <div class="header-right">
        <Menu ref="mainMenu" :model="mainMenuItems" :popup="true" />
        <Button icon="pi pi-bars" @click="toggleMainMenu" text rounded aria-label="Menu" />
      </div>
    </header>

    <main class="meals-content">
      <h1>Categories</h1>
      <div v-if="categoriesStore.categories.length > 0" class="categories-list">
        <div v-for="category in categoriesStore.categories" :key="category.id" class="category-item">
          <div class="category-info">
            <span class="category-name">{{ category.name }}</span>
          </div>
          <Button icon="pi pi-trash" @click="confirmDelete(category)" text rounded severity="danger" />
        </div>
      </div>
      <div v-else class="empty-message">
        <p>No categories found. Click "Create Category" to get started!</p>
      </div>
    </main>

    <Dialog v-model:visible="isDialogVisible" modal header="Create a New Category">
      <form @submit.prevent="handleAddCategory" class="dialog-form">
        <div class="form-group">
          <label for="categoryName">Category Name</label>
          <InputText id="categoryName" v-model="newCategoryName" autocomplete="off" />
        </div>
        <div class="dialog-footer">
          <Button label="Cancel" @click="isDialogVisible = false" text />
          <Button label="Create" type="submit" />
        </div>
      </form>
    </Dialog>
  </div>
</template>

<style scoped>
.meals-layout {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem;
  min-height: 100vh;
  padding-bottom: 100px;
}
.meals-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 1rem;
  margin-bottom: 2rem;
  border-bottom: 1px solid #444;
}
.header-left {
  display: flex;
  align-items: center;
}
.meals-content h1 {
  margin-bottom: 2rem;
  text-align: center;
  color: white;
}
.empty-message {
  text-align: center;
  color: #888;
  margin-top: 4rem;
}

.categories-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}
.category-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #1e1e1e;
  padding: 1rem 1.5rem;
  border-radius: 8px;
  transition: background-color 0.2s;
}
.category-item:hover {
  background-color: #2a2a2a;
}
.category-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}
.category-name {
  font-size: 1.1rem;
  font-weight: 500;
  color: white;
}
.dialog-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  margin-top: 1.5rem;
}
:deep(.p-dialog) {
  width: 90vw;
  max-width: 25rem;
}
</style>
