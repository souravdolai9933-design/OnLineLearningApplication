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
    public class AuthRepository : IAuthRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(DbConnectionFactory dbConnectionFactory, ILogger<AuthRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<DbResult> RegisterUser(RegisterRequest request, string passwordHash)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", request.RoleId);
                parameters.Add("@FirstName", request.FirstName);
                parameters.Add("@LastName", request.LastName);
                parameters.Add("@Email", request.Email);
                parameters.Add("@Phone", request.Phone);
                parameters.Add("@PasswordHash", passwordHash);
                parameters.Add("@ProfileImage", request.ProfileImage);

                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_RegisterUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return new DbResult
                {
                    Result = parameters.Get<int>("@Result"),
                    Message = parameters.Get<string?>("@Message") ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with email: {Email}", request.Email);
                throw;
            }
        }

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Email", request.Email);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection.QueryFirstOrDefaultAsync<LoginResponse>(
                    "sp_LoginUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int spResult = parameters.Get<int>("@Result");
                string spMessage = parameters.Get<string?>("@Message") ?? string.Empty;

                if (result != null)
                {
                    result.Result = spResult;
                    result.Message = spMessage;
                    return result;
                }

                return new LoginResponse { Result = spResult, Message = !string.IsNullOrWhiteSpace(spMessage) ? spMessage : "User not found or inactive." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user with email: {Email}", request.Email);
                throw;
            }
        }

        public async Task<UserProfileDto?> GetUserById(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return await connection.QueryFirstOrDefaultAsync<UserProfileDto>(
                    "sp_GetUserById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user profile for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> UpdateUserProfile(UpdateProfileRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@FirstName", request.FirstName);
                parameters.Add("@LastName", request.LastName);
                parameters.Add("@Phone", request.Phone);
                parameters.Add("@ProfileImage", request.ProfileImage);
                parameters.Add("@Gender", request.Gender);
                parameters.Add("@DateOfBirth", request.DateOfBirth);
                parameters.Add("@Bio", request.Bio);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateUserProfile",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update profile." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for UserId: {UserId}", request.UserId);
                throw;
            }
        }

        public async Task<DbResult> ChangePassword(int userId, string oldPasswordHash, string newPasswordHash)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@OldPasswordHash", oldPasswordHash);
                parameters.Add("@NewPasswordHash", newPasswordHash);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ChangePassword",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to change password." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> ForgotPassword(string email, string resetToken, DateTime expiry)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@ResetToken", resetToken);
                parameters.Add("@TokenExpiry", expiry);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ForgotPassword",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to generate forgot password token." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password for email: {Email}", email);
                throw;
            }
        }

        public async Task<DbResult> ResetPassword(string email, string token, string newPasswordHash)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@Token", token);
                parameters.Add("@NewPasswordHash", newPasswordHash);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ResetPassword",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to reset password." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", email);
                throw;
            }
        }

        public async Task<DbResult> SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@RefreshToken", refreshToken);
                parameters.Add("@ExpiryDate", expiryDate);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                parameters.Add("@RefreshTokenId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_SaveRefreshToken",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return new DbResult
                {
                    Result = parameters.Get<int>("@Result"),
                    Message = parameters.Get<string?>("@Message") ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving refresh token for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<RoleDto>(
                    "sp_GetRoles",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all roles");
                throw;
            }
        }

        public async Task<RoleDto?> GetRoleById(int roleId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId);

                return await connection.QueryFirstOrDefaultAsync<RoleDto>(
                    "sp_GetRoleById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role by Id: {RoleId}", roleId);
                throw;
            }
        }

        public async Task<DbResult> CreateRole(CreateRoleRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@RoleName", request.RoleName);
                parameters.Add("@Description", request.Description);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateRole",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create role." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role: {RoleName}", request.RoleName);
                throw;
            }
        }

        public async Task<DbResult> UpdateRole(UpdateRoleRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", request.RoleId);
                parameters.Add("@RoleName", request.RoleName);
                parameters.Add("@Description", request.Description);
                parameters.Add("@IsActive", request.IsActive);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateRole",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update role." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role Id: {RoleId}", request.RoleId);
                throw;
            }
        }

        public async Task<DbResult> DeleteRole(int roleId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteRole",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete role." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role Id: {RoleId}", roleId);
                throw;
            }
        }
    }
}
