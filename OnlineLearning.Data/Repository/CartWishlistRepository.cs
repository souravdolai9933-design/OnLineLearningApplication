using Dapper;
using Microsoft.Extensions.Logging;
using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using OnlineLearning.Data.Interfaces;
using OnlineLearning.Utilites.Connections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Repository
{
    public class CartWishlistRepository : ICartWishlistRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<CartWishlistRepository> _logger;

        public CartWishlistRepository(DbConnectionFactory dbConnectionFactory, ILogger<CartWishlistRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        // ================= Wishlist =================
        public async Task<DbResult> AddWishlist(WishlistRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@CourseId", request.CourseId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_AddWishlist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to add to wishlist." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to wishlist for UserId: {UserId}, CourseId: {CourseId}", request.UserId, request.CourseId);
                throw;
            }
        }

        public async Task<IEnumerable<WishlistDto>> GetWishlist(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return await connection.QueryAsync<WishlistDto>(
                    "sp_GetWishlist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wishlist for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> RemoveWishlist(int userId, int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@CourseId", courseId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RemoveWishlist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to remove from wishlist." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from wishlist for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                throw;
            }
        }

        public async Task<DbResult> ClearWishlist(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ClearWishlist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to clear wishlist." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist for UserId: {UserId}", userId);
                throw;
            }
        }

        // ================= Cart =================
        public async Task<DbResult> AddToCart(AddToCartRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@CourseId", request.CourseId);
                parameters.Add("@Price", request.Price);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_AddToCart",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to add to cart." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to cart for UserId: {UserId}, CourseId: {CourseId}", request.UserId, request.CourseId);
                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetCart(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return await connection.QueryAsync<CartItemDto>(
                    "sp_UpdateCartTotals",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<DbResult> RemoveFromCart(int userId, int courseId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@CourseId", courseId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_RemoveFromCart",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to remove from cart." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from cart for UserId: {UserId}, CourseId: {CourseId}", userId, courseId);
                throw;
            }
        }

        public async Task<DbResult> ClearCart(int userId)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_ClearCart",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to clear cart." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for UserId: {UserId}", userId);
                throw;
            }
        }

        // ================= Coupons =================
        public async Task<IEnumerable<CouponDto>> GetCoupons()
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<CouponDto>(
                    "sp_GetCoupons",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching coupons");
                throw;
            }
        }

        public async Task<DbResult> CreateCoupon(CreateCouponRequest request)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@CouponCode", request.CouponCode);
                parameters.Add("@CouponName", request.CouponName);
                parameters.Add("@DiscountType", request.DiscountType);
                parameters.Add("@DiscountValue", request.DiscountValue);
                parameters.Add("@MinPurchaseAmount", request.MinPurchaseAmount);
                parameters.Add("@MaxDiscountAmount", request.MaxDiscountAmount);
                parameters.Add("@StartDate", request.StartDate);
                parameters.Add("@ExpiryDate", request.ExpiryDate);
                parameters.Add("@UsageLimit", request.UsageLimit);
                parameters.Add("@CreatedBy", request.CreatedBy);

                var result = await connection.QueryFirstOrDefaultAsync<DbResult>(
                    "sp_CreateCoupon",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result ?? new DbResult { Result = 0, Message = "Failed to create coupon." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating coupon: {CouponCode}", request.CouponCode);
                throw;
            }
        }
    }
}
