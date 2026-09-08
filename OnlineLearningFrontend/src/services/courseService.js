import api from './api';

export const courseService = {
  getCategories: () => api.get('/Courses/categories'),
  getCategoryById: (id) => api.get(`/Courses/categories/${id}`),
  createCategory: (data) => api.post('/Courses/categories', data),
  updateCategory: (id, data) => api.put(`/Courses/categories/${id}`, data),
  deleteCategory: (id) => api.delete(`/Courses/categories/${id}`),

  getCourses: () => api.get('/Courses'),
  getCourseById: (id) => api.get(`/Courses/${id}`),
  searchCourses: (params) => api.get('/Courses/search', { params }),
  getCoursesByCategory: (id) => api.get(`/Courses/category/${id}`),
  getCoursesByInstructor: (id) => api.get(`/Courses/instructor/${id}`),
  createCourse: (data) => api.post('/Courses', data),
  updateCourse: (id, data) => api.put(`/Courses/${id}`, data),
  deleteCourse: (id, instructorId) => api.delete(`/Courses/${id}?instructorId=${instructorId}`),
  publishCourse: (id, instructorId) => api.put(`/Courses/${id}/publish?instructorId=${instructorId}`),
  unpublishCourse: (id, instructorId) => api.put(`/Courses/${id}/unpublish?instructorId=${instructorId}`),

  getPendingCourses: () => api.get('/Courses/pending'),
  approveCourse: (id, data) => api.post(`/Courses/${id}/approve`, data),
  rejectCourse: (id, data) => api.post(`/Courses/${id}/reject`, data),
  getCourseApprovalHistory: (id) => api.get(`/Courses/${id}/approval-history`),
};
