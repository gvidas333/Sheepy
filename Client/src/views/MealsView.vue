<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { useMealsStore, type Meal } from '@/stores/meals';
import { useShoppingListStore } from '@/stores/shoppingList';
import { useConfirm } from "primevue/useconfirm";
import { useToast } from 'primevue/usetoast';
import Button from 'primevue/button';
import Card from 'primevue/card';
import Chip from 'primevue/chip';
import Menu from 'primevue/menu';

const router = useRouter();
const authStore = useAuthStore();
const mealsStore = useMealsStore();
const shoppingListStore = useShoppingListStore();
const confirm = useConfirm();
const toast = useToast();

const cardMenu = ref();
const mainMenu = ref();

const selectedMealIds = ref<string[]>([]);

async function handleGenerateList() {
  toast.add({ severity: 'info', summary: 'Generating', detail: 'Preparing your list...', life: 2000 });

  try {
    await shoppingListStore.generateList(selectedMealIds.value);
    toast.add({ severity: 'success', summary: 'Success', detail: 'List generated!', life: 3000 });
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to generate list.', life: 3000 });
  }
}
function toggleMealSelection(mealId: string) {
  const index = selectedMealIds.value.indexOf(mealId);
  if (index > -1) {
    selectedMealIds.value.splice(index, 1);
  } else selectedMealIds.value.push(mealId);
}

const selectedMeal = ref<Meal | null>(null);
const cardMenuItems = ref([
  {
    label: 'Options',
    items: [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          if (selectedMeal.value) {
            router.push({ name: 'edit-meal', params: { id: selectedMeal.value.id } });
          }
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          if (selectedMeal.value) {
            confirmDelete(selectedMeal.value);
          }
        }
      }
    ]
  }
]);

const mainMenuItems = ref([
  { label: 'Meals', icon: 'pi pi-book', command: () => router.push({ name: 'meals' }) },
  { label: 'Products', icon: 'pi pi-shopping-cart', command: () => router.push({ name: 'products' }) },
  { label: 'Categories', icon: 'pi pi-tags', command: () => router.push({ name: 'categories' }) },
  { label: 'Shopping List', icon: 'pi pi-list', command: () => router.push({ name: 'shopping-list' }) },
  { separator: true },
  { label: 'Log Out', icon: 'pi pi-sign-out', command: () => handleLogout() }
]);

onMounted(() => {
  mealsStore.fetchMeals();
});

function toggleCardMenu(event: Event, meal: Meal) {
  selectedMeal.value = meal;
  cardMenu.value.toggle(event);
}

function toggleMainMenu(event: Event) {
  mainMenu.value.toggle(event);
}

function confirmDelete(meal: Meal) {
  confirm.require({
    message: 'Are you sure you want to delete this meal?',
    header: 'Delete Confirmation',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    rejectClass: 'p-button-text',
    acceptLabel: 'Delete',
    rejectLabel: 'Cancel',
    accept: async () => {
      try {
        await mealsStore.deleteMeal(meal.id);
        toast.add({ severity: 'success', summary: 'Success', detail: 'Meal deleted!', life: 3000 });
      } catch (error) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete meal.', life: 3000 });
      }
    },
  });
}

function handleLogout() {
  authStore.clearToken();
  router.push('/login');
}

function goToAddMealPage() {
  router.push('/meals/new');
}

</script>

<template>
  <div class="meals-layout">
    <header class="meals-header">
      <div class="header-left">
        <Button label="Add Mealy" icon="pi pi-plus" @click="goToAddMealPage" />
      </div>
      <div class="header-right">
        <Menu ref="mainMenu" :model="mainMenuItems" :popup="true" />
        <Button icon="pi pi-bars" @click="toggleMainMenu" text rounded aria-label="Menu" />
      </div>
    </header>

    <main class="meals-content">
      <h1>My Mealy</h1>

      <div v-if="mealsStore.meals.length > 0" class="meals-grid">
        <Menu ref="cardMenu" :model="cardMenuItems" :popup="true" />

        <div
          v-for="meal in mealsStore.meals"
          :key="meal.id"
          class="meal-card-wrapper"
          :class="{ 'selected': selectedMealIds.includes(meal.id) }"
          @click="toggleMealSelection(meal.id)"
        >
          <Button
            icon="pi pi-ellipsis-h"
            @click.stop="toggleCardMenu($event, meal)"
            text rounded
            class="meal-menu-button"
          />
          <Card>
            <template #title>{{ meal.name }}</template>
            <template #content>
              <div class="products-list" v-if="meal.products && meal.products.length">
                <Chip v-for="item in meal.products" :key="item.productId" :label="`${item.productName} - ${item.quantity}g`" />
              </div>
            </template>
          </Card>
        </div>

      </div>
      <div v-else class="no-meals-message">
        <p>You haven't added any meals yet. Click "Add Mealy" to get started!</p>
      </div>
    </main>

    <div v-if="selectedMealIds.length > 0" class="fixed-action-bar">
      <Button
        label="Generate List"
        @click="handleGenerateList"
        severity="secondary"
        class="generate-button"
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

.meals-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  gap: 1rem;
}

.meal-card-wrapper {
  position: relative;
  cursor: pointer;
  border: 2px solid transparent;
  border-radius: 10px;
  transition: border-color 0.2s;
}

.meal-card-wrapper:hover {
  border-color: #555;
}

.meal-card-wrapper.selected {
  border-color: #356655;
}

.meal-menu-button {
  position: absolute;
  bottom: 0.5rem;
  right: 0.5rem;
  z-index: 10;
  color: white;
}

.no-meals-message p {
  color: #ccc;
  text-align: center;
  font-style: italic;
  margin-top: 4rem;
}

.products-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 1rem;
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

:deep(.p-card) {
  background-color: #1e1e1e;
  color: white;
  border-radius: 8px;
  aspect-ratio: 1 / 1;
  display: flex;
  flex-direction: column;
  height: 100%;
}

:deep(.p-card-title) {
  font-size: 1.15rem;
  line-height: 1.3em;
  height: 2.6em;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
}

:deep(.p-card .p-card-content) {
  overflow-y: auto;
  flex-grow: 1;
  padding-bottom: 2rem;
}
</style>
