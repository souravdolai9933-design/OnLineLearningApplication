import { getStatusColor } from '../../utils/helpers';
import './StatusBadge.css';

export default function StatusBadge({ status }) {
  const color = getStatusColor(status);
  return <span className={`status-badge status-${color}`}>{status}</span>;
}
