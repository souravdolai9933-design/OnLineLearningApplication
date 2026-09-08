export const formatDate = (dateStr) => {
  if (!dateStr) return '—';
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric', month: 'short', day: 'numeric'
  });
};

export const formatDateTime = (dateStr) => {
  if (!dateStr) return '—';
  return new Date(dateStr).toLocaleString('en-US', {
    year: 'numeric', month: 'short', day: 'numeric',
    hour: '2-digit', minute: '2-digit'
  });
};

export const formatCurrency = (amount) => {
  if (amount == null) return '₹0';
  return new Intl.NumberFormat('en-IN', {
    style: 'currency', currency: 'INR', minimumFractionDigits: 0
  }).format(amount);
};

export const truncateText = (text, maxLen = 50) => {
  if (!text) return '';
  return text.length > maxLen ? text.substring(0, maxLen) + '...' : text;
};

export const getInitials = (firstName, lastName) => {
  return `${(firstName || '')[0] || ''}${(lastName || '')[0] || ''}`.toUpperCase();
};

export const getStatusColor = (status) => {
  const map = {
    Active: 'success', Published: 'success', Approved: 'success', Completed: 'success', Paid: 'success',
    Inactive: 'warning', Pending: 'warning', Draft: 'warning', InProgress: 'warning',
    Blocked: 'danger', Rejected: 'danger', Cancelled: 'danger', Suspended: 'danger', Revoked: 'danger',
    Open: 'info', Assigned: 'info',
  };
  return map[status] || 'default';
};
