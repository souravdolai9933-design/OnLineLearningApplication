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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminRepository adminRepository, ILogger<AdminController> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        // ================= Payouts =================
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost("payouts/request")]
        public async Task<IActionResult> RequestPayout([FromBody] RequestPayoutRequest request)
        {
            if (request == null || request.InstructorId <= 0 || request.Amount <= 0 || string.IsNullOrWhiteSpace(request.PaymentMethod))
                return BadRequest(new { Message = "Valid Instructor ID, Amount, and Payment Method are required." });

            try
            {
                _logger.LogInformation("Requesting payout for Instructor: {InstructorId}, Amount: {Amount}", request.InstructorId, request.Amount);
                var result = await _adminRepository.RequestPayout(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting payout for InstructorId: {InstructorId}", request.InstructorId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpGet("payouts")]
        public async Task<IActionResult> GetPayouts([FromQuery] int? instructorId, [FromQuery] string? status)
        {
            try
            {
                _logger.LogInformation("Fetching payouts (InstructorId: {InstructorId}, Status: {Status})", instructorId, status);
                var payouts = await _adminRepository.GetPayouts(instructorId, status);
                return Ok(payouts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payouts");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpGet("payouts/{id:int}")]
        public async Task<IActionResult> GetPayoutById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Payout ID is required." });

            try
            {
                _logger.LogInformation("Fetching payout by Id: {PayoutId}", id);
                var payout = await _adminRepository.GetPayoutById(id);
                if (payout == null)
                    return NotFound(new { Message = "Payout not found." });

                return Ok(payout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payout by Id: {PayoutId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("payouts/{id:int}/approve")]
        public async Task<IActionResult> ApprovePayout(int id, [FromBody] PayoutActionRequest request)
        {
            if (id <= 0 || request == null || request.PayoutId != id || request.AdminId <= 0)
                return BadRequest(new { Message = "Valid Payout ID and Admin ID are required." });

            try
            {
                _logger.LogInformation("Approving payout Id: {PayoutId} by Admin: {AdminId}", id, request.AdminId);
                var result = await _adminRepository.ApprovePayout(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving payout Id: {PayoutId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("payouts/{id:int}/reject")]
        public async Task<IActionResult> RejectPayout(int id, [FromBody] PayoutActionRequest request)
        {
            if (id <= 0 || request == null || request.PayoutId != id || request.AdminId <= 0)
                return BadRequest(new { Message = "Valid Payout ID and Admin ID are required." });

            try
            {
                _logger.LogInformation("Rejecting payout Id: {PayoutId} by Admin: {AdminId}", id, request.AdminId);
                var result = await _adminRepository.RejectPayout(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting payout Id: {PayoutId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Settings =================
        [HttpGet("settings")]
        public async Task<IActionResult> GetSiteSettings()
        {
            try
            {
                _logger.LogInformation("Fetching site settings");
                var settings = await _adminRepository.GetSiteSettings();
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching site settings");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("settings/{key}")]
        public async Task<IActionResult> GetSiteSettingByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest(new { Message = "Setting key is required." });

            try
            {
                _logger.LogInformation("Fetching site setting by key: {Key}", key);
                var setting = await _adminRepository.GetSiteSettingByKey(key);
                if (setting == null)
                    return NotFound(new { Message = "Setting not found." });

                return Ok(setting);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching setting for key: {Key}", key);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("settings/{key}")]
        public async Task<IActionResult> UpdateSiteSetting(string key, [FromBody] UpdateSiteSettingRequest request)
        {
            if (string.IsNullOrWhiteSpace(key) || request == null)
                return BadRequest(new { Message = "Setting key and value are required." });

            request.SettingKey = key;

            try
            {
                _logger.LogInformation("Updating site setting for key: {Key}", key);
                var result = await _adminRepository.UpdateSiteSetting(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating site setting for key: {Key}", key);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Audit Logs =================
        [Authorize(Roles = "Admin")]
        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] int? userId,
            [FromQuery] string? tableName,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            try
            {
                _logger.LogInformation("Fetching audit logs");
                var logs = await _adminRepository.GetAuditLogs(userId, tableName, fromDate, toDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching audit logs");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Dashboards =================
        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard/admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                _logger.LogInformation("Fetching Admin dashboard metrics");
                var dashboard = await _adminRepository.GetAdminDashboard();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching admin dashboard metrics");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpGet("dashboard/instructor/{instructorId:int}")]
        public async Task<IActionResult> GetInstructorDashboard(int instructorId)
        {
            if (instructorId <= 0)
                return BadRequest(new { Message = "Valid Instructor ID is required." });

            try
            {
                _logger.LogInformation("Fetching Instructor dashboard metrics for InstructorId: {InstructorId}", instructorId);
                var dashboard = await _adminRepository.GetInstructorDashboard(instructorId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching instructor dashboard metrics for InstructorId: {InstructorId}", instructorId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
