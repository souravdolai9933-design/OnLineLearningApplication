using Dapper;
using Microsoft.Extensions.Logging;
using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using OnlineLearning.Data.Interfaces;
using OnlineLearning.Utilites.Connections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Repository
{
    public class CertificateNotificationRepository : ICertificateNotificationRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<CertificateNotificationRepository> _logger;

        public CertificateNotificationRepository(DbConnectionFactory dbConnectionFactory, ILogger<CertificateNotificationRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Certificates =================
        public async Task<DbResult> IssueCertificate(IssueCertificateRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EnrollmentId", request.EnrollmentId);
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@CertificateUrl", request.CertificateUrl);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_IssueCertificate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to issue certificate." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error issuing certificate for EnrollmentId: {EnrollmentId}", request.EnrollmentId);
                throw;
            }
        }

        public async Task<IEnumerable<CertificateDto>> GetCertificates(int studentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@StudentId", studentId);

                return await connection.QueryAsync<CertificateDto>(
                    "sp_GetCertificates",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificates for StudentId: {StudentId}", studentId);
                throw;
            }
        }

        public async Task<CertificateDto?> GetCertificateById(int certificateId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CertificateId", certificateId);

                return await connection.QueryFirstOrDefaultAsync<CertificateDto>(
                    "sp_GetCertificateById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificate by Id: {CertificateId}", certificateId);
                throw;
            }
        }

        public async Task<CertificateDto?> VerifyCertificate(string certificateCode)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CertificateCode", certificateCode);

                return await connection.QueryFirstOrDefaultAsync<CertificateDto>(
                    "sp_VerifyCertificate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying certificate with code: {CertificateCode}", certificateCode);
                throw;
            }
        }

        public async Task<DbResult> RevokeCertificate(RevokeCertificateRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CertificateId", request.CertificateId);
                parameters.Add("@RevokedReason", request.RevokedReason);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RevokeCertificate",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to revoke certificate." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking certificate Id: {CertificateId}", request.CertificateId);
                throw;
            }
        }

        // ================= Notifications =================
        public async Task<DbResult> CreateNotification(CreateNotificationRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Title", request.Title);
                parameters.Add("@Message", request.Message);
                parameters.Add("@NotificationType", request.NotificationType);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateNotification",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create notification." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification for UserId: {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return await connection.QueryAsync<NotificationDto>(
                    "sp_GetNotifications",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> MarkNotificationRead(int notificationId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@NotificationId", notificationId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_MarkNotificationRead",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to mark notification as read." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification Id: {NotificationId} read", notificationId);
                throw;
            }
        }

        public async Task<DbResult> MarkAllNotificationsRead(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_MarkAllNotificationsRead",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to mark all notifications as read." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications read for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> DeleteNotification(int notificationId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@NotificationId", notificationId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteNotification",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete notification." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification Id: {NotificationId}", notificationId);
                throw;
            }
        }
    }
}
