import Modal from './Modal';
import './ConfirmDialog.css';

export default function ConfirmDialog({ isOpen, onClose, onConfirm, title = 'Confirm', message, confirmText = 'Delete', variant = 'danger' }) {
  return (
    <Modal isOpen={isOpen} onClose={onClose} title={title} size="sm">
      <p className="confirm-message">{message}</p>
      <div className="confirm-actions">
        <button className="btn btn-outline" onClick={onClose}>Cancel</button>
        <button className={`btn btn-${variant}`} onClick={onConfirm}>{confirmText}</button>
      </div>
    </Modal>
  );
}
