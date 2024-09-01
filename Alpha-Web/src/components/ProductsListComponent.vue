<template>
  <div class="bg-white">
    <div class="mx-auto max-w-2xl px-4 py-16 sm:px-6 lg:max-w-7xl lg:px-8">
      <div
        class="grid grid-cols-1 gap-x-6 gap-y-10 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 xl:gap-x-8"
      >
        <a
          v-for="product in products"
          :key="product.id"
          :href="product.image"
          class="group"
        >
          <div
            class="aspect-h-1 aspect-w-1 flex justify-center w-full overflow-hidden rounded-lg xl:aspect-h-8 xl:aspect-w-7"
          >
            <img
              :src="product.image"
              class="h-40 w-50 object-fill object-center group-hover:opacity-75"
            />
          </div>
          <div class="flex justify-between mt-4">
            <div class="d-flex flex-column">
              <h3 class="text-sm text-gray-700 mr-4">
                {{ setTitle(product.name) }}
              </h3>
              <p class="mt-1 text-lg font-medium text-gray-900">
                {{ product.price }}
              </p>
            </div>
            <div class="flex">
              <span class="sm:block">
                <button
                  type="button"
                  class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
                >
                  Editar
                </button>
                <!-- <span class="sm:ml-3">
                  <button
                    type="button"
                    class="inline-flex items-center rounded-md bg-red-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-red-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-red-600"
                  >
                    Remover
                  </button>
                </span> -->
              </span>
            </div>
          </div>
        </a>
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  name: {
    type: String,
    default: "ProductsList",
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
});
console.log("Produtos Lista: ", props.products);

function setTitle(name) {
  if (name.length > 40) {
    return name.substring(0, 40) + "...";
  }
  return name;
}
</script>
