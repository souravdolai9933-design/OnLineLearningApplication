import { useState, useEffect } from 'react';
import { MdAdd, MdEdit, MdDelete, MdCheckCircle, MdCancel, MdSearch } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import Modal from '../../components/common/Modal';
import StatusBadge from '../../components/common/StatusBadge';
import ConfirmDialog from '../../components/common/ConfirmDialog';
import { courseService } from '../../services/courseService';
import { formatCurrency, formatDate } from '../../utils/helpers';
import { toast } from 'react-toastify';

export default function Courses() {
  const [courses, setCourses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editItem, setEditItem] = useState(null);
  const [deleteId, setDeleteId] = useState(null);

  const [form, setForm] = useState({
    title: '',
    shortDescription: '',
    description: '',
    price: '',
    discountPrice: '',
    categoryId: '',
    courseLevel: 'Beginner',
    language: 'English',
    thumbnail: '',
    promoVideoUrl: '',
    instructorId: 1
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [coursesRes, catsRes] = await Promise.allSettled([
        courseService.getCourses(),
        courseService.getCategories()
      ]);

      if (coursesRes.status === 'fulfilled' && Array.isArray(coursesRes.value.data)) {
        setCourses(coursesRes.value.data);
      } else {
        // Mock fallback if db is empty
        setCourses([
          { courseId: 1, title: 'Complete React & Next.js Bootcamp', instructorName: 'John Doe', categoryName: 'Web Development', price: 2999, discountPrice: 1999, status: 'Published', courseLevel: 'All Levels', totalLessons: 42, averageRating: 4.8, createdAt: '2026-02-10' },
          { courseId: 2, title: 'ASP.NET Core & Dapper Enterprise Architecture', instructorName: 'Alex Smith', categoryName: 'Backend Development', price: 3499, discountPrice: 2499, status: 'Pending', courseLevel: 'Advanced', totalLessons: 55, averageRating: 4.9, createdAt: '2026-02-14' },
          { courseId: 3, title: 'Machine Learning & Python Masterclass', instructorName: 'Sarah Khan', categoryName: 'Data Science', price: 4999, discountPrice: 3999, status: 'Draft', courseLevel: 'Intermediate', totalLessons: 60, averageRating: 4.7, createdAt: '2026-03-01' }
        ]);
      }

      if (catsRes.status === 'fulfilled' && Array.isArray(catsRes.value.data)) {
        setCategories(catsRes.value.data);
      }
    } catch {
      toast.error('Failed to load courses');
    } finally {
      setLoading(false);
    }
  };

  const openCreate = () => {
    setEditItem(null);
    setForm({
      title: '',
      shortDescription: '',
      description: '',
      price: '',
      discountPrice: '',
      categoryId: categories[0]?.categoryId || 1,
      courseLevel: 'Beginner',
      language: 'English',
      thumbnail: '',
      promoVideoUrl: '',
      instructorId: 1
    });
    setShowModal(true);
  };

  const openEdit = (course) => {
    setEditItem(course);
    setForm({
      title: course.title,
      shortDescription: course.shortDescription || '',
      description: course.description || '',
      price: course.price,
      discountPrice: course.discountPrice || '',
      categoryId: course.categoryId,
      courseLevel: course.courseLevel || 'Beginner',
      language: course.language || 'English',
      thumbnail: course.thumbnail || '',
      promoVideoUrl: course.promoVideoUrl || '',
      instructorId: course.instructorId || 1
    });
    setShowModal(true);
  };

  const handleSave = async () => {
    if (!form.title.trim() || !form.price) {
      return toast.error('Title and price are required');
    }

    try {
      const payload = {
        ...form,
        price: parseFloat(form.price),
        discountPrice: form.discountPrice ? parseFloat(form.discountPrice) : null,
        categoryId: parseInt(form.categoryId) || 1,
        instructorId: parseInt(form.instructorId) || 1
      };

      if (editItem) {
        await courseService.updateCourse(editItem.courseId, { ...payload, courseId: editItem.courseId });
        toast.success('Course updated successfully');
      } else {
        await courseService.createCourse(payload);
        toast.success('Course created successfully');
      }
      setShowModal(false);
      loadData();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to save course');
    }
  };

  const togglePublish = async (course) => {
    try {
      if (course.status === 'Published') {
        await courseService.unpublishCourse(course.courseId, course.instructorId || 1);
        toast.info('Course unpublished');
      } else {
        await courseService.publishCourse(course.courseId, course.instructorId || 1);
        toast.success('Course published');
      }
      loadData();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Action failed');
    }
  };

  const handleDelete = async () => {
    try {
      await courseService.deleteCourse(deleteId, 1);
      toast.success('Course deleted');
      setDeleteId(null);
      loadData();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to delete');
      setDeleteId(null);
    }
  };

  const columns = [
    { key: 'courseId', label: 'ID', width: '60px' },
    { key: 'title', label: 'Course Title', render: (v, r) => (
      <div>
        <strong>{v}</strong>
        <div style={{ fontSize: '11px', color: 'var(--text-muted)' }}>{r.categoryName || 'General'} • {r.courseLevel}</div>
      </div>
    )},
    { key: 'instructorName', label: 'Instructor', render: (v) => v || 'Staff' },
    { key: 'price', label: 'Price', render: (v, r) => (
      <div>
        <span>{formatCurrency(v)}</span>
        {r.discountPrice && <div style={{ fontSize: '11px', color: 'var(--color-success)' }}>Disc: {formatCurrency(r.discountPrice)}</div>}
      </div>
    )},
    { key: 'status', label: 'Status', render: (v) => <StatusBadge status={v} /> },
    { key: 'createdAt', label: 'Created', render: (v) => formatDate(v) },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div>
          <h1 className="page-title">Course Management</h1>
          <p className="page-subtitle">View, search, edit and publish all courses</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}><MdAdd /> Create Course</button>
      </div>

      <DataTable
        columns={columns}
        data={courses}
        loading={loading}
        actions={(row) => (
          <>
            <button className="btn btn-ghost btn-sm" title="Edit" onClick={() => openEdit(row)}><MdEdit size={16} /></button>
            <button className="btn btn-ghost btn-sm" title={row.status === 'Published' ? 'Unpublish' : 'Publish'} onClick={() => togglePublish(row)}>
              {row.status === 'Published' ? <MdCancel size={16} color="var(--color-warning)" /> : <MdCheckCircle size={16} color="var(--color-success)" />}
            </button>
            <button className="btn btn-ghost btn-sm" title="Delete" onClick={() => setDeleteId(row.courseId)}><MdDelete size={16} color="var(--color-danger)" /></button>
          </>
        )}
      />

      <Modal isOpen={showModal} onClose={() => setShowModal(false)} title={editItem ? 'Edit Course' : 'Create Course'} size="lg">
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
          <div className="form-group" style={{ gridColumn: 'span 2' }}>
            <label className="form-label">Course Title *</label>
            <input className="form-input" placeholder="e.g. Master C# & .NET 10" value={form.title} onChange={e => setForm({ ...form, title: e.target.value })} />
          </div>
          <div className="form-group" style={{ gridColumn: 'span 2' }}>
            <label className="form-label">Short Description</label>
            <input className="form-input" placeholder="Brief summary of the course" value={form.shortDescription} onChange={e => setForm({ ...form, shortDescription: e.target.value })} />
          </div>
          <div className="form-group">
            <label className="form-label">Category</label>
            <select className="form-input form-select" value={form.categoryId} onChange={e => setForm({ ...form, categoryId: e.target.value })}>
              {categories.map(c => <option key={c.categoryId} value={c.categoryId}>{c.categoryName}</option>)}
              {categories.length === 0 && <option value="1">General</option>}
            </select>
          </div>
          <div className="form-group">
            <label className="form-label">Level</label>
            <select className="form-input form-select" value={form.courseLevel} onChange={e => setForm({ ...form, courseLevel: e.target.value })}>
              <option value="Beginner">Beginner</option>
              <option value="Intermediate">Intermediate</option>
              <option value="Advanced">Advanced</option>
              <option value="All Levels">All Levels</option>
            </select>
          </div>
          <div className="form-group">
            <label className="form-label">Price (INR) *</label>
            <input className="form-input" type="number" placeholder="2999" value={form.price} onChange={e => setForm({ ...form, price: e.target.value })} />
          </div>
          <div className="form-group">
            <label className="form-label">Discount Price (INR)</label>
            <input className="form-input" type="number" placeholder="1999" value={form.discountPrice} onChange={e => setForm({ ...form, discountPrice: e.target.value })} />
          </div>
          <div className="form-group" style={{ gridColumn: 'span 2' }}>
            <label className="form-label">Thumbnail URL</label>
            <input className="form-input" placeholder="https://..." value={form.thumbnail} onChange={e => setForm({ ...form, thumbnail: e.target.value })} />
          </div>
          <div className="form-group" style={{ gridColumn: 'span 2' }}>
            <label className="form-label">Full Description</label>
            <textarea className="form-input form-textarea" placeholder="Detailed syllabus and outcomes..." value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
          </div>
        </div>
        <div style={{ display: 'flex', gap: '0.75rem', justifyContent: 'flex-end', marginTop: '1.25rem' }}>
          <button className="btn btn-outline" onClick={() => setShowModal(false)}>Cancel</button>
          <button className="btn btn-primary" onClick={handleSave}>{editItem ? 'Save Changes' : 'Create Course'}</button>
        </div>
      </Modal>

      <ConfirmDialog isOpen={!!deleteId} onClose={() => setDeleteId(null)} onConfirm={handleDelete} message="Are you sure you want to permanently delete this course?" />
    </div>
  );
}
