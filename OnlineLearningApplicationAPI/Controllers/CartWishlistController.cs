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
    public class CartWishlistController : ControllerBase
    {
        private readonly ICartWishlistRepository _cartWishlistRepository;
        private readonly ILogger<CartWishlistController> _logger;

        public CartWishlistController(ICartWishlistRepository cartWishlistRepository, ILogger<CartWishlistController> logger)
        {
            _cartWishlistRepository = cartWishlistRepository;
            _logger = logger;
        }

        // ================= Wishlist =================
        [HttpPost("wishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] WishlistRequest request)
        {
            if (request == null || request.UserId <= 0 || request.CourseId <= 0)
                return BadRequest(new { Message = "Valid User ID and Course ID are required." });

            try
            {
                _logger.LogInformation("Adding course: {CourseId} to wishlist for User: {UserId}", request.CourseId, request.UserId);
                var result = await _cartWishlistRepository.AddWishlist(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to wishlist");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("wishlist/{userId:int}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Fetching wishlist for UserId: {UserId}", userId);
                var wishlist = await _cartWishlistRepository.GetWishlist(userId);
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wishlist for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpDelete("wishlist/{userId:int}/{courseId:int}")]
        public async Task<IActionResult> RemoveWishlist(int userId, int courseId)
        {
            if (userId <= 0 || courseId <= 0)
                return BadRequest(new { Message = "Valid User ID and Course ID are required." });

            try
            {
                _logger.LogInformation("Removing course: {CourseId} from wishlist for User: {UserId}", courseId, userId);
                var result = await _cartWishlistRepository.RemoveWishlist(userId, courseId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from wishlist");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpDelete("wishlist/clear/{userId:int}")]
        public async Task<IActionResult> ClearWishlist(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Clearing wishlist for UserId: {UserId}", userId);
                var result = await _cartWishlistRepository.ClearWishlist(userId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Cart =================
        [HttpPost("cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null || request.UserId <= 0 || request.CourseId <= 0)
                return BadRequest(new { Message = "Valid User ID and Course ID are required." });

            try
            {
                _logger.LogInformation("Adding course: {CourseId} to cart for User: {UserId}", request.CourseId, request.UserId);
                var result = await _cartWishlistRepository.AddToCart(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to cart");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpGet("cart/{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Fetching cart for UserId: {UserId}", userId);
                var cart = await _cartWishlistRepository.GetCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpDelete("cart/{userId:int}/{courseId:int}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int courseId)
        {
            if (userId <= 0 || courseId <= 0)
                return BadRequest(new { Message = "Valid User ID and Course ID are required." });

            try
            {
                _logger.LogInformation("Removing course: {CourseId} from cart for User: {UserId}", courseId, userId);
                var result = await _cartWishlistRepository.RemoveFromCart(userId, courseId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from cart");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [HttpDelete("cart/clear/{userId:int}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "Valid User ID is required." });

            try
            {
                _logger.LogInformation("Clearing cart for UserId: {UserId}", userId);
                var result = await _cartWishlistRepository.ClearCart(userId);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for UserId: {UserId}", userId);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        // ================= Coupons =================
        [AllowAnonymous]
        [HttpGet("coupons")]
        public async Task<IActionResult> GetCoupons()
        {
            try
            {
                _logger.LogInformation("Fetching active coupons");
                var coupons = await _cartWishlistRepository.GetCoupons();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching coupons");
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("coupons")]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CouponCode) || string.IsNullOrWhiteSpace(request.CouponName))
                return BadRequest(new { Message = "Coupon Code and Name are required." });

            try
            {
                _logger.LogInformation("Creating coupon: {CouponCode}", request.CouponCode);
                var result = await _cartWishlistRepository.CreateCoupon(request);
                if (result.Result == 0)
                    return BadRequest(new { Message = result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating coupon: {CouponCode}", request.CouponCode);
                return StatusCode(500, new { Message = "An internal server error occurred." });
            }
        }
    }
}
