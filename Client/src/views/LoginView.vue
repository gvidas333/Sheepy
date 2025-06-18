<script setup lang="ts">
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from "vue-router";

  import Button from 'primevue/button';
  import InputText from 'primevue/inputtext';


  const email = ref('');
  const password = ref('');
  const router = useRouter();

  async function handleLogin(){
    try {
      const response = await axios.post('https://localhost:7021/api/auth/login', {
        email: email.value,
        password: password.value
      });

      const token = response.data.token;
      localStorage.setItem('token', token);
      alert('HEY HEY');
      router.push('/');

    } catch (error) {
      alert('Prisijungti nepavyko :(');
    }
  }
</script>

<template>
  <div class="page-wrapper">
    <nav class="auth-nav">
      <RouterLink to="/login" class="nav-link">Login</RouterLink>
      <span>|</span>
      <RouterLink to="/register" class="nav-link">Register</RouterLink>
    </nav>

    <div class="auth-container">
      <div class="card">
        <h1 class="title">Login</h1>

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
      </div>
    </div>
  </div>
</template>

<style scoped>
.page-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-bottom: 20vh;
  min-height: 100vh;
}

.auth-nav {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  font-size: 1.1rem;
  color: #ccc;
}

.nav-link:hover {
  text-decoration: underline;
}

.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.card {
  width: 100%;
  max-width: 400px;
  padding: 2rem;
  background-color: #1e1e1e;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
}

.title {
  text-align: center;
  color: white;
  margin-bottom: 1.5rem;
}

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
