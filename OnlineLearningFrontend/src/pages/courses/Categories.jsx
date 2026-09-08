import { useState, useEffect } from 'react';
import { MdAdd, MdEdit, MdDelete } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import Modal from '../../components/common/Modal';
import StatusBadge from '../../components/common/StatusBadge';
import ConfirmDialog from '../../components/common/ConfirmDialog';
import { courseService } from '../../services/courseService';
import { formatDate } from '../../utils/helpers';
import { toast } from 'react-toastify';

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editItem, setEditItem] = useState(null);
  const [deleteId, setDeleteId] = useState(null);
  const [form, setForm] = useState({ categoryName: '', description: '', categoryImage: '' });

  useEffect(() => { loadData(); }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const res = await courseService.getCategories();
      setCategories(Array.isArray(res.data) ? res.data : []);
    } catch {
      setCategories([]);
    } finally { setLoading(false); }
  };

  const openCreate = () => { setEditItem(null); setForm({ categoryName: '', description: '', categoryImage: '' }); setShowModal(true); };
  const openEdit = (item) => { setEditItem(item); setForm({ categoryName: item.categoryName, description: item.description || '', categoryImage: item.categoryImage || '' }); setShowModal(true); };

  const handleSave = async () => {
    if (!form.categoryName.trim()) return toast.error('Category name is required');
    try {
      if (editItem) {
        await courseService.updateCategory(editItem.categoryId, { ...form, categoryId: editItem.categoryId, status: editItem.status });
        toast.success('Category updated');
      } else {
        await courseService.createCategory(form);
        toast.success('Category created');
      }
      setShowModal(false);
      loadData();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to save category');
    }
  };

  const handleDelete = async () => {
    try {
      await courseService.deleteCategory(deleteId);
      toast.success('Category deleted');
      setDeleteId(null);
      loadData();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to delete');
      setDeleteId(null);
    }
  };

  const columns = [
    { key: 'categoryId', label: 'ID', width: '60px' },
    { key: 'categoryName', label: 'Category Name' },
    { key: 'description', label: 'Description', render: (v) => v || '—' },
    { key: 'status', label: 'Status', render: (v) => <StatusBadge status={v} /> },
    { key: 'createdAt', label: 'Created', render: (v) => formatDate(v) },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div><h1 className="page-title">Categories</h1><p className="page-subtitle">Manage course categories</p></div>
        <button className="btn btn-primary" onClick={openCreate}><MdAdd /> Add Category</button>
      </div>

      <DataTable columns={columns} data={categories} loading={loading} actions={(row) => (
        <>
          <button className="btn btn-ghost btn-sm" onClick={() => openEdit(row)}><MdEdit size={16} /></button>
          <button className="btn btn-ghost btn-sm" onClick={() => setDeleteId(row.categoryId)}><MdDelete size={16} color="var(--color-danger)" /></button>
        </>
      )} />

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editItem ? 'Edit Category' : 'Add Category'}>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div className="form-group">
            <label className="form-label">Category Name *</label>
            <input className="form-input" placeholder="e.g. Programming" value={form.categoryName} onChange={e => setForm({ ...form, categoryName: e.target.value })} autoFocus />
          </div>
          <div className="form-group">
            <label className="form-label">Description</label>
            <textarea className="form-input form-textarea" placeholder="Brief description..." value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
          </div>
          <div className="form-group">
            <label className="form-label">Image URL</label>
            <input className="form-input" placeholder="https://..." value={form.categoryImage} onChange={e => setForm({ ...form, categoryImage: e.target.value })} />
          </div>
          <div style={{ display: 'flex', gap: '0.75rem', justifyContent: 'flex-end', marginTop: '0.5rem' }}>
            <button className="btn btn-outline" onClick={() => setShowModal(false)}>Cancel</button>
            <button className="btn btn-primary" onClick={handleSave}>{editItem ? 'Update' : 'Create'}</button>
          </div>
        </div>
      </Modal>

      <ConfirmDialog isOpen={!!deleteId} onClose={() => setDeleteId(null)} onConfirm={handleDelete} message="Are you sure you want to delete this category? All associated courses may be affected." />
    </div>
  );
}
