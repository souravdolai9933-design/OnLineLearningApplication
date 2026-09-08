import { useState } from 'react';
import DataTable from '../components/common/DataTable';
import StatusBadge from '../components/common/StatusBadge';
import { MdAdd } from 'react-icons/md';

export default function GenericModule({ title, subtitle, columns = [], mockData = [], actionButtonText }) {
  const [data] = useState(mockData);

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div>
          <h1 className="page-title">{title}</h1>
          <p className="page-subtitle">{subtitle}</p>
        </div>
        {actionButtonText && (
          <button className="btn btn-primary">
            <MdAdd /> {actionButtonText}
          </button>
        )}
      </div>

      <DataTable
        columns={columns.length > 0 ? columns : [
          { key: 'id', label: 'ID', width: '60px' },
          { key: 'name', label: 'Title / Description' },
          { key: 'status', label: 'Status', render: (v) => <StatusBadge status={v || 'Active'} /> },
          { key: 'date', label: 'Date' }
        ]}
        data={data.length > 0 ? data : [
          { id: 1, name: `${title} Item #1`, status: 'Active', date: '2026-02-18' },
          { id: 2, name: `${title} Item #2`, status: 'Pending', date: '2026-02-17' },
          { id: 3, name: `${title} Item #3`, status: 'Completed', date: '2026-02-15' }
        ]}
        loading={false}
      />
    </div>
  );
}
