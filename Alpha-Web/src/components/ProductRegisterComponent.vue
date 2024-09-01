<template>
  <div class="min-h-full">
    <NavBarComponent />
    <header class="bg-white shadow">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">
          Cadastro de Produto
        </h1>
      </div>
    </header>
    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <form>
          <div class="space-y-12">
            <div class="pb-12">
              <h2 class="text-base font-semibold leading-7 text-gray-900">
                Informações do Produto
              </h2>
              <p class="mt-1 text-sm leading-6 text-gray-600">
                Insira as informações do produto
              </p>
              <div class="border-t border-gray-900/10 pb-2">
                <div
                  class="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6"
                >
                  <div class="col-span-full">
                    <label
                      for="cover-photo"
                      class="block text-sm font-medium leading-6 text-gray-900"
                      >Foto do Produto</label
                    >
                    <div
                      class="mt-2 flex justify-center rounded-lg border border-dashed border-gray-900/25 px-6 py-10"
                    >
                      <div class="text-center">
                        <div class="mt-4 flex text-sm leading-6 text-gray-600">
                          <label
                            for="file-upload"
                            class="relative cursor-pointer rounded-md bg-white font-semibold text-indigo-600 focus-within:outline-none focus-within:ring-2 focus-within:ring-indigo-600 focus-within:ring-offset-2 hover:text-indigo-500"
                          >
                            <span class="text-indigo-600"
                              >Upload de imagem</span
                            >
                            <input
                              id="file-upload"
                              name="file-upload"
                              @change="handleFileChange"
                              type="file"
                              class="sr-only"
                            />
                          </label>
                        </div>
                        <p class="text-xs leading-5 text-gray-600">
                          PNG, JPG, GIF up to 10MB
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div
                class="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6"
              >
                <div class="sm:col-span-2">
                  <label
                    class="block text-sm font-medium leading-6 text-gray-900"
                    >Produto</label
                  >
                  <div class="mt-2">
                    <input
                      type="text"
                      v-model="form.name"
                      @blur="validateName"
                      class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                    />
                    <span class="span-error" v-if="errors.name">{{
                      errors.name
                    }}</span>
                  </div>
                </div>

                <div class="sm:col-span-2">
                  <label
                    class="block text-sm font-medium leading-6 text-gray-900"
                    >Código de Barras</label
                  >
                  <div class="mt-2">
                    <input
                      type="text"
                      v-model="form.barcode"
                      @blur="validateBarcode"
                      class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                    />
                    <span class="span-error" v-if="errors.barcode">{{
                      errors.barcode
                    }}</span>
                  </div>
                </div>

                <div class="sm:col-span-2">
                  <label
                    class="block text-sm font-medium leading-6 text-gray-900"
                    >Preço R$</label
                  >
                  <div class="mt-2">
                    <input
                      type="text"
                      v-mask="'#0,00'"
                      v-model="price"
                      @blur="updatePrice()"
                      placeholder="Digite um valor"
                      class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                    />
                    <span class="span-error" v-if="errors.price">{{
                      errors.price
                    }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="mt-6 flex items-center justify-end gap-x-6">
            <button
              type="button"
              @click="handleCancel"
              class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            >
              Cancelar
            </button>
            <button
              type="submit"
              :disabled="!isFormValid"
              @click="submitForm"
              class="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            >
              Cadastrar
            </button>
          </div>
        </form>
      </div>
    </main>
  </div>
</template>

<script>
import axios from "axios";
import { notify } from "@kyvg/vue3-notification";
import NavBarComponent from "./NavBarComponent.vue";
import { productService } from "../services/productService";

export default {
  components: {
    NavBarComponent,
  },
  data() {
    return {
      form: {
        name: "",
        price: "0",
        barcode: "",
        image: "https://i.pravatar.cc",
      },
      errors: {
        name: null,
        price: null,
        barcode: null,
      },
      product: {
        name: "",
        price: 0,
        barcode: "",
        image: "https://i.pravatar.cc",
      },
      price: "0",
    };
  },
  computed: {
    isFormValid() {
      return (
        !this.errors.name &&
        !this.errors.price &&
        !this.errors.barcode &&
        this.form.name &&
        this.form.price &&
        this.form.barcode
      );
    },
    formattedPrice() {
      return new Intl.NumberFormat("pt-BR", {
        style: "currency",
        currency: "BRL",
      }).format(parseFloat(this.price.replace(",", ".")));
    },
  },
  methods: {
    handleCancel() {
      (this.form.name = ""),
        (this.form.price = "0"),
        (this.price = "0"),
        (this.form.barcode = ""),
        (this.form.image = null);
    },
    validateName() {
      if (!this.form.name) {
        this.errors.name = "O nome do produto é obrigatório.";
      } else if (this.form.name.length < 6) {
        this.errors.name =
          "O nome do produto deve conter pelo menos 6 caracteres.";
      } else {
        this.errors.name = null;
      }
    },
    validatePrice() {
      if (!this.form.price) {
        this.errors.price = "O valor é obrigatório.";
      } else if (this.form.price == 0) {
        this.errors.price = "O valor deve ser maior que 0.";
      } else {
        this.errors.price = null;
      }
    },
    validateBarcode() {
      if (!this.form.barcode) {
        this.errors.barcode = "Código de barras é obrigatório.";
      } else if (this.form.barcode.length < 6) {
        this.errors.barcode =
          "Código de barras deve conter pelo menos 6 caracteres.";
      } else {
        this.errors.barcode = null;
      }
    },
    handleFileChange(event) {
      const file = event.target.files[0];
      if (file) {
        this.form.image = file;
      }
    },
    updatePrice() {
      this.validatePrice();
      if (this.form.price != 0 || this.form.price != NaN) {
        this.form.price = this.formattedPrice;
        this.price = this.formattedPrice;
      }
    },
    async submitForm() {
      this.product = { ...this.form };
      this.product.price = this.product.price.replace("R$", ",", ".");

      try {
        const response = await productService.create(this.product);
        notify({
          title: "Sucesso!",
          text: "Produto cadastrado com sucesso.",
          type: "success",
        });
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

    redirectHome() {
      this.$router.push("/home");
    },
  },
};
</script>
<style scoped>
.span-error {
  color: red;
  font-size: 12px;
}

button[disabled] {
  background-color: gray;
  cursor: not-allowed;
}
</style>
