using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineLearning.Core.Entities;
using OnlineLearning.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineLearningApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsLessonsController : ControllerBase
    {
        private readonly ISectionLessonRepository _sectionLessonRepository;
        private readonly ILogger<SectionsLessonsController> _logger;

        public SectionsLessonsController(ISectionLessonRepository sectionLessonRepository, ILogger<SectionsLessonsController> logger)
        {
            _sectionLessonRepository = sectionLessonRepository;
            _logger = logger;
        }

        // ================= Sections =================
        [HttpGet("sections/course/{courseId:int}")]
        public async Task<IActionResult> GetSections(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching sections for CourseId: {CourseId}", courseId);
                var sections = await _sectionLessonRepository.GetSections(courseId);
                return Ok(sections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sections for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("sections/{id:int}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Section ID is required." });

            try
            {
                _logger.LogInformation("Fetching section by Id: {SectionId}", id);
                var section = await _sectionLessonRepository.GetSectionById(id);
                if (section == null)
                    return NotFound(new { Message = "Section not found." });

                return Ok(section);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching section by Id: {SectionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("sections")]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionRequest request)
        {
            if (request == null || request.CourseId <= 0 || string.IsNullOrWhiteSpace(request.SectionTitle))
                return BadRequest(new { Message = "Course ID and Section Title are required." });

            try
            {
                _logger.LogInformation("Creating section: {SectionTitle} for CourseId: {CourseId}", request.SectionTitle, request.CourseId);
                var result = await _sectionLessonRepository.CreateSection(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating section: {SectionTitle}", request.SectionTitle);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("sections/{id:int}")]
        public async Task<IActionResult> UpdateSection(int id, [FromBody] UpdateSectionRequest request)
        {
            if (id <= 0 || request == null || request.SectionId != id || string.IsNullOrWhiteSpace(request.SectionTitle))
                return BadRequest(new { Message = "Valid Section ID and Section Title are required." });

            try
            {
                _logger.LogInformation("Updating section Id: {SectionId}", id);
                var result = await _sectionLessonRepository.UpdateSection(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating section Id: {SectionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("sections/{id:int}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Section ID is required." });

            try
            {
                _logger.LogInformation("Deleting section Id: {SectionId}", id);
                var result = await _sectionLessonRepository.DeleteSection(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting section Id: {SectionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Lessons =================
        [HttpGet("lessons/section/{sectionId:int}")]
        public async Task<IActionResult> GetLessons(int sectionId)
        {
            if (sectionId <= 0)
                return BadRequest(new { Message = "Valid Section ID is required." });

            try
            {
                _logger.LogInformation("Fetching lessons for SectionId: {SectionId}", sectionId);
                var lessons = await _sectionLessonRepository.GetLessons(sectionId);
                return Ok(lessons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lessons for SectionId: {SectionId}", sectionId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("lessons/{id:int}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Lesson ID is required." });

            try
            {
                _logger.LogInformation("Fetching lesson by Id: {LessonId}", id);
                var lesson = await _sectionLessonRepository.GetLessonById(id);
                if (lesson == null)
                    return NotFound(new { Message = "Lesson not found." });

                return Ok(lesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lesson by Id: {LessonId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("lessons")]
        public async Task<IActionResult> CreateLesson([FromBody] CreateLessonRequest request)
        {
            if (request == null || request.SectionId <= 0 || string.IsNullOrWhiteSpace(request.LessonTitle))
                return BadRequest(new { Message = "Section ID and Lesson Title are required." });

            try
            {
                _logger.LogInformation("Creating lesson: {LessonTitle} for SectionId: {SectionId}", request.LessonTitle, request.SectionId);
                var result = await _sectionLessonRepository.CreateLesson(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lesson: {LessonTitle}", request.LessonTitle);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("lessons/{id:int}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonRequest request)
        {
            if (id <= 0 || request == null || request.LessonId != id || string.IsNullOrWhiteSpace(request.LessonTitle))
                return BadRequest(new { Message = "Valid Lesson ID and Lesson Title are required." });

            try
            {
                _logger.LogInformation("Updating lesson Id: {LessonId}", id);
                var result = await _sectionLessonRepository.UpdateLesson(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson Id: {LessonId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("lessons/{id:int}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Lesson ID is required." });

            try
            {
                _logger.LogInformation("Deleting lesson Id: {LessonId}", id);
                var result = await _sectionLessonRepository.DeleteLesson(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson Id: {LessonId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Resources =================
        [HttpGet("lessons/{lessonId:int}/resources")]
        public async Task<IActionResult> GetLessonResources(int lessonId)
        {
            if (lessonId <= 0)
                return BadRequest(new { Message = "Valid Lesson ID is required." });

            try
            {
                _logger.LogInformation("Fetching resources for LessonId: {LessonId}", lessonId);
                var resources = await _sectionLessonRepository.GetLessonResources(lessonId);
                return Ok(resources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching resources for LessonId: {LessonId}", lessonId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("lessons/{lessonId:int}/resources")]
        public async Task<IActionResult> AddLessonResource(int lessonId, [FromBody] AddLessonResourceRequest request)
        {
            if (lessonId <= 0 || request == null || string.IsNullOrWhiteSpace(request.ResourceTitle) || string.IsNullOrWhiteSpace(request.FileUrl))
                return BadRequest(new { Message = "Valid Lesson ID, Resource Title, and File URL are required." });

            request.LessonId = lessonId;

            try
            {
                _logger.LogInformation("Adding resource: {ResourceTitle} to LessonId: {LessonId}", request.ResourceTitle, lessonId);
                var result = await _sectionLessonRepository.AddLessonResource(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding resource to LessonId: {LessonId}", lessonId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("lessons/resources/{resourceId:int}")]
        public async Task<IActionResult> DeleteLessonResource(int resourceId)
        {
            if (resourceId <= 0)
                return BadRequest(new { Message = "Valid Resource ID is required." });

            try
            {
                _logger.LogInformation("Deleting resource Id: {ResourceId}", resourceId);
                var result = await _sectionLessonRepository.DeleteLessonResource(resourceId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting resource Id: {ResourceId}", resourceId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Progress =================
        [Authorize]
        [HttpPost("progress")]
        public async Task<IActionResult> UpdateLessonProgress([FromBody] LessonProgressRequest request)
        {
            if (request == null || request.UserId <= 0 || request.CourseId <= 0 || request.LessonId <= 0)
                return BadRequest(new { Message = "User ID, Course ID, and Lesson ID are required." });

            try
            {
                _logger.LogInformation("Updating progress for UserId: {UserId}, LessonId: {LessonId}", request.UserId, request.LessonId);
                var result = await _sectionLessonRepository.UpdateLessonProgress(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lesson progress");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("progress/lesson/{lessonId:int}/user/{userId:int}")]
        public async Task<IActionResult> GetLessonProgress(int lessonId, int userId)
        {
            if (lessonId <= 0 || userId <= 0)
                return BadRequest(new { Message = "Valid Lesson ID and User ID are required." });

            try
            {
                _logger.LogInformation("Fetching progress for UserId: {UserId}, LessonId: {LessonId}", userId, lessonId);
                var progress = await _sectionLessonRepository.GetLessonProgress(userId, lessonId);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching progress for LessonId: {LessonId}", lessonId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("progress/course/{courseId:int}/user/{userId:int}")]
        public async Task<IActionResult> GetCourseProgress(int courseId, int userId)
        {
            if (courseId <= 0 || userId <= 0)
                return BadRequest(new { Message = "Valid Course ID and User ID are required." });

            try
            {
                _logger.LogInformation("Fetching progress for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                var progress = await _sectionLessonRepository.GetCourseProgress(userId, courseId);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course progress for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPost("progress/course/{courseId:int}/complete/user/{userId:int}")]
        public async Task<IActionResult> CompleteCourse(int courseId, int userId)
        {
            if (courseId <= 0 || userId <= 0)
                return BadRequest(new { Message = "Valid Course ID and User ID are required." });

            try
            {
                _logger.LogInformation("Marking course completed for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                var result = await _sectionLessonRepository.CompleteCourse(userId, courseId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking course complete for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
