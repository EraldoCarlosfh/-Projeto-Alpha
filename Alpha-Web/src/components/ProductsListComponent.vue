<template>
  <DialogComponent
    v-if="isModalOpen"
    :productName="product"
    :productId="id"
    @close="closeModal"
    @delete="handleProductDelete"
  />
  <div class="bg-white">
    <div class="mx-auto px-4 py-4">
      <h2 class="text-2xl font-bold tracking-tight text-gray-900">
        Produtos Cadastrados
      </h2>

      <div
        class="mt-6 grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-4 xl:gap-x-8"
      >
        <div
          v-for="product in products"
          :key="product.id"
          class="group relative"
        >
          <div
            class="aspect-h-1 aspect-w-1 w-full overflow-hidden rounded-md bg-gray-200 lg:aspect-none group-hover:opacity-75 lg:h-80"
          >
            <img
              @click="openModal(product.id, product.name)"
              :src="product.image"
              class="h-full w-full object-cover object-center lg:h-full lg:w-full"
            />
          </div>
          <div class="mt-4 flex justify-between">
            <div>
              <h3 class="text-sm text-gray-700">
                <span>
                  {{ setTitle(product.name) }}
                </span>
              </h3>
              <p class="mt-1 text-sm text-gray-500">
                Cód: {{ product.barCode }}
              </p>
            </div>
            <div>
              <p class="text-sm font-medium text-gray-900">
                {{ formattedPrice(product.price) }}
              </p>
              <span class="sm:block mt-1">
                <router-link :to="`/cadastro/` + product.id">
                  <button
                    type="button"
                    class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                  >
                    Editar
                  </button>
                </router-link>
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div>
      <p>Page Count: {{ searchResult.pageCount }}</p>
      <p>Page Index: {{ searchResult.pageIndex }}</p>
      <p>Page Size: {{ searchResult.pageSize }}</p>
      <p>Total Records: {{ searchResult.totalRecords }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import DialogComponent from "./DialogComponent.vue";
const isModalOpen = ref(false);

let product = "";
let id = "";

const openModal = (productId, productName) => {
  product = productName;
  id = productId;
  isModalOpen.value = true;
};

const closeModal = () => {
  isModalOpen.value = false;
};

const handleProductDelete = (payload) => {
  window.location.reload();
};

const props = defineProps({
  searchResult: {
    type: Object,
    required: true,
    validator(value) {
      return value.every(
        (item) =>
          item.hasOwnProperty("pageCount") &&
          item.hasOwnProperty("pageIndex") &&
          item.hasOwnProperty("pageSize") &&
          item.hasOwnProperty("totalRecords")
      );
    },
  },
  products: {
    type: Array,
    required: true,
    validator(value) {
      return value.every(
        (item) =>
          item.hasOwnProperty("id") &&
          item.hasOwnProperty("name") &&
          item.hasOwnProperty("barcode") &&
          item.hasOwnProperty("price") &&
          item.hasOwnProperty("image")
      );
    },
  },
  searchResult: {},
});

function formattedPrice(price) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(price);
}

function setTitle(name) {
  if (name.length > 40) {
    return name.substring(0, 40) + "...";
  }
  return name;
}
</script>
