import { apiClient } from './client';

export const usersApi = {
  getAll: async () => {
    const response = await apiClient.get('/users');
    return response.data;
  },

  updateRole: async (userId, role) => {
    const response = await apiClient.put(`/users/${userId}/role`, { userId, role });
    return response.data;
  },

  toggleStatus: async (userId) => {
    const response = await apiClient.put(`/users/${userId}/toggle-status`);
    return response.data;
  }
};