using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface ISectionLessonRepository
    {
        // Sections
        Task<IEnumerable<SectionDto>> GetSections(int courseId);
        Task<SectionDto?> GetSectionById(int sectionId);
        Task<DbResult> CreateSection(CreateSectionRequest request);
        Task<DbResult> UpdateSection(UpdateSectionRequest request);
        Task<DbResult> DeleteSection(int sectionId);

        // Lessons
        Task<IEnumerable<LessonDto>> GetLessons(int sectionId);
        Task<LessonDto?> GetLessonById(int lessonId);
        Task<DbResult> CreateLesson(CreateLessonRequest request);
        Task<DbResult> UpdateLesson(UpdateLessonRequest request);
        Task<DbResult> DeleteLesson(int lessonId);

        // Resources
        Task<IEnumerable<LessonResourceDto>> GetLessonResources(int lessonId);
        Task<LessonResourceDto?> GetLessonResourceById(int resourceId);
        Task<DbResult> AddLessonResource(AddLessonResourceRequest request);
        Task<DbResult> UpdateLessonResource(UpdateLessonResourceRequest request);
        Task<DbResult> DeleteLessonResource(int resourceId);

        // Progress
        Task<DbResult> UpdateLessonProgress(LessonProgressRequest request);
        Task<LessonProgressDto?> GetLessonProgress(int userId, int lessonId);
        Task<CourseProgressDto?> GetCourseProgress(int userId, int courseId);
        Task<DbResult> CompleteCourse(int userId, int courseId);
    }
}
