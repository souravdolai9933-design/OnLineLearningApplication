import api from './api';

export const reviewService = {
  getCourseReviews: (courseId) => api.get(`/Reviews/course/${courseId}`),
  getReviewById: (id) => api.get(`/Reviews/${id}`),
  createReview: (data) => api.post('/Reviews', data),
  updateReview: (id, data) => api.put(`/Reviews/${id}`, data),
  deleteReview: (id, userId) => api.delete(`/Reviews/${id}?userId=${userId}`),
};
