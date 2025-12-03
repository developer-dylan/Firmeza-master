<template>
  <div class="px-6 lg:px-16 py-12 min-h-screen">

    <!-- HEADER -->
    <div class="text-center mb-14">
      <h1 class="text-4xl font-extrabold tracking-tight text-[#0f172a]">
        Catálogo de Productos
      </h1>
      <p class="text-slate-600 mt-2 text-lg">
        Selección premium de productos disponibles
      </p>
    </div>

    <!-- LOADING -->
    <div v-if="loading" class="flex justify-center items-center py-20">
      <div class="glass-card p-10 text-center">
        <div class="animate-spin h-12 w-12 mx-auto text-indigo-600 mb-6">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
            <path class="opacity-75" fill="currentColor"
            d="M4 12a8 8 0 018-8V0C5 0 0 5 0 12h4z"/>
          </svg>
        </div>
        <p class="text-lg text-slate-600">Cargando productos...</p>
      </div>
    </div>

    <!-- ERROR -->
    <div v-else-if="error" class="glass-card p-8 text-red-600 border border-red-200 bg-red-50 text-center font-medium">
      {{ error }}
    </div>

    <!-- SIN PRODUCTOS -->
    <div v-else-if="allProducts.length === 0" class="glass-card p-12 text-center">
      <div class="text-6xl mb-4 opacity-70">📭</div>
      <h3 class="text-xl font-semibold text-slate-700 mb-2">No hay productos aún</h3>
      <p class="text-slate-500">Estamos trabajando en añadir nuevos productos.</p>
    </div>

    <!-- PRODUCT LIST -->
    <div v-else>

      <!-- INFO -->
      <div class="flex justify-between items-center mb-6 text-sm text-slate-600">
        <p>Mostrando {{ startIndex + 1 }} - {{ endIndex }} de {{ allProducts.length }}</p>
        <p>Página {{ currentPage }} de {{ totalPages }}</p>
      </div>

      <!-- GRID PROFESIONAL -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
        <div v-for="product in paginatedProducts" :key="product.id" class="product-card">

          <!-- IMAGE -->
          <div class="product-card-img">
            <img src="https://cdn-icons-png.flaticon.com/512/679/679720.png" class="product-img" />
          </div>

          <!-- INFO -->
          <div class="space-y-3">
            <h3 class="product-title">{{ product.name }}</h3>

            <p class="product-description">
              {{ product.description || 'Producto de excelente calidad' }}
            </p>

            <div class="product-footer">
              <div>
                <p class="product-price">{{ formatPrice(product.price) }}</p>
                <p class="product-stock">Stock: {{ product.quantity }}</p>
              </div>

              <button 
                @click="addToCart(product)"
                :disabled="product.quantity <= 0"
                class="product-btn"
              >
                🛒
              </button>
            </div>
          </div>

        </div>
      </div>

      <!-- PAGINATION -->
      <div class="pagination-wrapper">
        <button
          @click="goToPage(currentPage - 1)"
          :disabled="currentPage === 1"
          class="pagination-btn"
        >
          «
        </button>

        <button
          v-for="page in visiblePages"
          :key="page"
          @click="goToPage(page)"
          :class="['pagination-number',
            page === currentPage ? 'pagination-number-active' : '']"
        >
          {{ page }}
        </button>

        <button
          @click="goToPage(currentPage + 1)"
          :disabled="currentPage === totalPages"
          class="pagination-btn"
        >
          »
        </button>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import api from "../services/api";
import { useCartStore } from "../stores/cart";

const allProducts = ref([]);
const loading = ref(true);
const error = ref("");
const cartStore = useCartStore();

// Format money professionally
const formatPrice = (amount) => {
  return new Intl.NumberFormat("es-PE", {
    style: "currency",
    currency: "PEN",
    minimumFractionDigits: 2,
  }).format(amount);
};

// Pagination
const currentPage = ref(1);
const itemsPerPage = 12;

const totalPages = computed(() =>
  Math.ceil(allProducts.value.length / itemsPerPage)
);
const startIndex = computed(() => (currentPage.value - 1) * itemsPerPage);
const endIndex = computed(() =>
  Math.min(startIndex.value + itemsPerPage, allProducts.value.length)
);

const paginatedProducts = computed(() =>
  allProducts.value.slice(startIndex.value, endIndex.value)
);

const visiblePages = computed(() => {
  const pages = [];
  const maxVisible = 5;
  let start = Math.max(1, currentPage.value - Math.floor(maxVisible / 2));
  let end = Math.min(totalPages.value, start + maxVisible - 1);

  if (end - start < maxVisible - 1) {
    start = Math.max(1, end - maxVisible + 1);
  }

  for (let i = start; i <= end; i++) pages.push(i);
  return pages;
});

const goToPage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    window.scrollTo({ top: 0, behavior: "smooth" });
  }
};

const fetchProducts = async () => {
  try {
    const response = await api.get("/Products");
    allProducts.value = response.data;
  } catch (err) {
    error.value = "Error al cargar los productos.";
  } finally {
    loading.value = false;
  }
};

const addToCart = (product) => {
  cartStore.addToCart(product);
};

onMounted(fetchProducts);
</script>

<style scoped>

</style>
