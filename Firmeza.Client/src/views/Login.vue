<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-[#f6f7ff] via-[#f3f5ff] to-[#ececff] px-4 py-10">

    <div class="w-full max-w-md">

      <!-- HEADER -->
      <div class="text-center mb-10">
        <div class="mx-auto h-16 w-16 rounded-2xl bg-gradient-to-br from-indigo-600 to-violet-600 flex items-center justify-center shadow-xl shadow-indigo-400/30">
          <span class="text-white font-black text-3xl tracking-tight">F</span>
        </div>

        <h2 class="mt-6 text-4xl font-extrabold tracking-tight text-[#0f172a]">
          Bienvenido de vuelta
        </h2>
        <p class="mt-2 text-sm text-slate-500">
          Ingresa tus credenciales para continuar
        </p>
      </div>

      <!-- CARD -->
      <div class="rounded-2xl bg-white/70 backdrop-blur-xl shadow-xl shadow-slate-200/60 p-8 space-y-6 border border-white/40">

        <form @submit.prevent="handleLogin" class="space-y-6">

          <!-- EMAIL -->
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-2">
              Correo electrónico
            </label>
            <input
              v-model="email"
              type="email"
              required
              class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
              placeholder="tu@email.com"
            />
          </div>

          <!-- PASSWORD -->
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-2">
              Contraseña
            </label>
            <input
              v-model="password"
              type="password"
              required
              class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
              placeholder="••••••••"
            />
          </div>

          <!-- ERROR MESSAGE -->
          <div v-if="error" class="p-4 bg-red-50 border border-red-200 rounded-xl text-center">
            <p class="text-red-600 text-sm font-medium">
              {{ error }}
            </p>
          </div>

          <!-- BUTTON -->
          <button
            type="submit"
            :disabled="loading"
            class="w-full py-3 flex justify-center items-center rounded-xl font-semibold text-white bg-gradient-to-r from-indigo-600 to-violet-600 shadow-md shadow-indigo-400/30 hover:shadow-lg transition-all disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span v-if="loading" class="flex items-center gap-2">
              <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0"/>
              </svg>
              Cargando...
            </span>
            <span v-else>Ingresar</span>
          </button>
        </form>

        <!-- DIVIDER -->
        <div class="flex items-center gap-3 my-4">
          <div class="h-px flex-1 bg-slate-200"></div>
          <span class="text-slate-400 text-sm">¿No tienes cuenta?</span>
          <div class="h-px flex-1 bg-slate-200"></div>
        </div>

        <!-- REGISTER -->
        <router-link
          to="/register"
          class="block w-full text-center py-3 px-6 border border-indigo-200 text-indigo-600 font-semibold rounded-xl hover:bg-indigo-50 transition-all shadow-sm hover:border-indigo-300"
        >
          Crear una cuenta
        </router-link>

      </div>

    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const email = ref('');
const password = ref('');
const error = ref('');
const loading = ref(false);
const authStore = useAuthStore();
const router = useRouter();

const handleLogin = async () => {
  loading.value = true;
  error.value = '';
  try {
    await authStore.login({ email: email.value, password: password.value });
    router.push('/catalog');
  } catch (err) {
    if (err.message) {
      error.value = err.message;
    } else if (err.response?.data?.message) {
      error.value = err.response.data.message;
    } else {
      error.value = 'Credenciales inválidas o error en el servidor.';
    }
  } finally {
    loading.value = false;
  }
};
</script>
