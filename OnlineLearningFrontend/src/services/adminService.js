import api from './api';

export const adminService = {
  getAdminDashboard: () => api.get('/Admin/dashboard/admin'),
  getInstructorDashboard: (id) => api.get(`/Admin/dashboard/instructor/${id}`),

  requestPayout: (data) => api.post('/Admin/payouts/request', data),
  getPayouts: (params) => api.get('/Admin/payouts', { params }),
  getPayoutById: (id) => api.get(`/Admin/payouts/${id}`),
  approvePayout: (id, data) => api.post(`/Admin/payouts/${id}/approve`, data),
  rejectPayout: (id, data) => api.post(`/Admin/payouts/${id}/reject`, data),

  getSiteSettings: () => api.get('/Admin/settings'),
  getSiteSettingByKey: (key) => api.get(`/Admin/settings/${key}`),
  updateSiteSetting: (key, data) => api.put(`/Admin/settings/${key}`, data),

  getAuditLogs: (params) => api.get('/Admin/audit-logs', { params }),
};
