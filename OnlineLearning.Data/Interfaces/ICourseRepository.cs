using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface ICourseRepository
    {
        // Categories
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task<CategoryDto?> GetCategoryById(int categoryId);
        Task<DbResult> CreateCategory(CreateCategoryRequest request);
        Task<DbResult> UpdateCategory(UpdateCategoryRequest request);
        Task<DbResult> DeleteCategory(int categoryId);

        // Courses
        Task<IEnumerable<CourseDto>> GetCourses();
        Task<CourseDto?> GetCourseById(int courseId);
        Task<DbResult> CreateCourse(CreateCourseRequest request);
        Task<DbResult> UpdateCourse(UpdateCourseRequest request);
        Task<DbResult> DeleteCourse(int courseId, int instructorId);
        Task<IEnumerable<CourseDto>> SearchCourses(string? searchTerm, int? categoryId, string? courseLevel, decimal? minPrice, decimal? maxPrice);
        Task<IEnumerable<CourseDto>> GetCoursesByCategory(int categoryId);
        Task<IEnumerable<CourseDto>> GetCoursesByInstructor(int instructorId);
        Task<DbResult> PublishCourse(int courseId, int instructorId);
        Task<DbResult> UnpublishCourse(int courseId, int instructorId);

        // Approvals
        Task<IEnumerable<CourseDto>> GetPendingCourses();
        Task<DbResult> ApproveCourse(CourseApprovalRequest request);
        Task<DbResult> RejectCourse(CourseApprovalRequest request);
        Task<IEnumerable<CourseApprovalHistoryDto>> GetCourseApprovalHistory(int courseId);
    }
}
