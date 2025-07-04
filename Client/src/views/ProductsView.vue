<script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { useProductsStore, type Product } from '@/stores/products';
  import { useCategoriesStore } from '@/stores/categories';
  import { useShoppingListStore } from '@/stores/shoppingList';
  import { useAuthStore } from '@/stores/auth';
  import { useConfirm } from "primevue/useconfirm";
  import { useToast } from 'primevue/usetoast';
  import Button from 'primevue/button';
  import Dialog from 'primevue/dialog';
  import InputText from 'primevue/inputtext';
  import Select from 'primevue/select';
  import Menu from 'primevue/menu';

  const productsStore = useProductsStore();
  const categoriesStore = useCategoriesStore();
  const shoppingListStore = useShoppingListStore();
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
  function toggleMainMenu(event: Event) { mainMenu.value.toggle(event); }
  function handleLogout() { authStore.clearToken(); router.push('/login'); }

  async function handleGenerateList() {
    await shoppingListStore.generateList();
  }

  const isDialogVisible = ref(false);
  const newProductName = ref('');
  const selectedCategoryId = ref<string | null>(null);

  onMounted(() => {
    productsStore.fetchProducts();
    categoriesStore.fetchCategories();
  });

  async function handleAddProduct() {
    if (!newProductName.value || !selectedCategoryId.value) {
      toast.add({ severity: 'warn', summary: 'Warning', detail: 'Product name and category are required.', life: 3000 });
      return;
    }
    try {
      await productsStore.addProduct({
        name: newProductName.value,
        categoryTypeId: selectedCategoryId.value
      });
      isDialogVisible.value = false;
      newProductName.value = '';
      selectedCategoryId.value = null;
    } catch (error) {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to create product.', life: 3000 });
    }
  }

  function confirmDelete(product: Product) {
    confirm.require({
      message: `Are you sure you want to delete the product "${product.name}"?`,
      header: 'Delete Confirmation',
      icon: 'pi pi-exclamation-triangle',
      acceptClass: 'p-button-danger',
      rejectClass: 'p-button-text',
      acceptLabel: 'Delete',
      rejectLabel: 'Cancel',
      accept: async () => {
        try {
          await productsStore.deleteProduct(product.id);
        } catch (error) {
          toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete product.', life: 3000 });
        }
      },
    });
  }
</script>

<template>
  <div class="meals-layout">
    <header class="meals-header">
      <div class="header-left">
        <Button label="Create Product" icon="pi pi-plus" @click="isDialogVisible = true" />
      </div>
      <div class="header-right">
        <Menu ref="mainMenu" :model="mainMenuItems" :popup="true" />
        <Button icon="pi pi-bars" @click="toggleMainMenu" text rounded aria-label="Menu" />
      </div>
    </header>

    <main class="meals-content">
      <h1>Products</h1>

      <div v-for="(productsInCat, categoryName) in productsStore.productsByCategory" :key="categoryName" class="category-group">
        <h2 class="category-header">
          {{ categoryName }}
        </h2>
        <div class="products-grid">
          <div
            v-for="product in productsInCat"
            :key="product.id"
            class="product-card"
            :class="{ 'selected': shoppingListStore.selectedProductIds.includes(product.id) }"
            @click="shoppingListStore.toggleProductSelection(product.id)"
          >
            <span class="product-name">{{ product.name }}</span>
            <Button icon="pi pi-trash" @click.stop="confirmDelete(product)" text rounded severity="danger" class="delete-button"/>
          </div>
        </div>
      </div>

      <div v-if="Object.keys(productsStore.productsByCategory).length === 0" class="empty-message">
        <p>No products found. Click "Create Product" to get started!</p>
      </div>
    </main>

    <Dialog v-model:visible="isDialogVisible" modal header="Create a New Product">
      <form @submit.prevent="handleAddProduct" class="dialog-form">
        <div class="form-group">
          <label for="productName">Product Name</label>
          <InputText id="productName" v-model="newProductName" autocomplete="off" />
        </div>
        <div class="form-group">
          <label for="category">Category</label>
          <Select
            id="category"
            v-model="selectedCategoryId"
            :options="categoriesStore.categories"
            optionLabel="name"
            optionValue="id"
            placeholder="Select a Category"
          />
        </div>
        <div class="dialog-footer">
          <Button label="Cancel" @click="isDialogVisible = false" text />
          <Button label="Create" type="submit" />
        </div>
      </form>
    </Dialog>

    <div v-if="shoppingListStore.hasSelection" class="fixed-action-bar">
      <Button
        label="Generate List"
        @click="shoppingListStore.generateList()"
      />
    </div>
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
.product-name {
  font-size: 1.1rem;
  font-weight: 500;
  color: white;
  padding-right: 2rem;
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

.category-group {
  margin-bottom: 2rem;
}
.category-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.25rem;
  color: #ccc;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #333;
}
.color-dot {
  width: 0.75rem;
  height: 0.75rem;
  border-radius: 50%;
}
.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 1rem;
}
.product-card {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #1e1e1e;
  padding: 1rem;
  border-radius: 8px;
  cursor: pointer;
  border: 2px solid transparent;
  transition: all 0.2s;
}
.product-card:hover {
  border-color: #555;
}
.product-card.selected {
  border-color: #356655;
  background-color: #2a2a2a;
}
.delete-button {
  position: absolute;
  top: 0.25rem;
  right: 0.25rem;
}

:deep(.p-dialog) {
  width: 90vw;
  max-width: 25rem;
}

.fixed-action-bar {
  position: fixed;
  bottom: 2rem;
  left: 0;
  width: 100%;
  padding: 1rem;
  display: flex;
  justify-content: center;
  z-index: 100;
}

.fixed-action-bar :deep(.p-button) {
  background: linear-gradient(to top, #34bc84, #4a8d73);
  border-radius: 40px;
  border: 1px solid #172e26;
  padding: 1rem 2rem;
  font-weight: 700;
  font-size: 1.4rem;
  color: white;
  text-shadow: 0 -1px 1px rgba(0, 0, 0, 0.2);
  cursor: pointer;

  box-shadow:
    inset 0 2px 2px rgba(255, 255, 255, 0.4),
    inset 0 -2px 3px rgba(0, 0, 0, 0.3),
    0 8px 16px rgba(0, 0, 0, 0.2);

  transition: all 0.1s ease-out;
}

.fixed-action-bar :deep(.p-button:active) {
  transform: translateY(2px);

  box-shadow:
    inset 0 1px 2px rgba(0, 0, 0, 0.4),
    0 4px 8px rgba(0, 0, 0, 0.2);
}
</style>
