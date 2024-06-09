import { apiClient } from './client';

export const warehousesApi = {
  getAll: async () => {
    const response = await apiClient.get('/warehouses');
    return response.data;
  }
};