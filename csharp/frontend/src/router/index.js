import { createRouter, createWebHashHistory } from 'vue-router';
import Index from "@/views/Index";
import days from "@/components/days";

const routes = [
  {
    path: "/",
    name: "Index",
    component: Index
  },
];

days.forEach((component, index) => {
  routes.push({
    path: `/${index+1}`,
    component: component
  });
});

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
