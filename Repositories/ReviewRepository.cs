using Microsoft.EntityFrameworkCore;
using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;
namespace CommunityEvents.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(CommunityEventContext communityEventContext) : base(communityEventContext)
        {
        }
        public IEnumerable<Review> GetReviewsByEvent(int eventId)
        {
            return FindByCondition(r => r.EventId == eventId)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewId)
                .ToList();
        }
        public bool HasUserReviewed(int eventId, string userId)
        {
            return FindByCondition(r => r.EventId == eventId && r.UserId == userId).Any();
        }
    }
}
