using System;

namespace OnlineLearning.Core.Entities
{
    // ================= Quiz =================
    public class QuizDto
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxAttempts { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateQuizRequest
    {
        public int CourseId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxAttempts { get; set; }
    }

    public class UpdateQuizRequest
    {
        public int QuizId { get; set; }
        public int CourseId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalMarks { get; set; }
        public int PassingMarks { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxAttempts { get; set; }
        public bool IsActive { get; set; }
    }

    // ================= Quiz Questions =================
    public class QuizQuestionDto
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = "SingleChoice";
        public decimal Marks { get; set; }
        public int QuestionOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateQuizQuestionRequest
    {
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = "SingleChoice";
        public decimal Marks { get; set; }
        public int QuestionOrder { get; set; }
    }

    public class UpdateQuizQuestionRequest
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = "SingleChoice";
        public decimal Marks { get; set; }
        public int QuestionOrder { get; set; }
    }

    // ================= Quiz Options =================
    public class QuizOptionDto
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OptionOrder { get; set; }
    }

    public class CreateQuizOptionRequest
    {
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OptionOrder { get; set; }
    }

    public class UpdateQuizOptionRequest
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OptionOrder { get; set; }
    }

    // ================= Quiz Attempts =================
    public class SubmitQuizAttemptRequest
    {
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public decimal Score { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsPassed { get; set; }
        public int TimeSpentMinutes { get; set; }
    }

    public class QuizAttemptDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string? QuizTitle { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public decimal Score { get; set; }
        public decimal TotalMarks { get; set; }
        public bool IsPassed { get; set; }
        public int TimeSpentMinutes { get; set; }
        public DateTime AttemptDate { get; set; }
    }

    // ================= Assignment =================
    public class AssignmentDto
    {
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string? AssignmentDescription { get; set; }
        public decimal TotalMarks { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentPath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateAssignmentRequest
    {
        public int CourseId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string? AssignmentDescription { get; set; }
        public decimal TotalMarks { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentPath { get; set; }
    }

    public class UpdateAssignmentRequest
    {
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string? AssignmentDescription { get; set; }
        public decimal TotalMarks { get; set; }
        public DateTime DueDate { get; set; }
        public string? AttachmentPath { get; set; }
        public bool IsActive { get; set; }
    }

    public class SubmitAssignmentRequest
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? SubmissionRemarks { get; set; }
    }

    public class AssignmentSubmissionDto
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? SubmissionRemarks { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal? MarksObtained { get; set; }
        public string? InstructorFeedback { get; set; }
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string Status { get; set; } = "Submitted";
    }

    public class GradeAssignmentRequest
    {
        public int SubmissionId { get; set; }
        public decimal MarksObtained { get; set; }
        public string? InstructorFeedback { get; set; }
        public int ReviewedBy { get; set; }
    }
}
