<script setup lang="ts">
defineProps<{
  title: string
}>();
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
        <h1 class="title">{{ title }}</h1>
        <slot></slot>
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
</style>
