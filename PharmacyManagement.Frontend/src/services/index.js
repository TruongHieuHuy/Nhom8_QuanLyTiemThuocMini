import apiClient from './apiClient';

// Medicine Services
export const medicineService = {
  getAll: () => apiClient.get('/medicines'),
  getById: (id) => apiClient.get(`/medicines/${id}`),
  search: (searchTerm) => apiClient.get(`/medicines/search/${searchTerm}`),
  getByGroup: (groupId) => apiClient.get(`/medicines/group/${groupId}`),
  getLowStock: () => apiClient.get('/medicines/low-stock'),
  getExpired: () => apiClient.get('/medicines/expired'),
  create: (data) => apiClient.post('/medicines', data),
  update: (data) => apiClient.put('/medicines', data),
  delete: (id) => apiClient.delete(`/medicines/${id}`),
};

// Customer Services
export const customerService = {
  getAll: () => apiClient.get('/customers'),
  getById: (id) => apiClient.get(`/customers/${id}`),
  getDetail: (id) => apiClient.get(`/customers/detail/${id}`),
  search: (searchTerm) => apiClient.get(`/customers/search/${searchTerm}`),
  create: (data) => apiClient.post('/customers', data),
  update: (data) => apiClient.put('/customers', data),
  delete: (id) => apiClient.delete(`/customers/${id}`),
};

// Order Services
export const orderService = {
  getAll: () => apiClient.get('/orders'),
  getById: (id) => apiClient.get(`/orders/${id}`),
  getByCustomer: (customerId) => apiClient.get(`/orders/customer/${customerId}`),
  getByDateRange: (startDate, endDate) =>
    apiClient.get(`/orders/date-range?startDate=${startDate}&endDate=${endDate}`),
  create: (data) => apiClient.post('/orders', data),
  updateStatus: (id, status) => apiClient.put(`/orders/${id}/status`, status),
  delete: (id) => apiClient.delete(`/orders/${id}`),
};

// Report Services
export const reportService = {
  getRevenueByDate: (date) => apiClient.get(`/reports/revenue/date/${date}`),
  getRevenueByRange: (startDate, endDate) =>
    apiClient.get(`/reports/revenue/range?startDate=${startDate}&endDate=${endDate}`),
  getTopMedicines: (count = 10) => apiClient.get(`/reports/top-medicines/${count}`),
  getTopCustomers: (count = 10) => apiClient.get(`/reports/top-customers/${count}`),
  getOutOfStock: () => apiClient.get('/reports/out-of-stock'),
  getOrdersCount: (date) => apiClient.get(`/reports/orders-count/${date}`),
};
