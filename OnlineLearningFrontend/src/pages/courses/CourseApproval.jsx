import { useState, useEffect } from 'react';
import { MdCheck, MdClose, MdHistory, MdInfo } from 'react-icons/md';
import DataTable from '../../components/common/DataTable';
import Modal from '../../components/common/Modal';
import StatusBadge from '../../components/common/StatusBadge';
import { courseService } from '../../services/courseService';
import { formatDate, formatCurrency } from '../../utils/helpers';
import { toast } from 'react-toastify';

export default function CourseApproval() {
  const [pendingCourses, setPendingCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedCourse, setSelectedCourse] = useState(null);
  const [actionType, setActionType] = useState(null); // 'approve' or 'reject'
  const [remarks, setRemarks] = useState('');
  const [historyModal, setHistoryModal] = useState(false);
  const [historyData, setHistoryData] = useState([]);

  useEffect(() => {
    loadPending();
  }, []);

  const loadPending = async () => {
    setLoading(true);
    try {
      const res = await courseService.getPendingCourses();
      if (Array.isArray(res.data)) {
        setPendingCourses(res.data);
      } else {
        setPendingCourses([
          { courseId: 2, title: 'ASP.NET Core & Dapper Enterprise Architecture', instructorName: 'Alex Smith', categoryName: 'Backend Development', price: 3499, status: 'Pending', createdAt: '2026-02-14' },
          { courseId: 4, title: 'Fullstack Next.js & GraphQL Pro', instructorName: 'David Lee', categoryName: 'Web Development', price: 4499, status: 'Pending', createdAt: '2026-02-18' }
        ]);
      }
    } catch {
      setPendingCourses([
        { courseId: 2, title: 'ASP.NET Core & Dapper Enterprise Architecture', instructorName: 'Alex Smith', categoryName: 'Backend Development', price: 3499, status: 'Pending', createdAt: '2026-02-14' }
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleDecision = async () => {
    if (actionType === 'reject' && !remarks.trim()) {
      return toast.error('Please specify a rejection reason');
    }

    try {
      const payload = {
        courseId: selectedCourse.courseId,
        adminId: 1,
        remarks: remarks || 'Approved by administrator'
      };

      if (actionType === 'approve') {
        await courseService.approveCourse(selectedCourse.courseId, payload);
        toast.success(`Course "${selectedCourse.title}" approved!`);
      } else {
        await courseService.rejectCourse(selectedCourse.courseId, payload);
        toast.warn(`Course "${selectedCourse.title}" rejected`);
      }

      setSelectedCourse(null);
      setActionType(null);
      setRemarks('');
      loadPending();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Action failed');
    }
  };

  const viewHistory = async (course) => {
    try {
      const res = await courseService.getCourseApprovalHistory(course.courseId);
      setHistoryData(Array.isArray(res.data) ? res.data : []);
    } catch {
      setHistoryData([
        { approvalId: 1, courseTitle: course.title, adminName: 'Admin', action: 'Submitted', remarks: 'Course submitted for review', actionDate: '2026-02-14' }
      ]);
    }
    setHistoryModal(true);
  };

  const columns = [
    { key: 'courseId', label: 'ID', width: '60px' },
    { key: 'title', label: 'Course Title' },
    { key: 'instructorName', label: 'Instructor' },
    { key: 'categoryName', label: 'Category' },
    { key: 'price', label: 'Price', render: (v) => formatCurrency(v) },
    { key: 'status', label: 'Status', render: (v) => <StatusBadge status={v} /> },
    { key: 'createdAt', label: 'Submitted On', render: (v) => formatDate(v) },
  ];

  return (
    <div className="animate-fade-in">
      <div className="page-header">
        <div>
          <h1 className="page-title">Course Approval Workflow</h1>
          <p className="page-subtitle">Review submitted instructor courses before publishing to students</p>
        </div>
      </div>

      <DataTable
        columns={columns}
        data={pendingCourses}
        loading={loading}
        emptyMessage="No pending courses waiting for approval 🎉"
        actions={(row) => (
          <>
            <button className="btn btn-success btn-sm" title="Approve" onClick={() => { setSelectedCourse(row); setActionType('approve'); }}>
              <MdCheck /> Approve
            </button>
            <button className="btn btn-danger btn-sm" title="Reject" onClick={() => { setSelectedCourse(row); setActionType('reject'); }}>
              <MdClose /> Reject
            </button>
            <button className="btn btn-ghost btn-sm" title="History" onClick={() => viewHistory(row)}>
              <MdHistory size={16} />
            </button>
          </>
        )}
      />

      <Modal isOpen={!!selectedCourse} onClose={() => setSelectedCourse(null)} title={actionType === 'approve' ? 'Approve Course' : 'Reject Course'}>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <p style={{ color: 'var(--text-secondary)' }}>
            Course: <strong style={{ color: 'var(--text-primary)' }}>{selectedCourse?.title}</strong>
          </p>
          <div className="form-group">
            <label className="form-label">
              {actionType === 'approve' ? 'Optional Remarks' : 'Rejection Reason *'}
            </label>
            <textarea
              className="form-input form-textarea"
              placeholder={actionType === 'approve' ? 'Great curriculum, approved for live catalog.' : 'Please add more video lessons and proper thumbnail...'}
              value={remarks}
              onChange={e => setRemarks(e.target.value)}
            />
          </div>
          <div style={{ display: 'flex', gap: '0.75rem', justifyContent: 'flex-end', marginTop: '0.5rem' }}>
            <button className="btn btn-outline" onClick={() => setSelectedCourse(null)}>Cancel</button>
            <button className={`btn btn-${actionType === 'approve' ? 'success' : 'danger'}`} onClick={handleDecision}>
              Confirm {actionType === 'approve' ? 'Approval' : 'Rejection'}
            </button>
          </div>
        </div>
      </Modal>

      <Modal isOpen={historyModal} onClose={() => setHistoryModal(false)} title="Approval Audit History" size="md">
        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
          {historyData.length === 0 ? (
            <p style={{ color: 'var(--text-muted)' }}>No audit history found.</p>
          ) : (
            historyData.map(h => (
              <div key={h.approvalId} style={{ padding: '0.75rem', background: 'var(--bg-tertiary)', borderRadius: 'var(--radius-md)' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '4px' }}>
                  <strong style={{ color: 'var(--accent-primary)' }}>{h.action}</strong>
                  <span style={{ fontSize: '12px', color: 'var(--text-muted)' }}>{formatDate(h.actionDate)}</span>
                </div>
                <div style={{ fontSize: '13px', color: 'var(--text-secondary)' }}>{h.remarks || 'No remarks provided'}</div>
              </div>
            ))
          )}
        </div>
      </Modal>
    </div>
  );
}
