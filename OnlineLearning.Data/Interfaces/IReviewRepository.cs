using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<ReviewDto>> GetCourseReviews(int courseId);
        Task<ReviewDto?> GetReviewById(int reviewId);
        Task<DbResult> CreateReview(CreateReviewRequest request);
        Task<DbResult> UpdateReview(UpdateReviewRequest request);
        Task<DbResult> DeleteReview(int reviewId, int userId);
    }
}
