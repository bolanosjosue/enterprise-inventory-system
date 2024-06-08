import { apiClient } from './client';

export const suppliersApi = {
  getAll: async () => {
    const response = await apiClient.get('/suppliers');
    return response.data;
  },

  create: async (data) => {
    const response = await apiClient.post('/suppliers', data);
    return response.data;
  },

  update: async (id, data) => {
    const response = await apiClient.put(`/suppliers/${id}`, { ...data, id });
    return response.data;
  }
};