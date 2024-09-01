import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://localhost:44397/api',
  headers: {
    Authorization: `Bearer ${localStorage.getItem("token").replace(/"/g, '')}`,
    'Content-Type': 'application/json'
  }
});

export const productService = {
  create: async (product) => {
    try {
      const response = await instance.post('/products/new-product', product);
      return response.data.data;
    } catch (error) {
      throw error;
    }
  },
  update: async (product) => {
    try {
      const response = await instance.put('/products/update', product);
      return response.data.data;
    } catch (error) {
      throw error;
    }
  },
  delete: async (productId) => {
    try {
      const response = await instance.delete(`/products/${productId}`);
      return response.data.data;
    } catch (error) {
      throw error;
    }
  },
  listPage: async (requestPayload) => {
    try {
      const response = await instance.post(`/products/all-page`, requestPayload);
      return response.data.data;
    } catch (error) {
      throw error;
    }
  }
};