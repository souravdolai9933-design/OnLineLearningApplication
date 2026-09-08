using System;

namespace OnlineLearning.Core.Entities
{
    // ================= Certificates =================
    public class CertificateDto
    {
        public int CertificateId { get; set; }
        public string CertificateCode { get; set; } = string.Empty;
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? CertificateUrl { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsRevoked { get; set; }
        public string? RevokedReason { get; set; }
    }

    public class IssueCertificateRequest
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string? CertificateUrl { get; set; }
    }

    public class RevokeCertificateRequest
    {
        public int CertificateId { get; set; }
        public string RevokedReason { get; set; } = string.Empty;
    }

    // ================= Notifications =================
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string NotificationType { get; set; } = "Info";
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string NotificationType { get; set; } = "Info";
    }

    // ================= Banners =================
    public class BannerDto
    {
        public int BannerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string BannerImage { get; set; } = string.Empty;
        public string? RedirectUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBannerRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string BannerImage { get; set; } = string.Empty;
        public string? RedirectUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateBannerRequest
    {
        public int BannerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string BannerImage { get; set; } = string.Empty;
        public string? RedirectUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    // ================= Support Tickets =================
    public class SupportTicketDto
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateSupportTicketRequest
    {
        public int UserId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
    }

    public class UpdateSupportTicketRequest
    {
        public int TicketId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public class TicketReplyDto
    {
        public int ReplyId { get; set; }
        public int TicketId { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderRole { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTicketReplyRequest
    {
        public int TicketId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ================= Payouts =================
    public class PayoutDto
    {
        public int PayoutId { get; set; }
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string AccountDetails { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int? ProcessedBy { get; set; }
        public string? Remarks { get; set; }
    }

    public class RequestPayoutRequest
    {
        public int InstructorId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string AccountDetails { get; set; } = string.Empty;
    }

    public class PayoutActionRequest
    {
        public int PayoutId { get; set; }
        public int AdminId { get; set; }
        public string? Remarks { get; set; }
    }

    // ================= Site Settings & Logs =================
    public class SiteSettingDto
    {
        public int SettingId { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateSiteSettingRequest
    {
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class AuditLogDto
    {
        public long LogId { get; set; }
        public int? UserId { get; set; }
        public string? Action { get; set; }
        public string? TableName { get; set; }
        public int? RecordId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAuditLogRequest
    {
        public int? UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public int? RecordId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
    }

    // ================= Dashboards =================
    public class AdminDashboardDto
    {
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingApprovals { get; set; }
        public int OpenTickets { get; set; }
    }

    public class InstructorDashboardDto
    {
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int PendingAssignments { get; set; }
    }
}
