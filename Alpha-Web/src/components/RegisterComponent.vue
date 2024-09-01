<template>
  <div
    class="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8"
  >
    <div class="sm:mx-auto sm:w-full sm:max-w-sm">
      <img
        class="mx-auto h-10 w-auto"
        src="https://tailwindui.com/img/logos/mark.svg?color=indigo&shade=600"
        alt="Your Company"
      />
      <h2
        class="mt-6 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900"
      >
        CADASTRO
      </h2>
    </div>

    <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
      <form class="space-y-6" @submit.prevent="userRegister">
        <div>
          <label class="block text-sm font-medium leading-6 text-gray-900"
            >Nome Completo</label
          >
          <div class="mt-2">
            <input
              id="fullname"
              name="fullname"
              type="text"
              v-model="form.fullName"
              @blur="validateFullName"
              required=""
              class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
            />
            <span v-if="errors.fullName">{{ errors.fullName }}</span>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium leading-6 text-gray-900"
            >E-mail</label
          >
          <div class="mt-2">
            <input
              id="email"
              name="email"
              type="email"
              v-model="form.email"
              @blur="validateEmail"
              autocomplete="email"
              required=""
              class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
            />
            <span v-if="errors.email">{{ errors.email }}</span>
          </div>
        </div>

        <div>
          <div class="flex items-center justify-between">
            <label class="block text-sm font-medium leading-6 text-gray-900"
              >Senha</label
            >
          </div>
          <div class="mt-2">
            <input
              id="password"
              name="password"
              type="password"
              v-model="form.password"
              @blur="validatePassword"
              required=""
              class="p-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
            />
            <span v-if="errors.password">{{ errors.password }}</span>
          </div>
        </div>

        <div>
          <button
            type="submit"
            :disabled="!isFormValid"
            class="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
          >
            Cadastrar
          </button>
          <router-link to="/home">
            <button
              type="submit"
              class="mt-4 flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            >
              Voltar
            </button>
          </router-link>
        </div>
      </form>
    </div>
  </div>
</template>
<script>
import { notify } from "@kyvg/vue3-notification";
import { authService } from "../services/authService";

export default {
  data() {
    return {
      form: {
        fullName: "",
        email: "",
        password: "",
      },
      errors: {
        fullName: null,
        email: null,
        password: null,
      },
    };
  },
  computed: {
    isFormValid() {
      return (
        !this.errors.fullName &&
        !this.errors.password &&
        !this.errors.email &&
        this.form.fullName &&
        this.form.password &&
        this.form.email
      );
    },
  },
  methods: {
    validateFullName() {
      if (!this.form.fullName) {
        this.errors.fullName = "O nome é obrigatório.";
      } else if (this.form.fullName.length > 30) {
        this.errors.fullName = "O nome deve conter no máximo 30 caracteres.";
      } else {
        this.errors.fullName = null;
      }
    },
    validatePassword() {
      if (!this.form.password) {
        this.errors.password = "A senha é obrigatória.";
      } else if (this.form.password.length < 6) {
        this.errors.password = "A senha deve conter pelo menos 6 caracteres.";
      } else {
        this.errors.password = null;
      }
    },
    validateEmail() {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!this.form.email) {
        this.errors.email = "O email é obrigatório.";
      } else if (!emailRegex.test(this.form.email)) {
        this.errors.email = "Insira um email válido.";
      } else {
        this.errors.email = null;
      }
    },
    async userRegister() {
      this.validatePassword();
      this.validateFullName();
      this.validateEmail();

      try {
        const user = await authService.createUser(this.form);
        if (user != null) {
          notify({
            title: "Sucesso!",
            text: "Cadastro efetuado com sucesso.",
            type: "success",
          });
          this.$router.push("login");
        }
      } catch (error) {
        notify({
          title: error.code,
          text: error.message,
          type: "error",
        });
      }
    },
  },
};
</script>

<style scoped>
span {
  color: red;
  font-size: 12px;
}

button[disabled] {
  background-color: gray;
  cursor: not-allowed;
}
</style>
