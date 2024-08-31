import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import Notifications from '@kyvg/vue3-notification'
import router from './router';
import { VueMaskDirective } from 'v-mask';

const app = createApp(App);

app.use(router);
app.directive('mask', VueMaskDirective);
app.use(Notifications);

app.mount('#app');