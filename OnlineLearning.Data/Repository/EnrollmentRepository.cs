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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<EnrollmentRepository> _logger;

        public EnrollmentRepository(DbConnectionFactory dbConnectionFactory, ILogger<EnrollmentRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<DbResult> EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@AmountPaid", request.AmountPaid);
                parameters.Add("@PaymentMethod", request.PaymentMethod);
                parameters.Add("@TransactionId", request.TransactionId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_EnrollStudent",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to enroll student." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling student UserId: {UserId} in CourseId: {CourseId}", request.UserId, request.CourseId);
                throw;
            }
        }

        public async Task<IEnumerable<EnrollmentDto>> GetStudentCourses(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return await connection.QueryAsync<EnrollmentDto>(
                    "sp_GetStudentCourses",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student courses for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<EnrollmentDto?> GetEnrollmentById(int enrollmentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EnrollmentId", enrollmentId);

                return await connection.QueryFirstOrDefaultAsync<EnrollmentDto>(
                    "sp_GetEnrollmentById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching enrollment Id: {EnrollmentId}", enrollmentId);
                throw;
            }
        }

        public async Task<DbResult> CancelEnrollment(int enrollmentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@EnrollmentId", enrollmentId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CancelEnrollment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to cancel enrollment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling enrollment Id: {EnrollmentId}", enrollmentId);
                throw;
            }
        }
    }
}
