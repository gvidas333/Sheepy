<script setup lang="ts">
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from "vue-router";
  import { useAuthStore } from '@/stores/auth';
  import { useToast } from 'primevue/usetoast';
  import AuthLayout from '@/layouts/AuthLayout.vue';
  import Button from 'primevue/button';
  import InputText from 'primevue/inputtext';


  const email = ref('');
  const password = ref('');
  const router = useRouter();
  const authStore = useAuthStore();
  const toast = useToast();

  async function handleLogin(){
    try {
      const response = await axios.post('https://localhost:7021/api/auth/login', {
        email: email.value,
        password: password.value
      });

      authStore.setToken(response.data.token);
      router.push('/meals');

    } catch (error) {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Login failed!', life: 3000 });
    }
  }
</script>

<template>
  <AuthLayout title="Login">
    <form @submit.prevent="handleLogin" class="form-grid">
      <div class="form-group">
        <label for="email">Email</label>
        <InputText id="email" type="email" v-model="email" />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <InputText id="password" type="password" v-model="password" />
      </div>
      <Button label="Login" type="submit" icon="pi pi-sign-in" />
    </form>
  </AuthLayout>
</template>

<style scoped>
.form-grid {
  display: grid;
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  color: white;
}
</style>
