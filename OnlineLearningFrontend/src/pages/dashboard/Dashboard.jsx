import { useState, useEffect } from 'react';
import { MdPeople, MdSchool, MdMenuBook, MdAttachMoney, MdVerified, MdTrendingUp, MdPerson, MdCheck } from 'react-icons/md';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell, LineChart, Line } from 'recharts';
import StatsCard from '../../components/common/StatsCard';
import LoadingSpinner from '../../components/common/LoadingSpinner';
import { adminService } from '../../services/adminService';
import { formatCurrency } from '../../utils/helpers';
import './Dashboard.css';

const CHART_COLORS = ['#6c63ff', '#00d4aa', '#f59e0b', '#ef4444', '#3b82f6', '#a855f7'];

const MOCK_REVENUE = [
  { month: 'Jan', revenue: 45000 }, { month: 'Feb', revenue: 52000 }, { month: 'Mar', revenue: 48000 },
  { month: 'Apr', revenue: 61000 }, { month: 'May', revenue: 55000 }, { month: 'Jun', revenue: 67000 },
  { month: 'Jul', revenue: 72000 }, { month: 'Aug', revenue: 69000 },
];

const MOCK_REGISTRATIONS = [
  { month: 'Jan', students: 120 }, { month: 'Feb', students: 145 }, { month: 'Mar', students: 130 },
  { month: 'Apr', students: 175 }, { month: 'May', students: 160 }, { month: 'Jun', students: 190 },
  { month: 'Jul', students: 210 }, { month: 'Aug', students: 195 },
];

const MOCK_CATEGORIES = [
  { name: 'Programming', value: 35 }, { name: 'AI/ML', value: 25 }, { name: 'Cloud', value: 18 },
  { name: 'Data Science', value: 12 }, { name: 'Business', value: 10 },
];

const RECENT_ACTIVITIES = [
  { icon: <MdPerson />, text: 'New student registered', time: '2 min ago', color: 'info' },
  { icon: <MdMenuBook />, text: 'Course "React Mastery" submitted for review', time: '15 min ago', color: 'warning' },
  { icon: <MdCheck />, text: 'Course "Python Basics" approved', time: '1 hr ago', color: 'success' },
  { icon: <MdAttachMoney />, text: 'Payment of ₹4,999 received', time: '2 hrs ago', color: 'primary' },
  { icon: <MdVerified />, text: 'Certificate issued to Rahul S.', time: '3 hrs ago', color: 'success' },
];

export default function Dashboard() {
  const [dashboard, setDashboard] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const res = await adminService.getAdminDashboard();
      setDashboard(res.data);
    } catch {
      // Use fallback mock data
      setDashboard({ totalStudents: 1250, totalInstructors: 85, totalCourses: 320, publishedCourses: 210, pendingCourses: 24, totalCategories: 12, totalEnrollments: 3400, totalRevenue: 485000, monthlyRevenue: 67000, certificatesIssued: 890 });
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <LoadingSpinner text="Loading dashboard..." />;

  const d = dashboard || {};

  return (
    <div className="dashboard-page animate-fade-in">
      <div className="page-header">
        <div>
          <h1 className="page-title">Dashboard</h1>
          <p className="page-subtitle">Welcome back! Here&apos;s your platform overview.</p>
        </div>
      </div>

      <div className="stats-grid">
        <StatsCard icon={<MdPeople />} label="Total Students" value={d.totalStudents?.toLocaleString() || '0'} color="primary" delay={0} />
        <StatsCard icon={<MdSchool />} label="Total Instructors" value={d.totalInstructors?.toLocaleString() || '0'} color="info" delay={50} />
        <StatsCard icon={<MdMenuBook />} label="Total Courses" value={d.totalCourses?.toLocaleString() || '0'} color="success" delay={100} />
        <StatsCard icon={<MdMenuBook />} label="Published Courses" value={d.publishedCourses?.toLocaleString() || '0'} color="success" delay={150} />
        <StatsCard icon={<MdMenuBook />} label="Pending Courses" value={d.pendingCourses?.toLocaleString() || '0'} color="warning" delay={200} />
        <StatsCard icon={<MdAttachMoney />} label="Total Revenue" value={formatCurrency(d.totalRevenue)} color="primary" delay={250} />
        <StatsCard icon={<MdTrendingUp />} label="Monthly Revenue" value={formatCurrency(d.monthlyRevenue)} color="success" delay={300} trend="+12%" trendUp />
        <StatsCard icon={<MdVerified />} label="Certificates Issued" value={d.certificatesIssued?.toLocaleString() || '0'} color="info" delay={350} />
      </div>

      <div className="charts-grid">
        <div className="chart-card glass-card">
          <h3 className="chart-title">Revenue by Month</h3>
          <ResponsiveContainer width="100%" height={280}>
            <BarChart data={MOCK_REVENUE}>
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.05)" />
              <XAxis dataKey="month" stroke="#5a5e72" fontSize={12} />
              <YAxis stroke="#5a5e72" fontSize={12} tickFormatter={v => `₹${(v/1000).toFixed(0)}k`} />
              <Tooltip contentStyle={{ background: '#1a1d27', border: '1px solid rgba(255,255,255,0.1)', borderRadius: 8, color: '#f0f0f5' }} formatter={v => [formatCurrency(v), 'Revenue']} />
              <Bar dataKey="revenue" fill="url(#barGradient)" radius={[4,4,0,0]} />
              <defs>
                <linearGradient id="barGradient" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#6c63ff" />
                  <stop offset="100%" stopColor="#a855f7" />
                </linearGradient>
              </defs>
            </BarChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card glass-card">
          <h3 className="chart-title">Student Registrations</h3>
          <ResponsiveContainer width="100%" height={280}>
            <LineChart data={MOCK_REGISTRATIONS}>
              <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.05)" />
              <XAxis dataKey="month" stroke="#5a5e72" fontSize={12} />
              <YAxis stroke="#5a5e72" fontSize={12} />
              <Tooltip contentStyle={{ background: '#1a1d27', border: '1px solid rgba(255,255,255,0.1)', borderRadius: 8, color: '#f0f0f5' }} />
              <Line type="monotone" dataKey="students" stroke="#00d4aa" strokeWidth={2.5} dot={{ fill: '#00d4aa', r: 4 }} activeDot={{ r: 6 }} />
            </LineChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card glass-card">
          <h3 className="chart-title">Course Category Distribution</h3>
          <ResponsiveContainer width="100%" height={280}>
            <PieChart>
              <Pie data={MOCK_CATEGORIES} cx="50%" cy="50%" innerRadius={60} outerRadius={100} paddingAngle={4} dataKey="value" label={({ name, percent }) => `${name} ${(percent*100).toFixed(0)}%`}>
                {MOCK_CATEGORIES.map((_, i) => <Cell key={i} fill={CHART_COLORS[i % CHART_COLORS.length]} />)}
              </Pie>
              <Tooltip contentStyle={{ background: '#1a1d27', border: '1px solid rgba(255,255,255,0.1)', borderRadius: 8, color: '#f0f0f5' }} />
            </PieChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card glass-card">
          <h3 className="chart-title">Recent Activities</h3>
          <div className="activity-list">
            {RECENT_ACTIVITIES.map((a, i) => (
              <div key={i} className="activity-item">
                <div className={`activity-icon activity-${a.color}`}>{a.icon}</div>
                <div className="activity-info">
                  <p className="activity-text">{a.text}</p>
                  <span className="activity-time">{a.time}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
