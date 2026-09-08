import { useState, useMemo } from 'react';
import { MdSearch, MdChevronLeft, MdChevronRight } from 'react-icons/md';
import LoadingSpinner from './LoadingSpinner';
import './DataTable.css';

export default function DataTable({ columns, data = [], loading, searchable = true, pageSize = 10, actions, emptyMessage = 'No records found' }) {
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const [sortCol, setSortCol] = useState(null);
  const [sortDir, setSortDir] = useState('asc');

  const filtered = useMemo(() => {
    if (!search.trim()) return data;
    const q = search.toLowerCase();
    return data.filter(row =>
      columns.some(col => {
        const val = row[col.key];
        return val != null && String(val).toLowerCase().includes(q);
      })
    );
  }, [data, search, columns]);

  const sorted = useMemo(() => {
    if (!sortCol) return filtered;
    return [...filtered].sort((a, b) => {
      const va = a[sortCol] ?? '', vb = b[sortCol] ?? '';
      const cmp = typeof va === 'number' ? va - vb : String(va).localeCompare(String(vb));
      return sortDir === 'asc' ? cmp : -cmp;
    });
  }, [filtered, sortCol, sortDir]);

  const totalPages = Math.max(1, Math.ceil(sorted.length / pageSize));
  const paged = sorted.slice((page - 1) * pageSize, page * pageSize);

  const handleSort = (key) => {
    if (sortCol === key) setSortDir(d => d === 'asc' ? 'desc' : 'asc');
    else { setSortCol(key); setSortDir('asc'); }
  };

  if (loading) return <LoadingSpinner text="Loading data..." />;

  return (
    <div className="datatable-wrapper glass-card">
      {searchable && (
        <div className="datatable-toolbar">
          <div className="datatable-search">
            <MdSearch size={18} />
            <input type="text" placeholder="Search..." value={search} onChange={e => { setSearch(e.target.value); setPage(1); }} className="form-input" />
          </div>
          <span className="datatable-count">{filtered.length} record{filtered.length !== 1 ? 's' : ''}</span>
        </div>
      )}
      <div className="datatable-scroll">
        <table className="datatable">
          <thead>
            <tr>
              {columns.map(col => (
                <th key={col.key} onClick={() => col.sortable !== false && handleSort(col.key)} className={col.sortable !== false ? 'sortable' : ''} style={col.width ? { width: col.width } : {}}>
                  {col.label}
                  {sortCol === col.key && <span className="sort-icon">{sortDir === 'asc' ? ' ↑' : ' ↓'}</span>}
                </th>
              ))}
              {actions && <th style={{ width: 120 }}>Actions</th>}
            </tr>
          </thead>
          <tbody>
            {paged.length === 0 ? (
              <tr><td colSpan={columns.length + (actions ? 1 : 0)} className="datatable-empty">{emptyMessage}</td></tr>
            ) : (
              paged.map((row, i) => (
                <tr key={row.id || row[columns[0]?.key] || i}>
                  {columns.map(col => (
                    <td key={col.key}>{col.render ? col.render(row[col.key], row) : (row[col.key] ?? '—')}</td>
                  ))}
                  {actions && <td><div className="datatable-actions">{actions(row)}</div></td>}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
      {totalPages > 1 && (
        <div className="datatable-pagination">
          <button className="btn btn-ghost btn-sm" onClick={() => setPage(p => Math.max(1, p - 1))} disabled={page === 1}><MdChevronLeft /></button>
          <span className="page-info">Page {page} of {totalPages}</span>
          <button className="btn btn-ghost btn-sm" onClick={() => setPage(p => Math.min(totalPages, p + 1))} disabled={page === totalPages}><MdChevronRight /></button>
        </div>
      )}
    </div>
  );
}
