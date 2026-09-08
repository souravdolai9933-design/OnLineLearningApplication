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
    public class QuizRepository : IQuizRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<QuizRepository> _logger;

        public QuizRepository(DbConnectionFactory dbConnectionFactory, ILogger<QuizRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Quizzes =================
        public async Task<IEnumerable<QuizDto>> GetQuizzes()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<QuizDto>(
                    "sp_GetQuizzes",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quizzes");
                throw;
            }
        }

        public async Task<QuizDto?> GetQuizById(int quizId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", quizId);

                return await connection.QueryFirstOrDefaultAsync<QuizDto>(
                    "sp_GetQuizById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz by Id: {QuizId}", quizId);
                throw;
            }
        }

        public async Task<IEnumerable<QuizDto>> GetQuizzesByCourse(int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", courseId);

                return await connection.QueryAsync<QuizDto>(
                    "sp_GetQuizzesByCourse",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quizzes for CourseId: {CourseId}", courseId);
                throw;
            }
        }

        public async Task<DbResult> CreateQuiz(CreateQuizRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@QuizTitle", request.QuizTitle);
                parameters.Add("@Description", request.Description);
                parameters.Add("@TotalMarks", request.TotalMarks);
                parameters.Add("@PassingMarks", request.PassingMarks);
                parameters.Add("@DurationMinutes", request.DurationMinutes);
                parameters.Add("@MaxAttempts", request.MaxAttempts);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateQuiz",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create quiz." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz: {QuizTitle}", request.QuizTitle);
                throw;
            }
        }

        public async Task<DbResult> UpdateQuiz(UpdateQuizRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", request.QuizId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@QuizTitle", request.QuizTitle);
                parameters.Add("@Description", request.Description);
                parameters.Add("@TotalMarks", request.TotalMarks);
                parameters.Add("@PassingMarks", request.PassingMarks);
                parameters.Add("@DurationMinutes", request.DurationMinutes);
                parameters.Add("@MaxAttempts", request.MaxAttempts);
                parameters.Add("@IsActive", request.IsActive);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateQuiz",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update quiz." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quiz Id: {QuizId}", request.QuizId);
                throw;
            }
        }

        public async Task<DbResult> DeleteQuiz(int quizId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", quizId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteQuiz",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete quiz." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz Id: {QuizId}", quizId);
                throw;
            }
        }

        // ================= Questions =================
        public async Task<IEnumerable<QuizQuestionDto>> GetQuizQuestions(int quizId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", quizId);

                return await connection.QueryAsync<QuizQuestionDto>(
                    "sp_GetQuizQuestions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching questions for QuizId: {QuizId}", quizId);
                throw;
            }
        }

        public async Task<QuizQuestionDto?> GetQuizQuestionById(int questionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", questionId);

                return await connection.QueryFirstOrDefaultAsync<QuizQuestionDto>(
                    "sp_GetQuizQuestionById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching question by Id: {QuestionId}", questionId);
                throw;
            }
        }

        public async Task<DbResult> CreateQuizQuestion(CreateQuizQuestionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", request.QuizId);
                parameters.Add("@QuestionText", request.QuestionText);
                parameters.Add("@QuestionType", request.QuestionType);
                parameters.Add("@Marks", request.Marks);
                parameters.Add("@QuestionOrder", request.QuestionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateQuizQuestion",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create quiz question." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz question for QuizId: {QuizId}", request.QuizId);
                throw;
            }
        }

        public async Task<DbResult> UpdateQuizQuestion(UpdateQuizQuestionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", request.QuestionId);
                parameters.Add("@QuizId", request.QuizId);
                parameters.Add("@QuestionText", request.QuestionText);
                parameters.Add("@QuestionType", request.QuestionType);
                parameters.Add("@Marks", request.Marks);
                parameters.Add("@QuestionOrder", request.QuestionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateQuizQuestion",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update quiz question." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question Id: {QuestionId}", request.QuestionId);
                throw;
            }
        }

        public async Task<DbResult> DeleteQuizQuestion(int questionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", questionId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteQuizQuestion",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete question." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question Id: {QuestionId}", questionId);
                throw;
            }
        }

        // ================= Options =================
        public async Task<IEnumerable<QuizOptionDto>> GetQuizOptions(int questionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", questionId);

                return await connection.QueryAsync<QuizOptionDto>(
                    "sp_GetQuizOptions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching options for QuestionId: {QuestionId}", questionId);
                throw;
            }
        }

        public async Task<DbResult> CreateQuizOption(CreateQuizOptionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", request.QuestionId);
                parameters.Add("@OptionText", request.OptionText);
                parameters.Add("@IsCorrect", request.IsCorrect);
                parameters.Add("@OptionOrder", request.OptionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateQuizOption",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create quiz option." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz option for QuestionId: {QuestionId}", request.QuestionId);
                throw;
            }
        }

        public async Task<DbResult> UpdateQuizOption(UpdateQuizOptionRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@OptionId", request.OptionId);
                parameters.Add("@QuestionId", request.QuestionId);
                parameters.Add("@OptionText", request.OptionText);
                parameters.Add("@IsCorrect", request.IsCorrect);
                parameters.Add("@OptionOrder", request.OptionOrder);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_UpdateQuizOption",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to update quiz option." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating option Id: {OptionId}", request.OptionId);
                throw;
            }
        }

        public async Task<DbResult> DeleteQuizOption(int optionId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@OptionId", optionId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_DeleteQuizOption",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to delete option." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting option Id: {OptionId}", optionId);
                throw;
            }
        }

        // ================= Attempts =================
        public async Task<DbResult> SubmitQuizAttempt(SubmitQuizAttemptRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", request.QuizId);
                parameters.Add("@StudentId", request.StudentId);
                parameters.Add("@Score", request.Score);
                parameters.Add("@TotalMarks", request.TotalMarks);
                parameters.Add("@IsPassed", request.IsPassed);
                parameters.Add("@TimeSpentMinutes", request.TimeSpentMinutes);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_SubmitQuizAttempt",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to submit quiz attempt." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting quiz attempt for QuizId: {QuizId}, StudentId: {StudentId}", request.QuizId, request.StudentId);
                throw;
            }
        }

        public async Task<IEnumerable<QuizAttemptDto>> GetQuizAttempts(int quizId, int? studentId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@QuizId", quizId);
                parameters.Add("@StudentId", studentId);

                return await connection.QueryAsync<QuizAttemptDto>(
                    "sp_GetQuizAttempts",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz attempts for QuizId: {QuizId}", quizId);
                throw;
            }
        }

        public async Task<QuizAttemptDto?> GetQuizAttemptById(int attemptId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@AttemptId", attemptId);

                return await connection.QueryFirstOrDefaultAsync<QuizAttemptDto>(
                    "sp_GetQuizAttemptById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attempt by Id: {AttemptId}", attemptId);
                throw;
            }
        }
    }
}
