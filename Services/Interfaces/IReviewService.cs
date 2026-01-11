using CommunityEvents.Models;
using Microsoft.CodeAnalysis;

namespace CommunityEvents.Services.Interfaces
{
    public interface IReviewService
    {
        bool AddReview(Review review);
        void DeleteReview(int reviewId, string currentUserId, bool isAdmin);
    }
}
