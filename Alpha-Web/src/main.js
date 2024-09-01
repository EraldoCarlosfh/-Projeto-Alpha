import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import Notifications from '@kyvg/vue3-notification'
import router from './router';
import { VueMaskDirective } from 'v-mask';
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Paginator from 'primevue/paginator';

const app = createApp(App);

app.component('Paginator', Paginator);
app.use(router);
app.directive('mask', VueMaskDirective);
app.use(Notifications);
app.use(PrimeVue, {
  theme: {
    preset: Aura
  }
});
app.mount('#app');