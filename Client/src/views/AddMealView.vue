<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useMealsStore, type Meal, type MealProduct } from '@/stores/meals';
import { useProductsStore, type Product } from '@/stores/products';
import { useToast } from 'primevue/usetoast';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Select from 'primevue/select';
import InputNumber from 'primevue/inputnumber';

interface MealProductEntry {
  product: Product;
  quantity: number;
}

const router = useRouter();
const route = useRoute();
const mealsStore = useMealsStore();
const productsStore = useProductsStore();
const toast = useToast();

const name = ref('');
const description = ref('');
const isLoading = ref(false);

const mealProducts = ref<MealProduct[]>([]);
const selectedProduct = ref<Product | null>(null);
const quantity = ref<number | null>(1);

const isEditMode = computed(() => !!route.params.id);
const mealId = computed(() => route.params.id as string);

onMounted(async () => {
  await productsStore.fetchProducts();

  if (isEditMode.value) {
    const mealToEdit = await mealsStore.fetchMealById(mealId.value);
    setMealDataForEdit(mealToEdit);
  }
});

function setMealDataForEdit(mealToEdit: Meal | null) {
  if (mealToEdit) {
    name.value = mealToEdit.name;
    description.value = mealToEdit.description;

    console.log('Products received for editing:', mealToEdit.products);

    mealProducts.value = mealToEdit.products;
  } else {
    toast.add({ severity: 'error', summary: 'Error', detail: 'Meal not found.', life: 3000 });
    router.push('/meals');
  }
}

function addProductToList() {
  if (!selectedProduct.value || !quantity.value || quantity.value <= 0) {
    toast.add({ severity: 'warn', summary: 'Warning', detail: 'Please select a product and enter a valid quantity.', life: 3000 });
    return;
  }

  mealProducts.value.push({
    productId: selectedProduct.value.id,
    productName: selectedProduct.value.name,
    quantity: quantity.value
  });

  selectedProduct.value = null;
  quantity.value = 1;
}

function removeProduct(index: number) {
  mealProducts.value.splice(index, 1);
}

function goBack() {
  router.push('/meals');
}

async function handleSaveMeal() {
  if (!name.value) {
    toast.add({ severity: 'warn', summary: 'Warning', detail: 'Meal name is required.', life: 3000 });
    return;
  }
  isLoading.value = true;

  try {
    if (isEditMode.value) {
      const updatePayload = {
        id: mealId.value,
        name: name.value,
        description: description.value,
        products: mealProducts.value
      };

      await mealsStore.updateMeal(mealId.value, updatePayload);
    } else {
      const addPayload = {
        name: name.value,
        description: description.value,
        products: mealProducts.value
      };

      await mealsStore.addMeal(addPayload);
    }
    router.push('/meals');
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to save meal.', life: 3000 });
  } finally {
    isLoading.value = false;
  }
}
</script>

<template>
  <div class="add-meal-layout">
    <header class="add-meal-header">
      <Button icon="pi pi-arrow-left" @click="goBack" text rounded aria-label="Back" />
      <h1>{{ isEditMode ? 'Edit Meal' : 'Add a New Meal' }}</h1>
    </header>

    <main class="add-meal-content">
      <form @submit.prevent="handleSaveMeal" class="form-grid">
        <div class="form-group">
          <label for="mealName">Meal Name</label>
          <InputText id="mealName" v-model="name" />
        </div>
        <div class="form-group">
          <label for="mealDescription">Description</label>
          <Textarea id="mealDescription" v-model="description" rows="5" />
        </div>

        <div class="product-adder-card">
          <h3>Add Products</h3>
          <div class="product-input-group">
            <Select
              v-model="selectedProduct"
              :options="productsStore.products"
              optionLabel="name"
              placeholder="Select a Product"
              filter
              class="product-dropdown"
            />
            <InputNumber v-model="quantity" :min="0.1" mode="decimal" :max-fraction-digits="2" class="quantity-input" placeholder="Quantity" />
            <Button icon="pi pi-plus" @click="addProductToList" label="Add" type="button" />
          </div>

          <div class="product-list">
            <div v-if="!mealProducts.length" class="empty-message">No products added yet.</div>
            <div v-for="(item, index) in mealProducts" :key="item.productId" class="product-list-item">
              <span>{{ item.productName }} - <strong>{{ item.quantity }}</strong></span>
              <Button icon="pi pi-times" @click="removeProduct(index)" text rounded severity="danger" type="button" />
            </div>
          </div>
        </div>

        <Button :label="isEditMode ? 'Save Changes' : 'Add Meal'" type="submit" icon="pi pi-check" :loading="isLoading" />
      </form>
    </main>
  </div>
</template>

<style scoped>
.add-meal-layout {
  max-width: 800px;
  margin: 0 auto;
  padding: 1.5rem;
}
.add-meal-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #444;
  margin-bottom: 2rem;
  font-size: smaller;
}
.form-grid {
  display: grid;
  gap: 1.5rem;
}
.form-group, .product-adder-card {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  color: white;
}
.product-adder-card {
  background-color: #2a2a2a;
  padding: 1rem;
  border-radius: 8px;
  gap: 1rem;
}
.product-input-group {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  align-items: center;
}
.product-dropdown {
  flex: 1 1 150px;
}
.quantity-input {
  flex: 1 1 120px;
}
.product-input-group .p-button {
  flex-shrink: 0;
}
.product-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-top: 1rem;
}
.product-list-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #333;
  padding: 0.5rem 1rem;
  border-radius: 6px;
}
.empty-message {
  text-align: center;
  color: #888;
  font-style: italic;
}
</style>
