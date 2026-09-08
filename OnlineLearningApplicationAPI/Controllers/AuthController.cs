using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineLearning.Core.Entities;
using OnlineLearning.Data.Interfaces;
using OnlineLearning.Utilites.Helper;
using System;
using System.Threading.Tasks;

namespace OnlineLearningApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepository, JwtHelper jwtHelper, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Registration details are required." });

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Email and Password are required." });

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                return BadRequest(new { Message = "First Name and Last Name are required." });

            try
            {
                _logger.LogInformation("Registering new user with email: {Email}", request.Email);

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var result = await _authRepository.RegisterUser(request, passwordHash);

                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for email: {Email}", request.Email);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Email and Password are required." });

            try
            {
                _logger.LogInformation("Attempting login for email: {Email}", request.Email);

                var user = await _authRepository.LoginUser(request);
                if (user.Result == 0 || string.IsNullOrEmpty(user.PasswordHash))
                    return BadRequest(new { Message = user.Message ?? "Invalid email or password." });

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (!isPasswordValid)
                    return BadRequest(new { Message = "Invalid email or password." });

                string token = _jwtHelper.GenerateToken(user);
                string refreshToken = _jwtHelper.GenerateRefreshToken();
                var refreshExpiry = DateTime.UtcNow.AddDays(7);

                if (user.UserId.HasValue)
                {
                    await _authRepository.SaveRefreshToken(user.UserId.Value, refreshToken, refreshExpiry);
                }

                user.PasswordHash = null; // Do not return hash
                user.Token = token;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshExpiry;

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for email: {Email}", request.Email);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("profile/{userId:int}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Fetching user profile for UserId: {UserId}", userId);

                var profile = await _authRepository.GetUserById(userId);
                if (profile == null)
                    return NotFound(new { Message = "User profile not found." });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user profile for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest(new { Message = "Valid profile data is required." });

            try
            {
                _logger.LogInformation("Updating user profile for UserId: {UserId}", request.UserId);

                var result = await _authRepository.UpdateUserProfile(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile for UserId: {UserId}", request.UserId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || request.UserId <= 0 || string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest(new { Message = "UserId, Old Password, and New Password are required." });

            try
            {
                _logger.LogInformation("Changing password for UserId: {UserId}", request.UserId);

                var profile = await _authRepository.GetUserById(request.UserId);
                if (profile == null)
                    return NotFound(new { Message = "User not found." });

                string oldHash = BCrypt.Net.BCrypt.HashPassword(request.OldPassword);
                string newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                var result = await _authRepository.ChangePassword(request.UserId, oldHash, newHash);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for UserId: {UserId}", request.UserId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { Message = "Email is required." });

            try
            {
                _logger.LogInformation("Generating forgot password token for email: {Email}", request.Email);

                string token = Guid.NewGuid().ToString("N");
                var expiry = DateTime.UtcNow.AddHours(2);

                var result = await _authRepository.ForgotPassword(request.Email, token, expiry);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(new { Message = "Reset token generated successfully.", Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating forgot password token for email: {Email}", request.Email);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest(new { Message = "Email, Token, and New Password are required." });

            try
            {
                _logger.LogInformation("Resetting password for email: {Email}", request.Email);

                string newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                var result = await _authRepository.ResetPassword(request.Email, request.Token, newHash);

                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", request.Email);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Roles =================
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                _logger.LogInformation("Fetching all user roles");
                var roles = await _authRepository.GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user roles");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("roles/{roleId:int}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            if (roleId <= 0)
                return BadRequest(new { Message = "Valid Role ID is required." });

            try
            {
                _logger.LogInformation("Fetching role by Id: {RoleId}", roleId);
                var role = await _authRepository.GetRoleById(roleId);
                if (role == null)
                    return NotFound(new { Message = "Role not found." });

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role by Id: {RoleId}", roleId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RoleName))
                return BadRequest(new { Message = "Role Name is required." });

            try
            {
                _logger.LogInformation("Creating role: {RoleName}", request.RoleName);
                var result = await _authRepository.CreateRole(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role: {RoleName}", request.RoleName);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("roles/{roleId:int}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleRequest request)
        {
            if (roleId <= 0 || request == null || request.RoleId != roleId || string.IsNullOrWhiteSpace(request.RoleName))
                return BadRequest(new { Message = "Valid Role ID and Role Name are required." });

            try
            {
                _logger.LogInformation("Updating role Id: {RoleId}", roleId);
                var result = await _authRepository.UpdateRole(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role Id: {RoleId}", roleId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("roles/{roleId:int}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            if (roleId <= 0)
                return BadRequest(new { Message = "Valid Role ID is required." });

            try
            {
                _logger.LogInformation("Deleting role Id: {RoleId}", roleId);
                var result = await _authRepository.DeleteRole(roleId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role Id: {RoleId}", roleId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
