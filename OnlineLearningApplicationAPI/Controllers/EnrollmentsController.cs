using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineLearning.Core.Entities;
using OnlineLearning.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineLearningApplicationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(IEnrollmentRepository enrollmentRepository, ILogger<EnrollmentsController> logger)
        {
            _enrollmentRepository = enrollmentRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollStudentRequest request)
        {
            if (request == null || request.UserId <= 0 || request.CourseId <= 0)
                return BadRequest(new { Message = "Valid User ID and Course ID are required." });

            try
            {
                _logger.LogInformation("Enrolling student: {UserId} in Course: {CourseId}", request.UserId, request.CourseId);
                var result = await _enrollmentRepository.EnrollStudent(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling student: {UserId} in Course: {CourseId}", request.UserId, request.CourseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("student/{studentId:int}")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            if (studentId <= 0)
                return BadRequest(new { Message = "Valid Student ID is required." });

            try
            {
                _logger.LogInformation("Fetching enrolled courses for StudentId: {StudentId}", studentId);
                var courses = await _enrollmentRepository.GetStudentCourses(studentId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student courses for StudentId: {StudentId}", studentId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Enrollment ID is required." });

            try
            {
                _logger.LogInformation("Fetching enrollment by Id: {EnrollmentId}", id);
                var enrollment = await _enrollmentRepository.GetEnrollmentById(id);
                if (enrollment == null)
                    return NotFound(new { Message = "Enrollment not found." });

                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching enrollment by Id: {EnrollmentId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelEnrollment(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Enrollment ID is required." });

            try
            {
                _logger.LogInformation("Cancelling enrollment Id: {EnrollmentId}", id);
                var result = await _enrollmentRepository.CancelEnrollment(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling enrollment Id: {EnrollmentId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
