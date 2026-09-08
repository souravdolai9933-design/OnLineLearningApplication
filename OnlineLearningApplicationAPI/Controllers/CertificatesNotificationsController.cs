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
    public class CertificatesNotificationsController : ControllerBase
    {
        private readonly ICertificateNotificationRepository _certNotificationRepository;
        private readonly ILogger<CertificatesNotificationsController> _logger;

        public CertificatesNotificationsController(ICertificateNotificationRepository certNotificationRepository, ILogger<CertificatesNotificationsController> logger)
        {
            _certNotificationRepository = certNotificationRepository;
            _logger = logger;
        }

        // ================= Certificates =================
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("certificates/issue")]
        public async Task<IActionResult> IssueCertificate([FromBody] IssueCertificateRequest request)
        {
            if (request == null || request.EnrollmentId <= 0 || request.StudentId <= 0 || request.CourseId <= 0)
                return BadRequest(new { Message = "Valid Enrollment ID, Student ID, and Course ID are required." });

            try
            {
                _logger.LogInformation("Issuing certificate for EnrollmentId: {EnrollmentId}, StudentId: {StudentId}", request.EnrollmentId, request.StudentId);
                var result = await _certNotificationRepository.IssueCertificate(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error issuing certificate");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("certificates/student/{studentId:int}")]
        public async Task<IActionResult> GetCertificates(int studentId)
        {
            if (studentId <= 0)
                return BadRequest(new { Message = "Valid Student ID is required." });

            try
            {
                _logger.LogInformation("Fetching certificates for StudentId: {StudentId}", studentId);
                var certs = await _certNotificationRepository.GetCertificates(studentId);
                return Ok(certs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificates for StudentId: {StudentId}", studentId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("certificates/{id:int}")]
        public async Task<IActionResult> GetCertificateById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Certificate ID is required." });

            try
            {
                _logger.LogInformation("Fetching certificate by Id: {CertificateId}", id);
                var cert = await _certNotificationRepository.GetCertificateById(id);
                if (cert == null)
                    return NotFound(new { Message = "Certificate not found." });

                return Ok(cert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching certificate by Id: {CertificateId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("certificates/verify/{code}")]
        public async Task<IActionResult> VerifyCertificate(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new { Message = "Certificate code is required." });

            try
            {
                _logger.LogInformation("Verifying certificate with code: {Code}", code);
                var cert = await _certNotificationRepository.VerifyCertificate(code);
                if (cert == null)
                    return NotFound(new { Message = "Certificate is invalid or does not exist." });

                return Ok(cert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying certificate with code: {Code}", code);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("certificates/{id:int}/revoke")]
        public async Task<IActionResult> RevokeCertificate(int id, [FromBody] RevokeCertificateRequest request)
        {
            if (id <= 0 || request == null || string.IsNullOrWhiteSpace(request.RevokedReason))
                return BadRequest(new { Message = "Valid Certificate ID and Revocation Reason are required." });

            request.CertificateId = id;

            try
            {
                _logger.LogInformation("Revoking certificate Id: {CertificateId}", id);
                var result = await _certNotificationRepository.RevokeCertificate(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking certificate Id: {CertificateId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Notifications =================
        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost("notifications")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            if (request == null || request.UserId <= 0 || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Message = "User ID, Title, and Message are required." });

            try
            {
                _logger.LogInformation("Creating notification for UserId: {UserId}", request.UserId);
                var result = await _certNotificationRepository.CreateNotification(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("notifications/user/{userId:int}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Fetching notifications for UserId: {UserId}", userId);
                var notifications = await _certNotificationRepository.GetNotifications(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPut("notifications/{id:int}/read")]
        public async Task<IActionResult> MarkNotificationRead(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Notification ID is required." });

            try
            {
                _logger.LogInformation("Marking notification Id: {NotificationId} as read", id);
                var result = await _certNotificationRepository.MarkNotificationRead(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification Id: {NotificationId} read", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPut("notifications/read-all/{userId:int}")]
        public async Task<IActionResult> MarkAllNotificationsRead(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Marking all notifications as read for UserId: {UserId}", userId);
                var result = await _certNotificationRepository.MarkAllNotificationsRead(userId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications read for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpDelete("notifications/{id:int}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Notification ID is required." });

            try
            {
                _logger.LogInformation("Deleting notification Id: {NotificationId}", id);
                var result = await _certNotificationRepository.DeleteNotification(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification Id: {NotificationId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
