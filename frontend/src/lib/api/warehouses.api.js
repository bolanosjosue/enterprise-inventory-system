import { apiClient } from './client';

export const warehousesApi = {
  getAll: async () => {
    const response = await apiClient.get('/warehouses');
    return response.data;
  },

  create: async (data) => {
    const response = await apiClient.post('/warehouses', data);
    return response.data;
  },

  update: async (id, data) => {
    const response = await apiClient.put(`/warehouses/${id}`, { ...data, id });
    return response.data;
  }
};