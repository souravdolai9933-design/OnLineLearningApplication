import { MdNotifications, MdSearch } from 'react-icons/md';
import { useAuth } from '../../context/AuthContext';
import { getInitials } from '../../utils/helpers';
import './Header.css';

export default function Header() {
  const { user } = useAuth();

  return (
    <header className="app-header">
      <div className="header-search">
        <MdSearch size={18} />
        <input type="text" placeholder="Search anything..." className="header-search-input" />
      </div>
      <div className="header-right">
        <button className="btn-ghost btn-icon header-notification">
          <MdNotifications size={20} />
          <span className="notification-dot" />
        </button>
        <div className="header-profile">
          <div className="profile-avatar">{getInitials(user?.firstName, user?.lastName)}</div>
          <div className="profile-info">
            <span className="profile-name">{user?.firstName} {user?.lastName}</span>
            <span className="profile-role">{user?.roleName}</span>
          </div>
        </div>
      </div>
    </header>
  );
}
