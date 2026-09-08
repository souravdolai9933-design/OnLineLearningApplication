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
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILogger<AssignmentsController> _logger;

        public AssignmentsController(IAssignmentRepository assignmentRepository, ILogger<AssignmentsController> logger)
        {
            _assignmentRepository = assignmentRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignments()
        {
            try
            {
                _logger.LogInformation("Fetching all assignments");
                var assignments = await _assignmentRepository.GetAssignments();
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignments");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Assignment ID is required." });

            try
            {
                _logger.LogInformation("Fetching assignment by Id: {AssignmentId}", id);
                var assignment = await _assignmentRepository.GetAssignmentById(id);
                if (assignment == null)
                    return NotFound(new { Message = "Assignment not found." });

                return Ok(assignment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignment by Id: {AssignmentId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetAssignmentsByCourse(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching assignments for CourseId: {CourseId}", courseId);
                var assignments = await _assignmentRepository.GetAssignmentsByCourse(courseId);
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching assignments for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request)
        {
            if (request == null || request.CourseId <= 0 || string.IsNullOrWhiteSpace(request.AssignmentTitle))
                return BadRequest(new { Message = "Course ID and Assignment Title are required." });

            try
            {
                _logger.LogInformation("Creating assignment: {AssignmentTitle} for CourseId: {CourseId}", request.AssignmentTitle, request.CourseId);
                var result = await _assignmentRepository.CreateAssignment(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assignment: {AssignmentTitle}", request.AssignmentTitle);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] UpdateAssignmentRequest request)
        {
            if (id <= 0 || request == null || request.AssignmentId != id || string.IsNullOrWhiteSpace(request.AssignmentTitle))
                return BadRequest(new { Message = "Valid Assignment ID and Title are required." });

            try
            {
                _logger.LogInformation("Updating assignment Id: {AssignmentId}", id);
                var result = await _assignmentRepository.UpdateAssignment(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assignment Id: {AssignmentId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Assignment ID is required." });

            try
            {
                _logger.LogInformation("Deleting assignment Id: {AssignmentId}", id);
                var result = await _assignmentRepository.DeleteAssignment(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting assignment Id: {AssignmentId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Submissions =================
        [Authorize]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAssignment([FromBody] SubmitAssignmentRequest request)
        {
            if (request == null || request.AssignmentId <= 0 || request.StudentId <= 0 || string.IsNullOrWhiteSpace(request.FilePath))
                return BadRequest(new { Message = "Valid Assignment ID, Student ID, and File Path are required." });

            try
            {
                _logger.LogInformation("Submitting assignment Id: {AssignmentId} for Student: {StudentId}", request.AssignmentId, request.StudentId);
                var result = await _assignmentRepository.SubmitAssignment(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting assignment");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpGet("{assignmentId:int}/submissions")]
        public async Task<IActionResult> GetAssignmentSubmissions(int assignmentId)
        {
            if (assignmentId <= 0)
                return BadRequest(new { Message = "Valid Assignment ID is required." });

            try
            {
                _logger.LogInformation("Fetching submissions for AssignmentId: {AssignmentId}", assignmentId);
                var submissions = await _assignmentRepository.GetAssignmentSubmissions(assignmentId);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching submissions for AssignmentId: {AssignmentId}", assignmentId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("submissions/{id:int}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Submission ID is required." });

            try
            {
                _logger.LogInformation("Fetching submission by Id: {SubmissionId}", id);
                var submission = await _assignmentRepository.GetSubmissionById(id);
                if (submission == null)
                    return NotFound(new { Message = "Submission not found." });

                return Ok(submission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching submission by Id: {SubmissionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("grade")]
        public async Task<IActionResult> GradeAssignment([FromBody] GradeAssignmentRequest request)
        {
            if (request == null || request.SubmissionId <= 0 || request.ReviewedBy <= 0)
                return BadRequest(new { Message = "Valid Submission ID and Reviewer ID are required." });

            try
            {
                _logger.LogInformation("Grading submission Id: {SubmissionId} by Reviewer: {ReviewedBy}", request.SubmissionId, request.ReviewedBy);
                var result = await _assignmentRepository.GradeAssignment(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error grading submission Id: {SubmissionId}", request.SubmissionId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
