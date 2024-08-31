<template>
  <div class="min-h-full">
    <NavBarComponent />
    <header class="bg-white shadow">
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <h1 class="text-3xl font-bold tracking-tight text-gray-900">
          Produtos
        </h1>
      </div>
    </header>
    <main>
      <div class="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <span class="sm:ml-3">
          <button
            type="button"
            @click="redirectRegister"
            class="inline-flex items-center rounded-md bg-slate-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-slate-600"
          >
            + Cadastrar
          </button>
        </span>
        <ProductsListComponent :products="productsList" />
      </div>
    </main>
  </div>
</template>

<script>
import emitter from "../event-bus";
import ProductsListComponent from "./ProductsListComponent.vue";
import NavBarComponent from "./NavBarComponent.vue";

export default {
  components: {
    ProductsListComponent,
    NavBarComponent,
  },
  data() {
    return {
      productsList: [
        {
          id: 1,
          name: "Earthen Bottle",
          href: "#",
          price: "$48",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-01.jpg",
          imageAlt:
            "Tall slender porcelain bottle with natural clay textured body and cork stopper.",
        },
        {
          id: 2,
          name: "Nomad Tumbler",
          href: "#",
          price: "$35",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-02.jpg",
          imageAlt:
            "Olive drab green insulated bottle with flared screw lid and flat top.",
        },
        {
          id: 3,
          name: "Focus Paper Refill",
          href: "#",
          price: "$89",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-03.jpg",
          imageAlt:
            "Person using a pen to cross a task off a productivity paper card.",
        },
        {
          id: 4,
          name: "Machined Mechanical Pencil",
          href: "#",
          price: "$35",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-04.jpg",
          imageAlt:
            "Hand holding black machined steel mechanical pencil with brass tip and top.",
        },
        {
          id: 5,
          name: "Earthen Bottle",
          href: "#",
          price: "$48",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-01.jpg",
          imageAlt:
            "Tall slender porcelain bottle with natural clay textured body and cork stopper.",
        },
        {
          id: 6,
          name: "Nomad Tumbler",
          href: "#",
          price: "$35",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-02.jpg",
          imageAlt:
            "Olive drab green insulated bottle with flared screw lid and flat top.",
        },
        {
          id: 7,
          name: "Focus Paper Refill",
          href: "#",
          price: "$89",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-03.jpg",
          imageAlt:
            "Person using a pen to cross a task off a productivity paper card.",
        },
        {
          id: 8,
          name: "Machined Mechanical Pencil",
          href: "#",
          price: "$35",
          imageSrc:
            "https://tailwindui.com/img/ecommerce-images/category-page-04-image-card-04.jpg",
          imageAlt:
            "Hand holding black machined steel mechanical pencil with brass tip and top.",
        },
      ],
    };
  },
  methods: {
    redirectRegister() {
      this.$router.push("/cadastro");
    },
  },
};
</script>
