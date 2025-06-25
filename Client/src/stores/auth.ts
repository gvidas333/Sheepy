import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', () => {

  const token = ref(localStorage.getItem('token'))
  const isAuthenticated = computed(() => !!token.value)

  function setToken(newToken: string) {
    localStorage.setItem('token', newToken)
    token.value = newToken
  }

  function clearToken() {
    localStorage.removeItem('token')
    token.value = null
  }

  return {
    token,
    isAuthenticated,
    setToken,
    clearToken
  }
})
