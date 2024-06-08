import { apiClient } from './client';

export const categoriesApi = {
  getAll: async () => {
    const response = await apiClient.get('/categories');
    return response.data;
  }
};