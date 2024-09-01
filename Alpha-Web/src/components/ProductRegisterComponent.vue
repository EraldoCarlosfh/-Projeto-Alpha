<template>
  <div class="min-h-full">
    <NavBarComponent />
    <header class="bg-white shadow">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">
          {{ isEditMode ? "Editar Produto" : "Cadastro Produto" }}
        </h1>
      </div>
    </header>
    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <form @submit.prevent="productRegister">
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
                      placeholder="Digite o nome do produto"
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
                      v-model="form.barCode"
                      @blur="validateBarCode"
                      @input="onlyNumbers"
                      placeholder="Digite o código de barras"
                      class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                    />
                    <span class="span-error" v-if="errors.barCode">{{
                      errors.barCode
                    }}</span>
                  </div>
                </div>

                <div class="sm:col-span-2">
                  <label
                    class="block text-sm font-medium leading-6 text-gray-900"
                    >Preço R$</label
                  >
                  <div class="relative mt-2 rounded-md shadow-sm">
                    <div
                      :class="{
                        'pointer-events-none absolute inset-y-0 left-0 flex items-center pl-2': true,
                        'bottom-0': errors.price > 0,
                        'bottom-6': errors.price != null,
                      }"
                    >
                      <span class="text-gray-500 sm:text-sm">R$</span>
                    </div>
                    <input
                      type="text"
                      v-model="formattedValue"
                      @blur="validatePrice"
                      @input="formatCurrency"
                      placeholder="0.00"
                      class="block w-full rounded-md border-0 py-1.5 pl-7 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
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
              v-if="!isEditMode"
              type="button"
              @click="handleCancel"
              class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            >
              Limpar
            </button>
            <router-link to="/home" v-else>
              <button
                type="button"
                class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
              >
                Voltar
              </button>
            </router-link>
            <button
              type="submit"
              :disabled="!isFormValid"
              class="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            >
              {{ isEditMode ? "Atualizar" : "Cadastrar" }}
            </button>
          </div>
        </form>
      </div>
    </main>
  </div>
</template>

<script>
import { notify } from "@kyvg/vue3-notification";
import NavBarComponent from "../shared/NavBarComponent.vue";
import { productService } from "../services/productService";

export default {
  components: {
    NavBarComponent,
  },
  data() {
    return {
      form: {
        id: "",
        name: "",
        price: 0,
        barCode: "",
        image: "https://i.pravatar.cc",
      },
      errors: {
        name: null,
        price: null,
        barCode: null,
      },
      product: {
        id: "",
        name: "",
        price: 0,
        barCode: "",
        image: "https://i.pravatar.cc",
      },
      formattedValue: "",
    };
  },
  computed: {
    isEditMode() {
      return !!this.$route.params.id;
    },
    isFormValid() {
      return (
        !this.errors.name &&
        !this.errors.price &&
        !this.errors.barCode &&
        this.form.name &&
        this.form.price &&
        this.form.barCode
      );
    },
    formatCurrency() {
      let value = this.formattedValue.replace(/[^0-9,]/g, "");

      let parts = value.split(",");
      if (parts.length > 2) {
        value = parts[0] + "," + parts[1].slice(0, 2);
      }
      this.formattedValue = value;
      this.form.price = parseFloat(value.replace(",", "."));
    },
  },
  async created() {
    if (this.isEditMode) {
      await this.fetchItemData(this.$route.params.id);
    }
  },
  methods: {
    async fetchItemData(productId) {
      const response = await productService.getById(productId);
      this.form = response;
      this.formattedValue = response.price;
    },
    handleCancel() {
      (this.form.name = ""),
        (this.form.price = ""),
        (this.formattedValue = ""),
        (this.form.barCode = ""),
        (this.form.image = null);
    },
    onlyNumbers(event) {
      const valor = event.target.value;
      event.target.value = valor.replace(/[^0-9]/g, "");
      this.form.barCode = event.target.value;
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
      if (!this.formattedValue) {
        this.errors.price = "O valor é obrigatório.";
      } else if (this.formattedValue == 0) {
        this.errors.price = "O valor deve ser maior que 0.";
      } else {
        this.errors.price = null;
      }
    },
    validateBarCode() {
      if (!this.form.barCode) {
        this.errors.barCode = "Código de barras é obrigatório.";
      } else if (this.form.barCode.length != 8) {
        this.errors.barCode = "Código de barras deve conter 8 números.";
      } else {
        this.errors.barCode = null;
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
    async productRegister() {
      this.product = { ...this.form };

      if (this.isEditMode) {
        try {
          await productService.update(this.product);
          notify({
            title: "Sucesso!",
            text: `Produto: ${this.product.name} atualizado com sucesso.`,
            type: "success",
          });
          this.$router.push("/home");
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
      } else {
        try {
          const response = await productService.create(this.product);
          notify({
            title: "Sucesso!",
            text: `Produto: ${response.name} cadastrado com sucesso.`,
            type: "success",
          });
          this.$router.push("/home");
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
