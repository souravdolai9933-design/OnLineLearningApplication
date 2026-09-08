import { useState, useEffect } from 'react';
import { MdEdit, MdCheckCircle, MdBlock } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import StatusBadge from '../../components/common/StatusBadge';
import { toast } from 'react-toastify';

export default function Instructors() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setData([
      { userId: 10, firstName: 'Dr. Anand', lastName: 'Mehta', email: 'anand@edu.com', status: 'Active', totalCourses: 12, totalRevenue: 125000, rating: 4.8 },
      { userId: 11, firstName: 'Prof. Kavita', lastName: 'Desai', email: 'kavita@edu.com', status: 'Active', totalCourses: 8, totalRevenue: 89000, rating: 4.6 },
      { userId: 12, firstName: 'Rohan', lastName: 'Gupta', email: 'rohan@edu.com', status: 'Pending', totalCourses: 0, totalRevenue: 0, rating: 0 },
      { userId: 13, firstName: 'Neha', lastName: 'Verma', email: 'neha@edu.com', status: 'Suspended', totalCourses: 5, totalRevenue: 45000, rating: 4.2 },
    ]);
    setLoading(false);
  }, []);

  const columns = [
    { key: 'userId', label: 'ID', width: '60px' },
    { key: 'firstName', label: 'Name', render: (v, row) => `${row.firstName} ${row.lastName}` },
    { key: 'email', label: 'Email' },
    { key: 'totalCourses', label: 'Courses' },
    { key: 'rating', label: 'Rating', render: (v) => v > 0 ? `⭐ ${v}` : '—' },
    { key: 'status', label: 'Status', render: (v) => <StatusBadge status={v} /> },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div><h1 className="page-title">Instructors</h1><p className="page-subtitle">Manage instructor accounts</p></div>
      </div>
      <DataTable columns={columns} data={data} loading={loading} actions={(row) => (
        <>
          <button className="btn btn-ghost btn-sm" title="Edit"><MdEdit size={16} /></button>
          {row.status === 'Pending' && <button className="btn btn-success btn-sm" onClick={() => toast.success('Instructor approved')}>Approve</button>}
          {row.status === 'Active' && <button className="btn btn-ghost btn-sm" title="Suspend"><MdBlock size={16} color="var(--color-warning)" /></button>}
          {row.status === 'Suspended' && <button className="btn btn-ghost btn-sm" title="Activate"><MdCheckCircle size={16} color="var(--color-success)" /></button>}
        </>
      )} />
    </div>
  );
}
