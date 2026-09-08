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
    public class BannersSupportController : ControllerBase
    {
        private readonly IBannerSupportRepository _bannerSupportRepository;
        private readonly ILogger<BannersSupportController> _logger;

        public BannersSupportController(IBannerSupportRepository bannerSupportRepository, ILogger<BannersSupportController> logger)
        {
            _bannerSupportRepository = bannerSupportRepository;
            _logger = logger;
        }

        // ================= Banners =================
        [HttpGet("banners")]
        public async Task<IActionResult> GetBanners()
        {
            try
            {
                _logger.LogInformation("Fetching active banners");
                var banners = await _bannerSupportRepository.GetBanners();
                return Ok(banners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching banners");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("banners/{id:int}")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Banner ID is required." });

            try
            {
                _logger.LogInformation("Fetching banner by Id: {BannerId}", id);
                var banner = await _bannerSupportRepository.GetBannerById(id);
                if (banner == null)
                    return NotFound(new { Message = "Banner not found." });

                return Ok(banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching banner by Id: {BannerId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("banners")]
        public async Task<IActionResult> CreateBanner([FromBody] CreateBannerRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.BannerImage))
                return BadRequest(new { Message = "Banner Title and Banner Image are required." });

            try
            {
                _logger.LogInformation("Creating banner: {Title}", request.Title);
                var result = await _bannerSupportRepository.CreateBanner(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating banner: {Title}", request.Title);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("banners/{id:int}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] UpdateBannerRequest request)
        {
            if (id <= 0 || request == null || request.BannerId != id || string.IsNullOrWhiteSpace(request.Title))
                return BadRequest(new { Message = "Valid Banner ID and Title are required." });

            try
            {
                _logger.LogInformation("Updating banner Id: {BannerId}", id);
                var result = await _bannerSupportRepository.UpdateBanner(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating banner Id: {BannerId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("banners/{id:int}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Banner ID is required." });

            try
            {
                _logger.LogInformation("Deleting banner Id: {BannerId}", id);
                var result = await _bannerSupportRepository.DeleteBanner(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting banner Id: {BannerId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Support Tickets =================
        [Authorize]
        [HttpPost("support/tickets")]
        public async Task<IActionResult> CreateSupportTicket([FromBody] CreateSupportTicketRequest request)
        {
            if (request == null || request.UserId <= 0 || string.IsNullOrWhiteSpace(request.Subject) || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Message = "Valid User ID, Subject, and Message are required." });

            try
            {
                _logger.LogInformation("Creating support ticket for UserId: {UserId}", request.UserId);
                var result = await _bannerSupportRepository.CreateSupportTicket(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating support ticket");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("support/tickets")]
        public async Task<IActionResult> GetSupportTickets([FromQuery] int? userId, [FromQuery] string? status)
        {
            try
            {
                _logger.LogInformation("Fetching support tickets (UserId: {UserId}, Status: {Status})", userId, status);
                var tickets = await _bannerSupportRepository.GetSupportTickets(userId, status);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching support tickets");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("support/tickets/{id:int}")]
        public async Task<IActionResult> GetSupportTicketById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Ticket ID is required." });

            try
            {
                _logger.LogInformation("Fetching support ticket by Id: {TicketId}", id);
                var ticket = await _bannerSupportRepository.GetSupportTicketById(id);
                if (ticket == null)
                    return NotFound(new { Message = "Support ticket not found." });

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching support ticket by Id: {TicketId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("support/tickets/{id:int}")]
        public async Task<IActionResult> UpdateSupportTicket(int id, [FromBody] UpdateSupportTicketRequest request)
        {
            if (id <= 0 || request == null || request.TicketId != id)
                return BadRequest(new { Message = "Valid Ticket ID is required." });

            try
            {
                _logger.LogInformation("Updating support ticket Id: {TicketId}", id);
                var result = await _bannerSupportRepository.UpdateSupportTicket(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating support ticket Id: {TicketId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPut("support/tickets/{id:int}/close")]
        public async Task<IActionResult> CloseSupportTicket(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Ticket ID is required." });

            try
            {
                _logger.LogInformation("Closing support ticket Id: {TicketId}", id);
                var result = await _bannerSupportRepository.CloseSupportTicket(id);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing support ticket Id: {TicketId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPost("support/tickets/reply")]
        public async Task<IActionResult> CreateTicketReply([FromBody] CreateTicketReplyRequest request)
        {
            if (request == null || request.TicketId <= 0 || request.SenderId <= 0 || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Message = "Valid Ticket ID, Sender ID, and Message are required." });

            try
            {
                _logger.LogInformation("Replying to ticket Id: {TicketId} by Sender: {SenderId}", request.TicketId, request.SenderId);
                var result = await _bannerSupportRepository.CreateTicketReply(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket reply");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("support/tickets/{id:int}/replies")]
        public async Task<IActionResult> GetTicketReplies(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Valid Ticket ID is required." });

            try
            {
                _logger.LogInformation("Fetching replies for ticket Id: {TicketId}", id);
                var replies = await _bannerSupportRepository.GetTicketReplies(id);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ticket replies for TicketId: {TicketId}", id);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
