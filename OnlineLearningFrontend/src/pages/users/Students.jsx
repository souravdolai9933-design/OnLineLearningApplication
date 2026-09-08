import { useState, useEffect } from 'react';
import { MdEdit, MdDelete, MdBlock, MdCheckCircle } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import StatusBadge from '../../components/common/StatusBadge';
import ConfirmDialog from '../../components/common/ConfirmDialog';
import { authService } from '../../services/authService';
import { formatDate } from '../../utils/helpers';
import { toast } from 'react-toastify';

export default function Students() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleteId, setDeleteId] = useState(null);

  useEffect(() => { loadUsers(); }, []);

  const loadUsers = async () => {
    setLoading(true);
    try {
      // No bulk getUsers endpoint; use mock data for demo
      setUsers([
        { userId: 1, firstName: 'Rahul', lastName: 'Sharma', email: 'rahul@example.com', phone: '9876543210', status: 'Active', createdAt: '2026-01-15' },
        { userId: 2, firstName: 'Priya', lastName: 'Patel', email: 'priya@example.com', phone: '9876543211', status: 'Active', createdAt: '2026-02-20' },
        { userId: 3, firstName: 'Amit', lastName: 'Kumar', email: 'amit@example.com', phone: '9876543212', status: 'Blocked', createdAt: '2026-03-10' },
        { userId: 4, firstName: 'Sneha', lastName: 'Reddy', email: 'sneha@example.com', phone: '9876543213', status: 'Active', createdAt: '2026-04-05' },
        { userId: 5, firstName: 'Vikram', lastName: 'Singh', email: 'vikram@example.com', phone: '9876543214', status: 'Inactive', createdAt: '2026-05-18' },
      ]);
    } finally { setLoading(false); }
  };

  const columns = [
    { key: 'userId', label: 'ID', width: '60px' },
    { key: 'firstName', label: 'First Name' },
    { key: 'lastName', label: 'Last Name' },
    { key: 'email', label: 'Email' },
    { key: 'phone', label: 'Phone' },
    { key: 'status', label: 'Status', render: (val) => <StatusBadge status={val} /> },
    { key: 'createdAt', label: 'Joined', render: (val) => formatDate(val) },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div><h1 className="page-title">Students</h1><p className="page-subtitle">Manage student accounts</p></div>
      </div>
      <DataTable columns={columns} data={users} loading={loading} actions={(row) => (
        <>
          <button className="btn btn-ghost btn-sm" title="Edit"><MdEdit size={16} /></button>
          <button className="btn btn-ghost btn-sm" title={row.status === 'Active' ? 'Block' : 'Activate'}>
            {row.status === 'Active' ? <MdBlock size={16} color="var(--color-danger)" /> : <MdCheckCircle size={16} color="var(--color-success)" />}
          </button>
          <button className="btn btn-ghost btn-sm" title="Delete" onClick={() => setDeleteId(row.userId)}><MdDelete size={16} color="var(--color-danger)" /></button>
        </>
      )} />
      <ConfirmDialog isOpen={!!deleteId} onClose={() => setDeleteId(null)} onConfirm={() => { toast.success('Student deleted'); setDeleteId(null); }} message="Are you sure you want to delete this student?" />
    </div>
  );
}
