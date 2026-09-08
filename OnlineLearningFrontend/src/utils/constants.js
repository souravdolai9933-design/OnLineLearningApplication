export const API_BASE_URL = '/api';

export const MENU_ITEMS = [
  { key: 'dashboard', label: 'Dashboard', icon: 'MdDashboard', path: '/' },
  {
    key: 'users', label: 'User Management', icon: 'MdPeople',
    children: [
      { key: 'students', label: 'Students', path: '/users/students' },
      { key: 'instructors', label: 'Instructors', path: '/users/instructors' },
      { key: 'admins', label: 'Admins', path: '/users/admins' },
    ]
  },
  {
    key: 'courses', label: 'Course Management', icon: 'MdMenuBook',
    children: [
      { key: 'categories', label: 'Categories', path: '/courses/categories' },
      { key: 'all-courses', label: 'Courses', path: '/courses' },
      { key: 'approval', label: 'Course Approval', path: '/courses/approval' },
    ]
  },
  { key: 'enrollments', label: 'Enrollments', icon: 'MdSchool', path: '/enrollments' },
  { key: 'certificates', label: 'Certificates', icon: 'MdVerified', path: '/certificates' },
  { key: 'reviews', label: 'Reviews', icon: 'MdStar', path: '/reviews' },
  { key: 'quizzes', label: 'Quizzes', icon: 'MdQuiz', path: '/quizzes' },
  { key: 'assignments', label: 'Assignments', icon: 'MdAssignment', path: '/assignments' },
  { key: 'notifications', label: 'Notifications', icon: 'MdNotifications', path: '/notifications' },
  { key: 'banners', label: 'Banners', icon: 'MdImage', path: '/banners' },
  { key: 'withdrawals', label: 'Withdrawal Requests', icon: 'MdAccountBalanceWallet', path: '/withdrawals' },
  { key: 'support', label: 'Support Tickets', icon: 'MdSupport', path: '/support' },
  { key: 'audit', label: 'Audit Logs', icon: 'MdHistory', path: '/audit' },
  { key: 'settings', label: 'Settings', icon: 'MdSettings', path: '/settings' },
  { key: 'profile', label: 'Profile', icon: 'MdPerson', path: '/profile' },
];
