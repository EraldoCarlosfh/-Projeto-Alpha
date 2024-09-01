<template>
  <div class="min-h-full">
    <NavBarComponent />
    <header class="bg-white shadow">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">Home</h1>
      </div>
    </header>
    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <div class="flex flex-col md:flex-row gap-4 justify-between">
          <span class="sm:ml-3">
            <button
              type="button"
              @click="redirectRegister"
              class="inline-flex items-center rounded-md bg-slate-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
            >
              + Cadastrar
            </button>
          </span>
          <DropdownComponent @order-selected="handleOptionSelected" />
        </div>
        <p v-if="loading">Loading...</p>
        <ProductsListComponent
          :searchResult="searchResults"
          :products="productsList"
          v-else
        />
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
    </main>
  </div>
</template>

<script>
import { notify } from "@kyvg/vue3-notification";
import ProductsListComponent from "./ProductsListComponent.vue";
import NavBarComponent from "../shared/NavBarComponent.vue";
import DropdownComponent from "../shared/DropdownComponent.vue";
import { productService } from "../services/productService";

export default {
  components: {
    ProductsListComponent,
    NavBarComponent,
    DropdownComponent,
  },
  data() {
    return {
      productsList: [],
      searchResults: {
        pageCount: 0,
        pageIndex: 0,
        pageSize: 0,
        totalRecords: 0,
      },
      loading: true,
      errorMessage: null,
    };
  },
  async mounted() {
    await this.fetchProducts();
  },
  methods: {
    mountedSearchResults(response) {
      this.searchResults.pageCount = response.pageCount;
      this.searchResults.pageIndex = response.pageIndex;
      this.searchResults.pageSize = response.pageSize;
      this.searchResults.totalRecords = response.totalRecords;
    },
    async fetchProducts(value = 1) {
      this.productsList = [];
      const requestPayload = {
        globalFilter: "",
        order: value,
        pageIndex: 0,
        pageSize: 10,
      };
      try {
        const response = await productService.listPage(requestPayload);
        this.productsList = response.searchResult;
        this.mountedSearchResults(response);
      } catch (error) {
        this.errorMessage = error.message;
        notify({
          title: error.code,
          text: error.message,
          type: "error",
        });
      } finally {
        this.loading = false;
      }
    },

    async handleOptionSelected(value) {
      await this.fetchProducts(value);
    },

    redirectRegister() {
      this.$router.push("/cadastro");
    },
  },
  computed: {
    OrderingOption() {
      return {
        Ascending: 1,
        Descending: 2,
        MoreValue: 3,
        LowerValue: 4,
      };
    },
  },
};
</script>

<style>
.error {
  color: red;
}
</style>
