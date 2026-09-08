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
    public class AdminRepository : IAdminRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(DbConnectionFactory dbConnectionFactory, ILogger<AdminRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Payouts =================
        public async Task<DbResult> RequestPayout(RequestPayoutRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@InstructorId", request.InstructorId);
                parameters.Add("@Amount", request.Amount);
                parameters.Add("@PaymentMethod", request.PaymentMethod);
                parameters.Add("@AccountDetails", request.AccountDetails);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RequestPayout",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to request payout." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting payout for InstructorId: {InstructorId}", request.InstructorId);
                throw;
            }
        }

        public async Task<IEnumerable<PayoutDto>> GetPayouts(int? instructorId, string? status)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@InstructorId", instructorId);
                parameters.Add("@Status", status);

                return await connection.QueryAsync<PayoutDto>(
                    "sp_GetPayouts",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payouts");
                throw;
            }
        }

        public async Task<PayoutDto?> GetPayoutById(int payoutId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@PayoutId", payoutId);

                return await connection.QueryFirstOrDefaultAsync<PayoutDto>(
                    "sp_GetPayoutById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payout by Id: {PayoutId}", payoutId);
                throw;
            }
        }

        public async Task<DbResult> ApprovePayout(PayoutActionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@PayoutId", request.PayoutId);
                parameters.Add("@AdminId", request.AdminId);
                parameters.Add("@Remarks", request.Remarks);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ApprovePayout",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to approve payout." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving payout Id: {PayoutId}", request.PayoutId);
                throw;
            }
        }

        public async Task<DbResult> RejectPayout(PayoutActionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@PayoutId", request.PayoutId);
                parameters.Add("@AdminId", request.AdminId);
                parameters.Add("@Remarks", request.Remarks);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RejectPayout",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to reject payout." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting payout Id: {PayoutId}", request.PayoutId);
                throw;
            }
        }

        // ================= Settings & Audit Logs =================
        public async Task<IEnumerable<SiteSettingDto>> GetSiteSettings()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<SiteSettingDto>(
                    "sp_GetSiteSettings",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching site settings");
                throw;
            }
        }

        public async Task<SiteSettingDto?> GetSiteSettingByKey(string settingKey)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SettingKey", settingKey);

                return await connection.QueryFirstOrDefaultAsync<SiteSettingDto>(
                    "sp_GetSiteSettingByKey",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching setting by key: {SettingKey}", settingKey);
                throw;
            }
        }

        public async Task<DbResult> UpdateSiteSetting(UpdateSiteSettingRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SettingKey", request.SettingKey);
                parameters.Add("@SettingValue", request.SettingValue);
                parameters.Add("@Description", request.Description);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateSiteSetting",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update site setting." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating setting key: {SettingKey}", request.SettingKey);
                throw;
            }
        }

        public async Task<DbResult> CreateAuditLog(CreateAuditLogRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Action", request.Action);
                parameters.Add("@TableName", request.TableName);
                parameters.Add("@RecordId", request.RecordId);
                parameters.Add("@OldValues", request.OldValues);
                parameters.Add("@NewValues", request.NewValues);
                parameters.Add("@IpAddress", request.IpAddress);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateAuditLog",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create audit log." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating audit log for Table: {TableName}", request.TableName);
                throw;
            }
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogs(int? userId, string? tableName, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@TableName", tableName);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);

                return await connection.QueryAsync<AuditLogDto>(
                    "sp_GetAuditLogs",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching audit logs");
                throw;
            }
        }

        // ================= Dashboards =================
        public async Task<AdminDashboardDto?> GetAdminDashboard()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryFirstOrDefaultAsync<AdminDashboardDto>(
                    "sp_GetAdminDashboard",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching admin dashboard stats");
                throw;
            }
        }

        public async Task<InstructorDashboardDto?> GetInstructorDashboard(int instructorId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@InstructorId", instructorId);

                return await connection.QueryFirstOrDefaultAsync<InstructorDashboardDto>(
                    "sp_GetInstructorDashboard",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching instructor dashboard for InstructorId: {InstructorId}", instructorId);
                throw;
            }
        }
    }
}
