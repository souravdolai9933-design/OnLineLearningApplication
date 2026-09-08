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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewRepository reviewRepository, ILogger<ReviewsController> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetCourseReviews(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching reviews for CourseId: {CourseId}", courseId);
                var reviews = await _reviewRepository.GetCourseReviews(courseId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reviews for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Review ID is required." });

            try
            {
                _logger.LogInformation("Fetching review by Id: {ReviewId}", id);
                var review = await _reviewRepository.GetReviewById(id);
                if (review == null)
                    return NotFound(new { Message = "Review not found." });

                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching review by Id: {ReviewId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            if (request == null || request.CourseId <= 0 || request.UserId <= 0 || request.Rating < 1 || request.Rating > 5)
                return BadRequest(new { Message = "Valid Course ID, User ID, and Rating (1-5) are required." });

            try
            {
                _logger.LogInformation("Creating review for CourseId: {CourseId} by UserId: {UserId}", request.CourseId, request.UserId);
                var result = await _reviewRepository.CreateReview(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewRequest request)
        {
            if (id <= 0 || request == null || request.ReviewId != id || request.UserId <= 0 || request.Rating < 1 || request.Rating > 5)
                return BadRequest(new { Message = "Valid Review ID, User ID, and Rating (1-5) are required." });

            try
            {
                _logger.LogInformation("Updating review Id: {ReviewId}", id);
                var result = await _reviewRepository.UpdateReview(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review Id: {ReviewId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReview(int id, [FromQuery] int userId)
        {
            if (id <= 0 || userId <= 0)
                return BadRequest(new { Message = "Valid Review ID and User ID are required." });

            try
            {
                _logger.LogInformation("Deleting review Id: {ReviewId} by UserId: {UserId}", id, userId);
                var result = await _reviewRepository.DeleteReview(id, userId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review Id: {ReviewId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
