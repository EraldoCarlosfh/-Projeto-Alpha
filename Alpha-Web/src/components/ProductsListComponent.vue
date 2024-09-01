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
              <p class="text-sm font-medium text-gray-900">
                {{ formattedPrice(product.price) }}
              </p>
            </div>
            <div class="">
              <span class="sm:block mt-1 ml-1">
                <router-link :to="`/cadastro/` + product.id">
                  <button
                    type="button"
                    class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 mr-1"
                  >
                    Editar
                  </button>
                </router-link>
              </span>
              <span class="sm:block mt-1">
                <button
                  type="button"
                  @click="openModal(product.id, product.name)"
                  class="inline-flex items-center rounded-md bg-red-500 px-3 py-2 text-sm font-semibold text-white shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-red-500"
                >
                  Deletar
                </button>
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import DialogComponent from "../shared/DialogComponent.vue";
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
  products: {
    type: Array,
    required: true,
    validator(value) {
      return value.every(
        (item) =>
          item.hasOwnProperty("id") &&
          item.hasOwnProperty("name") &&
          item.hasOwnProperty("barCode") &&
          item.hasOwnProperty("price") &&
          item.hasOwnProperty("image") &&
          item.hasOwnProperty("createdAt") &&
          item.hasOwnProperty("isActive")
      );
    },
  },
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
