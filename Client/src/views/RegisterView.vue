<script setup lang="ts">
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  import { useToast } from 'primevue/usetoast';
  import AuthLayout from '@/layouts/AuthLayout.vue';
  import Button from 'primevue/button';
  import InputText from 'primevue/inputtext';
  import apiClient from '@/services/api';

  const email = ref('');
  const password = ref('');
  const router = useRouter();
  const toast = useToast();

  async function handleRegister()
  {
    try {
      await apiClient.post('/auth/register', {
        email: email.value,
        password: password.value
      });
      toast.add({ severity: 'success', summary: 'Success', detail: 'Registration successful! You can now log in.', life: 3000 });
      router.push('/login');
    } catch (error) {
      toast.add({ severity: 'error', summary: 'Error', detail: 'Registration failed!', life: 3000 });
    }
  }
</script>

<template>
  <AuthLayout title="Register">
    <form @submit.prevent="handleRegister" class="form-grid">
      <div class="form-group">
        <label for="email">Email</label>
        <InputText type="email" id="email" v-model="email"/>
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <InputText type="password" id="password" v-model="password"/>
      </div>
      <Button label="Register" type="submit" icon="pi pi-user-plus" />
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
