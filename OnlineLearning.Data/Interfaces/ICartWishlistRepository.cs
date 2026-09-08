using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface ICartWishlistRepository
    {
        // Wishlist
        Task<DbResult> AddWishlist(WishlistRequest request);
        Task<IEnumerable<WishlistDto>> GetWishlist(int userId);
        Task<DbResult> RemoveWishlist(int userId, int courseId);
        Task<DbResult> ClearWishlist(int userId);

        // Cart
        Task<DbResult> AddToCart(AddToCartRequest request);
        Task<IEnumerable<CartItemDto>> GetCart(int userId);
        Task<DbResult> RemoveFromCart(int userId, int courseId);
        Task<DbResult> ClearCart(int userId);

        // Coupons
        Task<IEnumerable<CouponDto>> GetCoupons();
        Task<DbResult> CreateCoupon(CreateCouponRequest request);
    }
}
