import { useState } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { MdDashboard, MdPeople, MdMenuBook, MdSchool, MdVerified, MdStar, MdQuiz, MdAssignment, MdNotifications, MdImage, MdAccountBalanceWallet, MdSupport, MdHistory, MdSettings, MdPerson, MdLogout, MdExpandMore, MdChevronLeft, MdChevronRight } from 'react-icons/md';
import { useAuth } from '../../context/AuthContext';
import './Sidebar.css';

const ICON_MAP = { MdDashboard, MdPeople, MdMenuBook, MdSchool, MdVerified, MdStar, MdQuiz, MdAssignment, MdNotifications, MdImage, MdAccountBalanceWallet, MdSupport, MdHistory, MdSettings, MdPerson };

const MENU = [
  { key: 'dashboard', label: 'Dashboard', icon: 'MdDashboard', path: '/' },
  { key: 'users', label: 'User Management', icon: 'MdPeople', children: [
    { key: 'students', label: 'Students', path: '/users/students' },
    { key: 'instructors', label: 'Instructors', path: '/users/instructors' },
    { key: 'admins', label: 'Admins', path: '/users/admins' },
  ]},
  { key: 'courses', label: 'Course Management', icon: 'MdMenuBook', children: [
    { key: 'categories', label: 'Categories', path: '/courses/categories' },
    { key: 'all-courses', label: 'Courses', path: '/courses' },
    { key: 'approval', label: 'Course Approval', path: '/courses/approval' },
  ]},
  { key: 'enrollments', label: 'Enrollments', icon: 'MdSchool', path: '/enrollments' },
  { key: 'certificates', label: 'Certificates', icon: 'MdVerified', path: '/certificates' },
  { key: 'reviews', label: 'Reviews', icon: 'MdStar', path: '/reviews' },
  { key: 'quizzes', label: 'Quizzes', icon: 'MdQuiz', path: '/quizzes' },
  { key: 'assignments', label: 'Assignments', icon: 'MdAssignment', path: '/assignments' },
  { key: 'notifications', label: 'Notifications', icon: 'MdNotifications', path: '/notifications' },
  { key: 'banners', label: 'Banners', icon: 'MdImage', path: '/banners' },
  { key: 'withdrawals', label: 'Withdrawals', icon: 'MdAccountBalanceWallet', path: '/withdrawals' },
  { key: 'support', label: 'Support Tickets', icon: 'MdSupport', path: '/support' },
  { key: 'audit', label: 'Audit Logs', icon: 'MdHistory', path: '/audit' },
  { key: 'settings', label: 'Settings', icon: 'MdSettings', path: '/settings' },
  { key: 'profile', label: 'Profile', icon: 'MdPerson', path: '/profile' },
];

export default function Sidebar({ collapsed, onToggle }) {
  const [openMenus, setOpenMenus] = useState({});
  const location = useLocation();
  const { logout } = useAuth();

  const toggleMenu = (key) => setOpenMenus(prev => ({ ...prev, [key]: !prev[key] }));

  const isChildActive = (item) => item.children?.some(c => location.pathname === c.path);

  return (
    <aside className={`sidebar ${collapsed ? 'collapsed' : ''}`}>
      <div className="sidebar-header">
        <div className="sidebar-logo">
          <span className="logo-icon">📚</span>
          {!collapsed && <span className="logo-text">EduAdmin</span>}
        </div>
        <button className="btn-ghost sidebar-toggle" onClick={onToggle}>
          {collapsed ? <MdChevronRight size={18} /> : <MdChevronLeft size={18} />}
        </button>
      </div>

      <nav className="sidebar-nav">
        {MENU.map(item => {
          const Icon = ICON_MAP[item.icon];
          const hasChildren = item.children?.length > 0;
          const isOpen = openMenus[item.key] || isChildActive(item);

          if (hasChildren) {
            return (
              <div key={item.key} className={`nav-group ${isOpen ? 'open' : ''}`}>
                <button className={`nav-item ${isChildActive(item) ? 'active' : ''}`} onClick={() => toggleMenu(item.key)} title={collapsed ? item.label : ''}>
                  {Icon && <Icon size={20} className="nav-icon" />}
                  {!collapsed && <span className="nav-label">{item.label}</span>}
                  {!collapsed && <MdExpandMore size={18} className={`nav-arrow ${isOpen ? 'rotated' : ''}`} />}
                </button>
                {!collapsed && isOpen && (
                  <div className="nav-children">
                    {item.children.map(child => (
                      <NavLink key={child.key} to={child.path} className={({ isActive }) => `nav-child ${isActive ? 'active' : ''}`}>
                        <span className="nav-dot" />
                        <span>{child.label}</span>
                      </NavLink>
                    ))}
                  </div>
                )}
              </div>
            );
          }

          return (
            <NavLink key={item.key} to={item.path} end={item.path === '/'} className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`} title={collapsed ? item.label : ''}>
              {Icon && <Icon size={20} className="nav-icon" />}
              {!collapsed && <span className="nav-label">{item.label}</span>}
            </NavLink>
          );
        })}
      </nav>

      <div className="sidebar-footer">
        <button className="nav-item logout-btn" onClick={logout} title="Logout">
          <MdLogout size={20} className="nav-icon" />
          {!collapsed && <span className="nav-label">Logout</span>}
        </button>
      </div>
    </aside>
  );
}
