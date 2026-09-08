import { useState } from 'react';
import { MdAdd, MdEdit, MdDelete } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import Modal from '../../components/common/Modal';
import ConfirmDialog from '../../components/common/ConfirmDialog';
import { toast } from 'react-toastify';

export default function Admins() {
  const [data] = useState([
    { userId: 100, firstName: 'Super', lastName: 'Admin', email: 'admin@platform.com', status: 'Active', permissions: 'Full Access' },
  ]);
  const [showModal, setShowModal] = useState(false);

  const columns = [
    { key: 'userId', label: 'ID', width: '60px' },
    { key: 'firstName', label: 'Name', render: (v, row) => `${row.firstName} ${row.lastName}` },
    { key: 'email', label: 'Email' },
    { key: 'permissions', label: 'Permissions' },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div><h1 className="page-title">Admins</h1><p className="page-subtitle">Manage admin accounts and permissions</p></div>
        <button className="btn btn-primary" onClick={() => setShowModal(true)}><MdAdd /> Add Admin</button>
      </div>
      <DataTable columns={columns} data={data} loading={false} actions={(row) => (
        <>
          <button className="btn btn-ghost btn-sm"><MdEdit size={16} /></button>
          <button className="btn btn-ghost btn-sm"><MdDelete size={16} color="var(--color-danger)" /></button>
        </>
      )} />
      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title="Add Admin">
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div className="form-group"><label className="form-label">First Name</label><input className="form-input" placeholder="Enter first name" /></div>
          <div className="form-group"><label className="form-label">Last Name</label><input className="form-input" placeholder="Enter last name" /></div>
          <div className="form-group"><label className="form-label">Email</label><input className="form-input" type="email" placeholder="admin@example.com" /></div>
          <div className="form-group"><label className="form-label">Password</label><input className="form-input" type="password" placeholder="Set password" /></div>
          <div style={{ display: 'flex', gap: '0.75rem', justifyContent: 'flex-end', marginTop: '0.5rem' }}>
            <button className="btn btn-outline" onClick={() => setShowModal(false)}>Cancel</button>
            <button className="btn btn-primary" onClick={() => { toast.success('Admin added successfully'); setShowModal(false); }}>Create Admin</button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
