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
    public class BannerSupportRepository : IBannerSupportRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<BannerSupportRepository> _logger;

        public BannerSupportRepository(DbConnectionFactory dbConnectionFactory, ILogger<BannerSupportRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Banners =================
        public async Task<DbResult> CreateBanner(CreateBannerRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Title", request.Title);
                parameters.Add("@Description", request.Description);
                parameters.Add("@BannerImage", request.BannerImage);
                parameters.Add("@RedirectUrl", request.RedirectUrl);
                parameters.Add("@DisplayOrder", request.DisplayOrder);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@StartDate", request.StartDate);
                parameters.Add("@EndDate", request.EndDate);
                parameters.Add("@CreatedBy", request.CreatedBy);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateBanner",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create banner." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating banner: {Title}", request.Title);
                throw;
            }
        }

        public async Task<IEnumerable<BannerDto>> GetBanners()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<BannerDto>(
                    "sp_GetBanners",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching banners");
                throw;
            }
        }

        public async Task<BannerDto?> GetBannerById(int bannerId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@BannerId", bannerId);

                return await connection.QueryFirstOrDefaultAsync<BannerDto>(
                    "sp_GetBannerById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching banner by Id: {BannerId}", bannerId);
                throw;
            }
        }

        public async Task<DbResult> UpdateBanner(UpdateBannerRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@BannerId", request.BannerId);
                parameters.Add("@Title", request.Title);
                parameters.Add("@Description", request.Description);
                parameters.Add("@BannerImage", request.BannerImage);
                parameters.Add("@RedirectUrl", request.RedirectUrl);
                parameters.Add("@DisplayOrder", request.DisplayOrder);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@StartDate", request.StartDate);
                parameters.Add("@EndDate", request.EndDate);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateBanner",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update banner." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating banner Id: {BannerId}", request.BannerId);
                throw;
            }
        }

        public async Task<DbResult> DeleteBanner(int bannerId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@BannerId", bannerId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteBanner",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete banner." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting banner Id: {BannerId}", bannerId);
                throw;
            }
        }

        // ================= Support Tickets =================
        public async Task<DbResult> CreateSupportTicket(CreateSupportTicketRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Subject", request.Subject);
                parameters.Add("@Message", request.Message);
                parameters.Add("@Priority", request.Priority);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateSupportTicket",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create support ticket." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating support ticket for UserId: {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<IEnumerable<SupportTicketDto>> GetSupportTickets(int? userId, string? status)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@Status", status);

                return await connection.QueryAsync<SupportTicketDto>(
                    "sp_GetSupportTickets",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching support tickets");
                throw;
            }
        }

        public async Task<SupportTicketDto?> GetSupportTicketById(int ticketId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@TicketId", ticketId);

                return await connection.QueryFirstOrDefaultAsync<SupportTicketDto>(
                    "sp_GetSupportTicketById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching support ticket by Id: {TicketId}", ticketId);
                throw;
            }
        }

        public async Task<DbResult> UpdateSupportTicket(UpdateSupportTicketRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@TicketId", request.TicketId);
                parameters.Add("@Status", request.Status);
                parameters.Add("@Priority", request.Priority);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateSupportTicket",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update support ticket." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating support ticket Id: {TicketId}", request.TicketId);
                throw;
            }
        }

        public async Task<DbResult> CloseSupportTicket(int ticketId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@TicketId", ticketId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CloseSupportTicket",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to close support ticket." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing support ticket Id: {TicketId}", ticketId);
                throw;
            }
        }

        public async Task<DbResult> CreateTicketReply(CreateTicketReplyRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@TicketId", request.TicketId);
                parameters.Add("@SenderId", request.SenderId);
                parameters.Add("@Message", request.Message);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateTicketReply",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to send ticket reply." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reply for TicketId: {TicketId}", request.TicketId);
                throw;
            }
        }

        public async Task<IEnumerable<TicketReplyDto>> GetTicketReplies(int ticketId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@TicketId", ticketId);

                return await connection.QueryAsync<TicketReplyDto>(
                    "sp_GetTicketReplies",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching replies for TicketId: {TicketId}", ticketId);
                throw;
            }
        }
    }
}
