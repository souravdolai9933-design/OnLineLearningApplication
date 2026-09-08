using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface ICertificateNotificationRepository
    {
        // Certificates
        Task<DbResult> IssueCertificate(IssueCertificateRequest request);
        Task<IEnumerable<CertificateDto>> GetCertificates(int studentId);
        Task<CertificateDto?> GetCertificateById(int certificateId);
        Task<CertificateDto?> VerifyCertificate(string certificateCode);
        Task<DbResult> RevokeCertificate(RevokeCertificateRequest request);

        // Notifications
        Task<DbResult> CreateNotification(CreateNotificationRequest request);
        Task<IEnumerable<NotificationDto>> GetNotifications(int userId);
        Task<DbResult> MarkNotificationRead(int notificationId);
        Task<DbResult> MarkAllNotificationsRead(int userId);
        Task<DbResult> DeleteNotification(int notificationId);
    }
}
