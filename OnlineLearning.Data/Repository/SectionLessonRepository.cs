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
    public class SectionLessonRepository : ISectionLessonRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<SectionLessonRepository> _logger;

        public SectionLessonRepository(DbConnectionFactory dbConnectionFactory, ILogger<SectionLessonRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Sections =================
        public async Task<IEnumerable<SectionDto>> GetSections(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryAsync<SectionDto>(
                    "sp_GetSections",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sections for CourseId: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<SectionDto?> GetSectionById(int sectionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SectionId", sectionId);

                return await connection.QueryFirstOrDefaultAsync<SectionDto>(
                    "sp_GetSectionById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching section by Id: {SectionId}", sectionId);
                throw;
            }
        }

        public async Task<DbResult> CreateSection(CreateSectionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@SectionTitle", request.SectionTitle);
                parameters.Add("@SectionOrder", request.SectionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateSection",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create section." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating section: {SectionTitle}", request.SectionTitle);
                throw;
            }
        }

        public async Task<DbResult> UpdateSection(UpdateSectionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SectionId", request.SectionId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@SectionTitle", request.SectionTitle);
                parameters.Add("@SectionOrder", request.SectionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateSection",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update section." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating section Id: {SectionId}", request.SectionId);
                throw;
            }
        }

        public async Task<DbResult> DeleteSection(int sectionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SectionId", sectionId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteSection",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete section." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting section Id: {SectionId}", sectionId);
                throw;
            }
        }

        // ================= Lessons =================
        public async Task<IEnumerable<LessonDto>> GetLessons(int sectionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SectionId", sectionId);

                return await connection.QueryAsync<LessonDto>(
                    "sp_GetLessons",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lessons for SectionId: {SectionId}", sectionId);
                throw;
            }
        }

        public async Task<LessonDto?> GetLessonById(int lessonId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@LessonId", lessonId);

                return await connection.QueryFirstOrDefaultAsync<LessonDto>(
                    "sp_GetLessonById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lesson by Id: {LessonId}", lessonId);
                throw;
            }
        }

        public async Task<DbResult> CreateLesson(CreateLessonRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SectionId", request.SectionId);
                parameters.Add("@LessonTitle", request.LessonTitle);
                parameters.Add("@LessonType", request.LessonType);
                parameters.Add("@VideoUrl", request.VideoUrl);
                parameters.Add("@Duration", request.Duration);
                parameters.Add("@Content", request.Content);
                parameters.Add("@IsPreview", request.IsPreview);
                parameters.Add("@LessonOrder", request.LessonOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateLesson",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create lesson." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lesson: {LessonTitle}", request.LessonTitle);
                throw;
            }
        }

        public async Task<DbResult> UpdateLesson(UpdateLessonRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@LessonId", request.LessonId);
                parameters.Add("@SectionId", request.SectionId);
                parameters.Add("@LessonTitle", request.LessonTitle);
                parameters.Add("@LessonType", request.LessonType);
                parameters.Add("@VideoUrl", request.VideoUrl);
                parameters.Add("@Duration", request.Duration);
                parameters.Add("@Content", request.Content);
                parameters.Add("@IsPreview", request.IsPreview);
                parameters.Add("@LessonOrder", request.LessonOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateLesson",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update lesson." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson Id: {LessonId}", request.LessonId);
                throw;
            }
        }

        public async Task<DbResult> DeleteLesson(int lessonId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@LessonId", lessonId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteLesson",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete lesson." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson Id: {LessonId}", lessonId);
                throw;
            }
        }

        // ================= Resources =================
        public async Task<IEnumerable<LessonResourceDto>> GetLessonResources(int lessonId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@LessonId", lessonId);

                return await connection.QueryAsync<LessonResourceDto>(
                    "sp_GetLessonResources",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching resources for LessonId: {LessonId}", lessonId);
                throw;
            }
        }

        public async Task<LessonResourceDto?> GetLessonResourceById(int resourceId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ResourceId", resourceId);

                return await connection.QueryFirstOrDefaultAsync<LessonResourceDto>(
                    "sp_GetLessonResourceById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching resource by Id: {ResourceId}", resourceId);
                throw;
            }
        }

        public async Task<DbResult> AddLessonResource(AddLessonResourceRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@LessonId", request.LessonId);
                parameters.Add("@ResourceTitle", request.ResourceTitle);
                parameters.Add("@ResourceType", request.ResourceType);
                parameters.Add("@FileUrl", request.FileUrl);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_AddLessonResource",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to add lesson resource." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding resource for LessonId: {LessonId}", request.LessonId);
                throw;
            }
        }

        public async Task<DbResult> UpdateLessonResource(UpdateLessonResourceRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ResourceId", request.ResourceId);
                parameters.Add("@LessonId", request.LessonId);
                parameters.Add("@ResourceTitle", request.ResourceTitle);
                parameters.Add("@ResourceType", request.ResourceType);
                parameters.Add("@FileUrl", request.FileUrl);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateLessonResource",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update lesson resource." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating resource Id: {ResourceId}", request.ResourceId);
                throw;
            }
        }

        public async Task<DbResult> DeleteLessonResource(int resourceId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@ResourceId", resourceId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteLessonResource",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete lesson resource." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting resource Id: {ResourceId}", resourceId);
                throw;
            }
        }

        // ================= Progress =================
        public async Task<DbResult> UpdateLessonProgress(LessonProgressRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@LessonId", request.LessonId);
                parameters.Add("@IsCompleted", request.IsCompleted);
                parameters.Add("@WatchedDuration", request.WatchedDuration);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateLessonProgress",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update lesson progress." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson progress for UserId: {UserId}, LessonId: {LessonId}", request.UserId, request.LessonId);
                throw;
            }
        }

        public async Task<LessonProgressDto?> GetLessonProgress(int userId, int lessonId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@LessonId", lessonId);

                return await connection.QueryFirstOrDefaultAsync<LessonProgressDto>(
                    "sp_GetLessonProgress",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching progress for UserId: {UserId}, LessonId: {LessonId}", userId, lessonId);
                throw;
            }
        }

        public async Task<CourseProgressDto?> GetCourseProgress(int userId, int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@CourseId", courseId);

                return await connection.QueryFirstOrDefaultAsync<CourseProgressDto>(
                    "sp_GetCourseProgress",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course progress for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                throw;
            }
        }

        public async Task<DbResult> CompleteCourse(int userId, int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@CourseId", courseId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CompleteCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to mark course complete." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing course for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                throw;
            }
        }
    }
}
