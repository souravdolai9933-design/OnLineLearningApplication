import api from './api';

export const authService = {
  login: (data) => api.post('/Auth/login', data),
  register: (data) => api.post('/Auth/register', data),
  getProfile: (userId) => api.get(`/Auth/profile/${userId}`),
  updateProfile: (data) => api.put('/Auth/profile', data),
  changePassword: (data) => api.post('/Auth/change-password', data),
  getRoles: () => api.get('/Auth/roles'),
  getRoleById: (id) => api.get(`/Auth/roles/${id}`),
  createRole: (data) => api.post('/Auth/roles', data),
  updateRole: (id, data) => api.put(`/Auth/roles/${id}`, data),
  deleteRole: (id) => api.delete(`/Auth/roles/${id}`),
};
