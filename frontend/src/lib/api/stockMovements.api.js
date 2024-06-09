import { apiClient } from './client';

export const stockMovementsApi = {
  getAll: async (params = {}) => {
    const response = await apiClient.get('/stockmovements', { params });
    return response.data;
  },

  processPurchase: async (data) => {
    const response = await apiClient.post('/stockmovements/purchase', data);
    return response.data;
  },

  processSale: async (data) => {
    const response = await apiClient.post('/stockmovements/sale', data);
    return response.data;
  },

  transferStock: async (data) => {
    const response = await apiClient.post('/stockmovements/transfer', data);
    return response.data;
  },

  getStockByWarehouse: async (warehouseId) => {
    const response = await apiClient.get(`/stockmovements/warehouse/${warehouseId}/stock`);
    return response.data;
  }
};