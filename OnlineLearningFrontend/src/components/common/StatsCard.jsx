import './StatsCard.css';

export default function StatsCard({ icon, label, value, trend, trendUp, color = 'primary', delay = 0 }) {
  return (
    <div className={`stats-card stats-${color}`} style={{ animationDelay: `${delay}ms` }}>
      <div className="stats-icon-wrap">
        <span className="stats-icon">{icon}</span>
      </div>
      <div className="stats-info">
        <span className="stats-label">{label}</span>
        <span className="stats-value">{value}</span>
        {trend && (
          <span className={`stats-trend ${trendUp ? 'trend-up' : 'trend-down'}`}>
            {trendUp ? '↑' : '↓'} {trend}
          </span>
        )}
      </div>
    </div>
  );
}
