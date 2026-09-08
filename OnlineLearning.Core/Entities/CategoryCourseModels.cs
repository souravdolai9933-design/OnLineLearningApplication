using System;

namespace OnlineLearning.Core.Entities
{
    // ================= Category =================
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CategoryImage { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CategoryImage { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CategoryImage { get; set; }
        public string Status { get; set; } = "Active";
    }

    // ================= Course =================
    public class CourseDto
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CourseLevel { get; set; }
        public string? Language { get; set; }
        public string? Thumbnail { get; set; }
        public string? PromoVideoUrl { get; set; }
        public string Status { get; set; } = "Draft";
        public bool IsApproved { get; set; }
        public int TotalDuration { get; set; }
        public int TotalLessons { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCourseRequest
    {
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CourseLevel { get; set; }
        public string? Language { get; set; }
        public string? Thumbnail { get; set; }
        public string? PromoVideoUrl { get; set; }
    }

    public class UpdateCourseRequest
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? CourseLevel { get; set; }
        public string? Language { get; set; }
        public string? Thumbnail { get; set; }
        public string? PromoVideoUrl { get; set; }
    }

    public class CourseApprovalRequest
    {
        public int CourseId { get; set; }
        public int AdminId { get; set; }
        public string? Remarks { get; set; }
    }

    public class CourseApprovalHistoryDto
    {
        public int ApprovalId { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public int AdminId { get; set; }
        public string? AdminName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime ActionDate { get; set; }
    }

    // ================= Section =================
    public class SectionDto
    {
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int SectionOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateSectionRequest
    {
        public int CourseId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int SectionOrder { get; set; }
    }

    public class UpdateSectionRequest
    {
        public int SectionId { get; set; }
        public int CourseId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int SectionOrder { get; set; }
    }

    // ================= Lesson =================
    public class LessonDto
    {
        public int LessonId { get; set; }
        public int SectionId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string? LessonType { get; set; }
        public string? VideoUrl { get; set; }
        public int Duration { get; set; }
        public string? Content { get; set; }
        public bool IsPreview { get; set; }
        public int LessonOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateLessonRequest
    {
        public int SectionId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string? LessonType { get; set; }
        public string? VideoUrl { get; set; }
        public int Duration { get; set; }
        public string? Content { get; set; }
        public bool IsPreview { get; set; }
        public int LessonOrder { get; set; }
    }

    public class UpdateLessonRequest
    {
        public int LessonId { get; set; }
        public int SectionId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public string? LessonType { get; set; }
        public string? VideoUrl { get; set; }
        public int Duration { get; set; }
        public string? Content { get; set; }
        public bool IsPreview { get; set; }
        public int LessonOrder { get; set; }
    }

    // ================= Lesson Resources =================
    public class LessonResourceDto
    {
        public int ResourceId { get; set; }
        public int LessonId { get; set; }
        public string ResourceTitle { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AddLessonResourceRequest
    {
        public int LessonId { get; set; }
        public string ResourceTitle { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
    }

    public class UpdateLessonResourceRequest
    {
        public int ResourceId { get; set; }
        public int LessonId { get; set; }
        public string ResourceTitle { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
    }

    // ================= Progress =================
    public class LessonProgressRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchedDuration { get; set; }
    }

    public class LessonProgressDto
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchedDuration { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CourseProgressDto
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool IsCourseCompleted { get; set; }
    }
}
