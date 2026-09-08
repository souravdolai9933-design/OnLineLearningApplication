using System;

namespace OnlineLearning.Core.Entities
{
    // ================= Enrollment =================
    public class EnrollStudentRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; } = "Online";
        public string? TransactionId { get; set; }
    }

    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public string? StudentName { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; } = "Active";
        public decimal ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    // ================= Wishlist =================
    public class WishlistRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }

    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? Thumbnail { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? InstructorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ================= Cart =================
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public decimal Price { get; set; }
    }

    public class CartItemDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string? Thumbnail { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime AddedAt { get; set; }
    }

    public class RemoveFromCartRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }

    // ================= Coupons =================
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public string CouponName { get; set; } = string.Empty;
        public string DiscountType { get; set; } = "Percentage";
        public decimal DiscountValue { get; set; }
        public decimal MinPurchaseAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCouponRequest
    {
        public string CouponCode { get; set; } = string.Empty;
        public string CouponName { get; set; } = string.Empty;
        public string DiscountType { get; set; } = "Percentage";
        public decimal DiscountValue { get; set; }
        public decimal MinPurchaseAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsageLimit { get; set; }
        public int CreatedBy { get; set; }
    }

    // ================= Reviews =================
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentProfileImage { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateReviewRequest
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
    }

    public class UpdateReviewRequest
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
    }
}
