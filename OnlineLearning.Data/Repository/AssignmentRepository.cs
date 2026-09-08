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
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<AssignmentRepository> _logger;

        public AssignmentRepository(DbConnectionFactory dbConnectionFactory, ILogger<AssignmentRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignments()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<AssignmentDto>(
                    "sp_GetAssignments",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all assignments");
                throw;
            }
        }

        public async Task<AssignmentDto?> GetAssignmentById(int assignmentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", assignmentId);

                return await connection.QueryFirstOrDefaultAsync<AssignmentDto>(
                    "sp_GetAssignmentById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignment by Id: {AssignmentId}", assignmentId);
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByCourse(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryAsync<AssignmentDto>(
                    "sp_GetAssignmentsByCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignments for CourseId: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<DbResult> CreateAssignment(CreateAssignmentRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@AssignmentTitle", request.AssignmentTitle);
                parameters.Add("@AssignmentDescription", request.AssignmentDescription);
                parameters.Add("@TotalMarks", request.TotalMarks);
                parameters.Add("@DueDate", request.DueDate);
                parameters.Add("@AttachmentPath", request.AttachmentPath);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateAssignment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create assignment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assignment: {AssignmentTitle}", request.AssignmentTitle);
                throw;
            }
        }

        public async Task<DbResult> UpdateAssignment(UpdateAssignmentRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", request.AssignmentId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@AssignmentTitle", request.AssignmentTitle);
                parameters.Add("@AssignmentDescription", request.AssignmentDescription);
                parameters.Add("@TotalMarks", request.TotalMarks);
                parameters.Add("@DueDate", request.DueDate);
                parameters.Add("@AttachmentPath", request.AttachmentPath);
                parameters.Add("@IsActive", request.IsActive);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateAssignment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update assignment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assignment Id: {AssignmentId}", request.AssignmentId);
                throw;
            }
        }

        public async Task<DbResult> DeleteAssignment(int assignmentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", assignmentId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteAssignment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete assignment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting assignment Id: {AssignmentId}", assignmentId);
                throw;
            }
        }

        public async Task<DbResult> SubmitAssignment(SubmitAssignmentRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", request.AssignmentId);
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@FileName", request.FileName);
                parameters.Add("@FilePath", request.FilePath);
                parameters.Add("@SubmissionRemarks", request.SubmissionRemarks);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_SubmitAssignment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to submit assignment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting assignment Id: {AssignmentId}, StudentId: {StudentId}", request.AssignmentId, request.StudentId);
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentSubmissionDto>> GetAssignmentSubmissions(int assignmentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AssignmentId", assignmentId);

                return await connection.QueryAsync<AssignmentSubmissionDto>(
                    "sp_GetAssignmentSubmissions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching submissions for AssignmentId: {AssignmentId}", assignmentId);
                throw;
            }
        }

        public async Task<AssignmentSubmissionDto?> GetSubmissionById(int submissionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SubmissionId", submissionId);

                return await connection.QueryFirstOrDefaultAsync<AssignmentSubmissionDto>(
                    "sp_GetSubmissionById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching submission by Id: {SubmissionId}", submissionId);
                throw;
            }
        }

        public async Task<DbResult> GradeAssignment(GradeAssignmentRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@SubmissionId", request.SubmissionId);
                parameters.Add("@MarksObtained", request.MarksObtained);
                parameters.Add("@InstructorFeedback", request.InstructorFeedback);
                parameters.Add("@ReviewedBy", request.ReviewedBy);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_GradeAssignment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to grade assignment." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error grading submission Id: {SubmissionId}", request.SubmissionId);
                throw;
            }
        }
    }
}
