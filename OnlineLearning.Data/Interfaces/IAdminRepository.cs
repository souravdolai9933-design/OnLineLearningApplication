using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IAdminRepository
    {
        // Payouts
        Task<DbResult> RequestPayout(RequestPayoutRequest request);
        Task<IEnumerable<PayoutDto>> GetPayouts(int? instructorId, string? status);
        Task<PayoutDto?> GetPayoutById(int payoutId);
        Task<DbResult> ApprovePayout(PayoutActionRequest request);
        Task<DbResult> RejectPayout(PayoutActionRequest request);

        // Settings & Audit Logs
        Task<IEnumerable<SiteSettingDto>> GetSiteSettings();
        Task<SiteSettingDto?> GetSiteSettingByKey(string settingKey);
        Task<DbResult> UpdateSiteSetting(UpdateSiteSettingRequest request);
        Task<DbResult> CreateAuditLog(CreateAuditLogRequest request);
        Task<IEnumerable<AuditLogDto>> GetAuditLogs(int? userId, string? tableName, DateTime? fromDate, DateTime? toDate);

        // Dashboards
        Task<AdminDashboardDto?> GetAdminDashboard();
        Task<InstructorDashboardDto?> GetInstructorDashboard(int instructorId);
    }
}
