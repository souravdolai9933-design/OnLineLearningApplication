import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import AdminLayout from './components/Layout/AdminLayout';
import Login from './pages/auth/Login';
import Dashboard from './pages/dashboard/Dashboard';
import Students from './pages/users/Students';
import Instructors from './pages/users/Instructors';
import Admins from './pages/users/Admins';
import Categories from './pages/courses/Categories';
import Courses from './pages/courses/Courses';
import CourseApproval from './pages/courses/CourseApproval';
import GenericModule from './pages/GenericModule';

function ProtectedRoute({ children }) {
  const { user } = useAuth();
  // If user is not logged in, we can either redirect to /login or let them preview as demo
  // Let's redirect to login if not authenticated, or allow mock session
  if (!user && !localStorage.getItem('token')) {
    return <Navigate to="/login" replace />;
  }
  return children;
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          
          <Route path="/" element={
            <ProtectedRoute>
              <AdminLayout />
            </ProtectedRoute>
          }>
            <Route index element={<Dashboard />} />
            
            {/* User Management */}
            <Route path="users/students" element={<Students />} />
            <Route path="users/instructors" element={<Instructors />} />
            <Route path="users/admins" element={<Admins />} />

            {/* Course Management */}
            <Route path="courses/categories" element={<Categories />} />
            <Route path="courses" element={<Courses />} />
            <Route path="courses/approval" element={<CourseApproval />} />

            {/* Other Modules */}
            <Route path="enrollments" element={<GenericModule title="Enrollments" subtitle="Track student course purchases and active enrollments" actionButtonText="Manual Enrollment" />} />
            <Route path="certificates" element={<GenericModule title="Certificates" subtitle="Issue, track and verify completion certificates" actionButtonText="Generate Certificate" />} />
            <Route path="reviews" element={<GenericModule title="Course Reviews" subtitle="Moderate student feedback and star ratings" />} />
            <Route path="quizzes" element={<GenericModule title="Quizzes" subtitle="Manage quizzes, question banks, and grading criteria" actionButtonText="Create Quiz" />} />
            <Route path="assignments" element={<GenericModule title="Assignments" subtitle="Review assignment submissions and assign marks" actionButtonText="Add Assignment" />} />
            <Route path="notifications" element={<GenericModule title="Notifications" subtitle="Send platform-wide broadcast alerts and student updates" actionButtonText="Compose Alert" />} />
            <Route path="banners" element={<GenericModule title="Promotional Banners" subtitle="Manage marketing banners on the student home page" actionButtonText="Add Banner" />} />
            <Route path="withdrawals" element={<GenericModule title="Withdrawal Requests" subtitle="Process instructor payout requests and payment settlement" />} />
            <Route path="support" element={<GenericModule title="Support Tickets" subtitle="Manage user assistance inquiries and ticket resolution" />} />
            <Route path="audit" element={<GenericModule title="Audit Logs" subtitle="Security logs, admin actions, and system events" />} />
            <Route path="settings" element={<GenericModule title="Site Settings" subtitle="Platform configuration, SMTP, commission rates and payment gateways" />} />
            <Route path="profile" element={<GenericModule title="Admin Profile" subtitle="Account settings, security credentials, and preferences" />} />
          </Route>

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
      <ToastContainer position="top-right" autoClose={3000} theme="dark" />
    </AuthProvider>
  );
}
