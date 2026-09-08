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
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ILogger<QuizzesController> _logger;

        public QuizzesController(IQuizRepository quizRepository, ILogger<QuizzesController> logger)
        {
            _quizRepository = quizRepository;
            _logger = logger;
        }

        // ================= Quizzes =================
        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            try
            {
                _logger.LogInformation("Fetching all quizzes");
                var quizzes = await _quizRepository.GetQuizzes();
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quizzes");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Quiz ID is required." });

            try
            {
                _logger.LogInformation("Fetching quiz by Id: {QuizId}", id);
                var quiz = await _quizRepository.GetQuizById(id);
                if (quiz == null)
                    return NotFound(new { Message = "Quiz not found." });

                return Ok(quiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz by Id: {QuizId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("course/{courseId:int}")]
        public async Task<IActionResult> GetQuizzesByCourse(int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new { Message = "Valid Course ID is required." });

            try
            {
                _logger.LogInformation("Fetching quizzes for CourseId: {CourseId}", courseId);
                var quizzes = await _quizRepository.GetQuizzesByCourse(courseId);
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quizzes for CourseId: {CourseId}", courseId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
        {
            if (request == null || request.CourseId <= 0 || string.IsNullOrWhiteSpace(request.QuizTitle))
                return BadRequest(new { Message = "Course ID and Quiz Title are required." });

            try
            {
                _logger.LogInformation("Creating quiz: {QuizTitle} for CourseId: {CourseId}", request.QuizTitle, request.CourseId);
                var result = await _quizRepository.CreateQuiz(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz: {QuizTitle}", request.QuizTitle);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizRequest request)
        {
            if (id <= 0 || request == null || request.QuizId != id || string.IsNullOrWhiteSpace(request.QuizTitle))
                return BadRequest(new { Message = "Valid Quiz ID and Quiz Title are required." });

            try
            {
                _logger.LogInformation("Updating quiz Id: {QuizId}", id);
                var result = await _quizRepository.UpdateQuiz(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quiz Id: {QuizId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Quiz ID is required." });

            try
            {
                _logger.LogInformation("Deleting quiz Id: {QuizId}", id);
                var result = await _quizRepository.DeleteQuiz(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz Id: {QuizId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Questions =================
        [HttpGet("{quizId:int}/questions")]
        public async Task<IActionResult> GetQuizQuestions(int quizId)
        {
            if (quizId <= 0)
                return BadRequest(new { Message = "Valid Quiz ID is required." });

            try
            {
                _logger.LogInformation("Fetching questions for QuizId: {QuizId}", quizId);
                var questions = await _quizRepository.GetQuizQuestions(quizId);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching questions for QuizId: {QuizId}", quizId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("questions/{id:int}")]
        public async Task<IActionResult> GetQuizQuestionById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Question ID is required." });

            try
            {
                _logger.LogInformation("Fetching question by Id: {QuestionId}", id);
                var question = await _quizRepository.GetQuizQuestionById(id);
                if (question == null)
                    return NotFound(new { Message = "Question not found." });

                return Ok(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching question by Id: {QuestionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("questions")]
        public async Task<IActionResult> CreateQuizQuestion([FromBody] CreateQuizQuestionRequest request)
        {
            if (request == null || request.QuizId <= 0 || string.IsNullOrWhiteSpace(request.QuestionText))
                return BadRequest(new { Message = "Quiz ID and Question Text are required." });

            try
            {
                _logger.LogInformation("Creating question for QuizId: {QuizId}", request.QuizId);
                var result = await _quizRepository.CreateQuizQuestion(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question for QuizId: {QuizId}", request.QuizId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("questions/{id:int}")]
        public async Task<IActionResult> UpdateQuizQuestion(int id, [FromBody] UpdateQuizQuestionRequest request)
        {
            if (id <= 0 || request == null || request.QuestionId != id || string.IsNullOrWhiteSpace(request.QuestionText))
                return BadRequest(new { Message = "Valid Question ID and Question Text are required." });

            try
            {
                _logger.LogInformation("Updating question Id: {QuestionId}", id);
                var result = await _quizRepository.UpdateQuizQuestion(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question Id: {QuestionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("questions/{id:int}")]
        public async Task<IActionResult> DeleteQuizQuestion(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Question ID is required." });

            try
            {
                _logger.LogInformation("Deleting question Id: {QuestionId}", id);
                var result = await _quizRepository.DeleteQuizQuestion(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question Id: {QuestionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Options =================
        [HttpGet("questions/{questionId:int}/options")]
        public async Task<IActionResult> GetQuizOptions(int questionId)
        {
            if (questionId <= 0)
                return BadRequest(new { Message = "Valid Question ID is required." });

            try
            {
                _logger.LogInformation("Fetching options for QuestionId: {QuestionId}", questionId);
                var options = await _quizRepository.GetQuizOptions(questionId);
                return Ok(options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching options for QuestionId: {QuestionId}", questionId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("options")]
        public async Task<IActionResult> CreateQuizOption([FromBody] CreateQuizOptionRequest request)
        {
            if (request == null || request.QuestionId <= 0 || string.IsNullOrWhiteSpace(request.OptionText))
                return BadRequest(new { Message = "Question ID and Option Text are required." });

            try
            {
                _logger.LogInformation("Creating option for QuestionId: {QuestionId}", request.QuestionId);
                var result = await _quizRepository.CreateQuizOption(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating option for QuestionId: {QuestionId}", request.QuestionId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("options/{id:int}")]
        public async Task<IActionResult> UpdateQuizOption(int id, [FromBody] UpdateQuizOptionRequest request)
        {
            if (id <= 0 || request == null || request.OptionId != id || string.IsNullOrWhiteSpace(request.OptionText))
                return BadRequest(new { Message = "Valid Option ID and Option Text are required." });

            try
            {
                _logger.LogInformation("Updating option Id: {OptionId}", id);
                var result = await _quizRepository.UpdateQuizOption(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating option Id: {OptionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("options/{id:int}")]
        public async Task<IActionResult> DeleteQuizOption(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Option ID is required." });

            try
            {
                _logger.LogInformation("Deleting option Id: {OptionId}", id);
                var result = await _quizRepository.DeleteQuizOption(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting option Id: {OptionId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Attempts =================
        [Authorize]
        [HttpPost("attempts/submit")]
        public async Task<IActionResult> SubmitQuizAttempt([FromBody] SubmitQuizAttemptRequest request)
        {
            if (request == null || request.QuizId <= 0 || request.StudentId <= 0)
                return BadRequest(new { Message = "Valid Quiz ID and Student ID are required." });

            try
            {
                _logger.LogInformation("Submitting quiz attempt for QuizId: {QuizId}, StudentId: {StudentId}", request.QuizId, request.StudentId);
                var result = await _quizRepository.SubmitQuizAttempt(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting quiz attempt");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("{quizId:int}/attempts")]
        public async Task<IActionResult> GetQuizAttempts(int quizId, [FromQuery] int? studentId)
        {
            if (quizId <= 0)
                return BadRequest(new { Message = "Valid Quiz ID is required." });

            try
            {
                _logger.LogInformation("Fetching attempts for QuizId: {QuizId}", quizId);
                var attempts = await _quizRepository.GetQuizAttempts(quizId, studentId);
                return Ok(attempts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attempts for QuizId: {QuizId}", quizId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("attempts/{id:int}")]
        public async Task<IActionResult> GetQuizAttemptById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Attempt ID is required." });

            try
            {
                _logger.LogInformation("Fetching attempt by Id: {AttemptId}", id);
                var attempt = await _quizRepository.GetQuizAttemptById(id);
                if (attempt == null)
                    return NotFound(new { Message = "Quiz attempt not found." });

                return Ok(attempt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attempt by Id: {AttemptId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
