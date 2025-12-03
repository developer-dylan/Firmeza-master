<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-[#f6f7ff] via-[#f3f5ff] to-[#ececff] px-4 py-12">

    <div class="max-w-md w-full space-y-10">

      <!-- HEADER -->
      <div class="text-center">
        <div class="mx-auto h-16 w-16 rounded-2xl bg-gradient-to-br from-indigo-600 to-violet-600 flex items-center justify-center shadow-xl shadow-indigo-400/30">
          <span class="text-white font-black text-3xl">F</span>
        </div>

        <h2 class="mt-6 text-4xl font-extrabold tracking-tight text-[#0f172a]">
          Únete a Firmeza
        </h2>
        <p class="mt-2 text-sm text-slate-500">
          Crea tu cuenta para comenzar
        </p>
      </div>

      <!-- CARD -->
      <div class="rounded-2xl bg-white/70 backdrop-blur-xl shadow-xl shadow-slate-300/40 p-8 border border-white/40 space-y-6">

        <form @submit.prevent="handleRegister" class="space-y-5">

          <!-- FULL NAME -->
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-2">
              Nombre completo
            </label>
            <input
              v-model="form.fullName"
              type="text"
              required
              class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
              placeholder="Juan Pérez"
            />
          </div>

          <!-- 2 COLUMNS -->
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-slate-700 mb-2">
                Documento
              </label>
              <input
                v-model="form.documentNumber"
                type="text"
                required
                class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
                placeholder="12345678"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-slate-700 mb-2">
                Teléfono
              </label>
              <input
                v-model="form.phone"
                type="text"
                required
                class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
                placeholder="987654321"
              />
            </div>
          </div>

          <!-- EMAIL -->
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-2">
              Correo electrónico
            </label>
            <input
              v-model="form.email"
              type="email"
              required
              class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
              placeholder="correo@ejemplo.com"
            />
          </div>

          <!-- PASSWORD -->
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-2">
              Contraseña
            </label>
            <input
              v-model="form.password"
              type="password"
              required
              class="w-full px-4 py-3 rounded-xl border border-slate-200 bg-white/60 focus:ring-4 focus:ring-indigo-200 focus:border-indigo-500 transition-all"
              placeholder="Mínimo 6 caracteres"
            />
          </div>

          <!-- ERROR MESSAGE -->
          <div v-if="error" class="rounded-xl bg-red-50 border border-red-200 p-4 text-center">
            <p class="text-sm text-red-600 font-medium">
              {{ error }}
            </p>
          </div>

          <!-- SUBMIT BUTTON -->
          <button
            type="submit"
            :disabled="loading"
            class="w-full py-3 flex justify-center items-center rounded-xl font-semibold text-white bg-gradient-to-r from-indigo-600 to-violet-600 shadow-md shadow-indigo-400/30 hover:shadow-lg transition-all disabled:opacity-50 disabled:cursor-not-allowed mt-4"
          >
            <span v-if="loading" class="flex items-center gap-2">
              <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"/>
              </svg>
              Registrando...
            </span>
            <span v-else>Crear cuenta</span>
          </button>
        </form>

        <!-- DIVIDER -->
        <div class="flex items-center gap-3">
          <div class="h-px flex-1 bg-slate-200"></div>
          <span class="text-slate-400 text-sm">¿Ya tienes cuenta?</span>
          <div class="h-px flex-1 bg-slate-200"></div>
        </div>

        <!-- LOGIN LINK -->
        <router-link
          to="/login"
          class="block w-full text-center py-3 px-6 border border-indigo-200 text-indigo-600 font-semibold rounded-xl hover:bg-indigo-50 hover:border-indigo-300 transition-all shadow-sm"
        >
          Iniciar sesión
        </router-link>

      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useRouter } from 'vue-router';

const form = reactive({
  fullName: '',
  documentNumber: '',
  phone: '',
  email: '',
  password: ''
});

const error = ref('');
const loading = ref(false);
const authStore = useAuthStore();
const router = useRouter();

const handleRegister = async () => {
  loading.value = true;
  error.value = '';
  try {
    await authStore.register(form);
    await authStore.login({ email: form.email, password: form.password });
    router.push('/catalog');
  } catch (err) {
    error.value = err.response?.data?.message || 'Error en el registro.';
  } finally {
    loading.value = false;
  }
};
</script>
