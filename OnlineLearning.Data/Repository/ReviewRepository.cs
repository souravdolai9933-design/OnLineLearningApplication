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
    public class ReviewRepository : IReviewRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(DbConnectionFactory dbConnectionFactory, ILogger<ReviewRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewDto>> GetCourseReviews(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryAsync<ReviewDto>(
                    "sp_GetCourseReviews",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reviews for CourseId: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<ReviewDto?> GetReviewById(int reviewId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", reviewId);

                return await connection.QueryFirstOrDefaultAsync<ReviewDto>(
                    "sp_GetReviewById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching review by Id: {ReviewId}", reviewId);
                throw;
            }
        }

        public async Task<DbResult> CreateReview(CreateReviewRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Rating", request.Rating);
                parameters.Add("@ReviewText", request.ReviewText);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateReview",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create review." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for CourseId: {CourseId}, UserId: {UserId}", request.CourseId, request.UserId);
                throw;
            }
        }

        public async Task<DbResult> UpdateReview(UpdateReviewRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", request.ReviewId);
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@Rating", request.Rating);
                parameters.Add("@ReviewText", request.ReviewText);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateReview",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update review." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review Id: {ReviewId}", request.ReviewId);
                throw;
            }
        }

        public async Task<DbResult> DeleteReview(int reviewId, int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", reviewId);
                parameters.Add("@UserId", userId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteReview",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete review." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review Id: {ReviewId}", reviewId);
                throw;
            }
        }
    }
}
