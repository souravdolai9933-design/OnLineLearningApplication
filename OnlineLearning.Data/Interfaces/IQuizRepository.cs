using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IQuizRepository
    {
        // Quizzes
        Task<IEnumerable<QuizDto>> GetQuizzes();
        Task<QuizDto?> GetQuizById(int quizId);
        Task<IEnumerable<QuizDto>> GetQuizzesByCourse(int courseId);
        Task<DbResult> CreateQuiz(CreateQuizRequest request);
        Task<DbResult> UpdateQuiz(UpdateQuizRequest request);
        Task<DbResult> DeleteQuiz(int quizId);

        // Questions
        Task<IEnumerable<QuizQuestionDto>> GetQuizQuestions(int quizId);
        Task<QuizQuestionDto?> GetQuizQuestionById(int questionId);
        Task<DbResult> CreateQuizQuestion(CreateQuizQuestionRequest request);
        Task<DbResult> UpdateQuizQuestion(UpdateQuizQuestionRequest request);
        Task<DbResult> DeleteQuizQuestion(int questionId);

        // Options
        Task<IEnumerable<QuizOptionDto>> GetQuizOptions(int questionId);
        Task<DbResult> CreateQuizOption(CreateQuizOptionRequest request);
        Task<DbResult> UpdateQuizOption(UpdateQuizOptionRequest request);
        Task<DbResult> DeleteQuizOption(int optionId);

        // Attempts
        Task<DbResult> SubmitQuizAttempt(SubmitQuizAttemptRequest request);
        Task<IEnumerable<QuizAttemptDto>> GetQuizAttempts(int quizId, int? studentId);
        Task<QuizAttemptDto?> GetQuizAttemptById(int attemptId);
    }
}
