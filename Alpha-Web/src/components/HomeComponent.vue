<template>
  <div class="min-h-full">
    <NavBarComponent />
    <header class="bg-white shadow">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">Home</h1>
      </div>
    </header>
    <main>
      <div class="card flex flex-wrap justify-center gap-4 mt-8">
        <InputText
          class="w-[62%] input-text"
          @blur="setGlobalFilter($event)"
          placeholder="Informe o produto ou código"
        />
        <button
          type="button"
          @click="clearSearch()"
          class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 mr-1"
        >
          Limpar Pesquisa
        </button>
        <DropdownComponent
          class="h-50"
          @order-selected="handleOptionSelected"
        />
      </div>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <p v-if="loading">Loading...</p>
        <ProductsListComponent :products="productsList" v-else />
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
      <div class="card mb-4">
        <Paginator
          :rows="searchResult.pageSize"
          @page="handlePageSelected($event)"
          :totalRecords="searchResult.totalRecords"
        ></Paginator>
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

let globalFilter = "";
let order = 0;
let page = 0;

export default {
  components: {
    ProductsListComponent,
    NavBarComponent,
    DropdownComponent,
  },
  data() {
    return {
      productsList: [],
      searchResult: {
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
    order = 0;
    page = 0;
    await this.fetchProducts();
  },
  methods: {
    mountedSearchResult(response) {
      this.searchResult.pageCount = response.pageCount;
      this.searchResult.pageIndex = response.pageIndex;
      this.searchResult.pageSize = response.pageSize;
      this.searchResult.totalRecords = response.totalRecords;
    },
    async fetchProducts() {
      this.productsList = [];
      const requestPayload = {
        globalFilter: globalFilter,
        order: order,
        pageIndex: page,
        pageSize: 8,
      };
      try {
        const response = await productService.listPage(requestPayload);
        this.productsList = response.searchResult;
        this.mountedSearchResult(response);
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

    async clearSearch() {
      globalFilter = "";
      const input = document.querySelector(".input-text");
      if (input) {
        input.value = "";
      }
      await this.fetchProducts();
    },

    async setGlobalFilter(event) {
      globalFilter = event.currentTarget.value;
      await this.fetchProducts();
    },

    async handleOptionSelected(value) {
      order = value;
      await this.fetchProducts();
    },

    async handlePageSelected(value) {
      page = value.page;
      await this.fetchProducts();
    },

    redirectRegister() {
      this.$router.push("/cadastro");
    },
  },
};
</script>

<style>
.error {
  color: red;
}
</style>
