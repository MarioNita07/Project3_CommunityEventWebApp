using CommunityEvents.Models;

namespace CommunityEvents.Repositories.Interfaces
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        IEnumerable<Review> GetReviewsByEvent(int eventId);
        bool HasUserReviewed(int eventId, string userId);
    }
}
