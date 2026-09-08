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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseRepository courseRepository, ILogger<CoursesController> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        // ================= Categories =================
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all course categories");
                var categories = await _courseRepository.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Category ID is required." });

            try
            {
                _logger.LogInformation("Fetching category by Id: {CategoryId}", id);
                var category = await _courseRepository.GetCategoryById(id);
                if (category == null)
                    return NotFound(new { Message = "Category not found." });

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category by Id: {CategoryId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(new { Message = "Category Name is required." });

            try
            {
                _logger.LogInformation("Creating category: {CategoryName}", request.CategoryName);
                var result = await _courseRepository.CreateCategory(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryName}", request.CategoryName);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            if (id <= 0 || request == null || request.CategoryId != id || string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest(new { Message = "Valid Category ID and Name are required." });

            try
            {
                _logger.LogInformation("Updating category Id: {CategoryId}", id);
                var result = await _courseRepository.UpdateCategory(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category Id: {CategoryId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Category ID is required." });

            try
            {
                _logger.LogInformation("Deleting category Id: {CategoryId}", id);
                var result = await _courseRepository.DeleteCategory(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category Id: {CategoryId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Courses =================
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                _logger.LogInformation("Fetching all published courses");
                var courses = await _courseRepository.GetCourses();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching course by Id: {CourseId}", id);
                var course = await _courseRepository.GetCourseById(id);
                if (course == null)
                    return NotFound(new { Message = "Course not found." });

                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course by Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses(
            [FromQuery] string? searchTerm,
            [FromQuery] int? categoryId,
            [FromQuery] string? courseLevel,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            try
            {
                _logger.LogInformation("Searching courses with term: {SearchTerm}, Category: {CategoryId}", searchTerm, categoryId);
                var courses = await _courseRepository.SearchCourses(searchTerm, categoryId, courseLevel, minPrice, maxPrice);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching courses");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetCoursesByCategory(int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest(new { Message = "Valid Category ID is required." });

            try
            {
                _logger.LogInformation("Fetching courses by CategoryId: {CategoryId}", categoryId);
                var courses = await _courseRepository.GetCoursesByCategory(categoryId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses for CategoryId: {CategoryId}", categoryId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("instructor/{instructorId:int}")]
        public async Task<IActionResult> GetCoursesByInstructor(int instructorId)
        {
            if (instructorId <= 0)
                return BadRequest(new { Message = "Valid Instructor ID is required." });

            try
            {
                _logger.LogInformation("Fetching courses for InstructorId: {InstructorId}", instructorId);
                var courses = await _courseRepository.GetCoursesByInstructor(instructorId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses for InstructorId: {InstructorId}", instructorId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || request.InstructorId <= 0 || request.CategoryId <= 0)
                return BadRequest(new { Message = "Instructor ID, Category ID, and Course Title are required." });

            try
            {
                _logger.LogInformation("Creating course: {Title} by Instructor: {InstructorId}", request.Title, request.InstructorId);
                var result = await _courseRepository.CreateCourse(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course: {Title}", request.Title);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            if (id <= 0 || request == null || request.CourseId != id || string.IsNullOrWhiteSpace(request.Title))
                return BadRequest(new { Message = "Valid Course ID and Title are required." });

            try
            {
                _logger.LogInformation("Updating course Id: {CourseId}", id);
                var result = await _courseRepository.UpdateCourse(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse(int id, [FromQuery] int instructorId)
        {
            if (id <= 0 || instructorId <= 0)
                return BadRequest(new { Message = "Valid Course ID and Instructor ID are required." });

            try
            {
                _logger.LogInformation("Deleting course Id: {CourseId} by Instructor: {InstructorId}", id, instructorId);
                var result = await _courseRepository.DeleteCourse(id, instructorId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}/publish")]
        public async Task<IActionResult> PublishCourse(int id, [FromQuery] int instructorId)
        {
            if (id <= 0 || instructorId <= 0)
                return BadRequest(new { Message = "Valid Course ID and Instructor ID are required." });

            try
            {
                _logger.LogInformation("Publishing course Id: {CourseId}", id);
                var result = await _courseRepository.PublishCourse(id, instructorId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}/unpublish")]
        public async Task<IActionResult> UnpublishCourse(int id, [FromQuery] int instructorId)
        {
            if (id <= 0 || instructorId <= 0)
                return BadRequest(new { Message = "Valid Course ID and Instructor ID are required." });

            try
            {
                _logger.LogInformation("Unpublishing course Id: {CourseId}", id);
                var result = await _courseRepository.UnpublishCourse(id, instructorId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unpublishing course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Course Approvals =================
        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingCourses()
        {
            try
            {
                _logger.LogInformation("Fetching pending courses for admin review");
                var courses = await _courseRepository.GetPendingCourses();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending courses");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> ApproveCourse(int id, [FromBody] CourseApprovalRequest request)
        {
            if (id <= 0 || request == null || request.CourseId != id || request.AdminId <= 0)
                return BadRequest(new { Message = "Valid Course ID and Admin ID are required." });

            try
            {
                _logger.LogInformation("Approving course Id: {CourseId} by Admin: {AdminId}", id, request.AdminId);
                var result = await _courseRepository.ApproveCourse(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> RejectCourse(int id, [FromBody] CourseApprovalRequest request)
        {
            if (id <= 0 || request == null || request.CourseId != id || request.AdminId <= 0)
                return BadRequest(new { Message = "Valid Course ID and Admin ID are required." });

            try
            {
                _logger.LogInformation("Rejecting course Id: {CourseId} by Admin: {AdminId}", id, request.AdminId);
                var result = await _courseRepository.RejectCourse(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting course Id: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin,Instructor")]
        [HttpGet("{id:int}/approval-history")]
        public async Task<IActionResult> GetCourseApprovalHistory(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching approval history for CourseId: {CourseId}", id);
                var history = await _courseRepository.GetCourseApprovalHistory(id);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching approval history for CourseId: {CourseId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
