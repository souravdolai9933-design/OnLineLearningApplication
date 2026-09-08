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
    public class CourseRepository : ICourseRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(DbConnectionFactory dbConnectionFactory, ILogger<CourseRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Categories =================
        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<CategoryDto>(
                    "sp_GetCategories",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                throw;
            }
        }

        public async Task<CategoryDto?> GetCategoryById(int categoryId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);

                return await connection.QueryFirstOrDefaultAsync<CategoryDto>(
                    "sp_GetCategoryById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category by Id: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<DbResult> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryName", request.CategoryName);
                parameters.Add("@Description", request.Description);
                parameters.Add("@CategoryImage", request.CategoryImage);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateCategory",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create category." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryName}", request.CategoryName);
                throw;
            }
        }

        public async Task<DbResult> UpdateCategory(UpdateCategoryRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", request.CategoryId);
                parameters.Add("@CategoryName", request.CategoryName);
                parameters.Add("@Description", request.Description);
                parameters.Add("@CategoryImage", request.CategoryImage);
                parameters.Add("@Status", request.Status);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateCategory",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update category." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category Id: {CategoryId}", request.CategoryId);
                throw;
            }
        }

        public async Task<DbResult> DeleteCategory(int categoryId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteCategory",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete category." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category Id: {CategoryId}", categoryId);
                throw;
            }
        }

        // ================= Courses =================
        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<CourseDto>(
                    "sp_GetCourses",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses");
                throw;
            }
        }

        public async Task<CourseDto?> GetCourseById(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryFirstOrDefaultAsync<CourseDto>(
                    "sp_GetCourseById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course by Id: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<DbResult> CreateCourse(CreateCourseRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@InstructorId", request.InstructorId);
                parameters.Add("@CategoryId", request.CategoryId);
                parameters.Add("@Title", request.Title);
                parameters.Add("@ShortDescription", request.ShortDescription);
                parameters.Add("@Description", request.Description);
                parameters.Add("@Price", request.Price);
                parameters.Add("@DiscountPrice", request.DiscountPrice);
                parameters.Add("@CourseLevel", request.CourseLevel);
                parameters.Add("@Language", request.Language);
                parameters.Add("@Thumbnail", request.Thumbnail);
                parameters.Add("@PromoVideoUrl", request.PromoVideoUrl);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course: {Title}", request.Title);
                throw;
            }
        }

        public async Task<DbResult> UpdateCourse(UpdateCourseRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@InstructorId", request.InstructorId);
                parameters.Add("@CategoryId", request.CategoryId);
                parameters.Add("@Title", request.Title);
                parameters.Add("@ShortDescription", request.ShortDescription);
                parameters.Add("@Description", request.Description);
                parameters.Add("@Price", request.Price);
                parameters.Add("@DiscountPrice", request.DiscountPrice);
                parameters.Add("@CourseLevel", request.CourseLevel);
                parameters.Add("@Language", request.Language);
                parameters.Add("@Thumbnail", request.Thumbnail);
                parameters.Add("@PromoVideoUrl", request.PromoVideoUrl);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course Id: {CourseId}", request.CourseId);
                throw;
            }
        }

        public async Task<DbResult> DeleteCourse(int courseId, int instructorId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);
                parameters.Add("@InstructorId", instructorId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course Id: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<IEnumerable<CourseDto>> SearchCourses(string? searchTerm, int? categoryId, string? courseLevel, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SearchTerm", searchTerm);
                parameters.Add("@CategoryId", categoryId);
                parameters.Add("@CourseLevel", courseLevel);
                parameters.Add("@MinPrice", minPrice);
                parameters.Add("@MaxPrice", maxPrice);

                return await connection.QueryAsync<CourseDto>(
                    "sp_SearchCourses",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching courses with keyword: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByCategory(int categoryId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);

                return await connection.QueryAsync<CourseDto>(
                    "sp_GetCoursesByCategory",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses by CategoryId: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByInstructor(int instructorId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@InstructorId", instructorId);

                return await connection.QueryAsync<CourseDto>(
                    "sp_GetCoursesByInstructor",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses by InstructorId: {InstructorId}", instructorId);
                throw;
            }
        }

        public async Task<DbResult> PublishCourse(int courseId, int instructorId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);
                parameters.Add("@InstructorId", instructorId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_PublishCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to publish course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing course Id: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<DbResult> UnpublishCourse(int courseId, int instructorId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);
                parameters.Add("@InstructorId", instructorId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UnpublishCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to unpublish course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unpublishing course Id: {CourseId}", courseId);
                throw;
            }
        }

        // ================= Approvals =================
        public async Task<IEnumerable<CourseDto>> GetPendingCourses()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<CourseDto>(
                    "sp_GetPendingCourses",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending courses");
                throw;
            }
        }

        public async Task<DbResult> ApproveCourse(CourseApprovalRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@AdminId", request.AdminId);
                parameters.Add("@Remarks", request.Remarks);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ApproveCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to approve course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving course Id: {CourseId}", request.CourseId);
                throw;
            }
        }

        public async Task<DbResult> RejectCourse(CourseApprovalRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@AdminId", request.AdminId);
                parameters.Add("@Remarks", request.Remarks);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RejectCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to reject course." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting course Id: {CourseId}", request.CourseId);
                throw;
            }
        }

        public async Task<IEnumerable<CourseApprovalHistoryDto>> GetCourseApprovalHistory(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryAsync<CourseApprovalHistoryDto>(
                    "sp_GetCourseApprovalHistory",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course approval history for CourseId: {CourseId}", courseId);
                throw;
            }
        }
    }
}
