import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://localhost:44397/api',
});

export const authService = {
  login: async (userLogin) => {
    try {
      const response = await instance.post('/users/login', userLogin);
      if (response.data.data.token) {
        localStorage.setItem('token', JSON.stringify(token));
        localStorage.setItem('user', JSON.stringify(response.data.data));
      }
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  createUser: async (user) => {
    try {
      const response = await instance.post('/users/new-user', user);
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }
};