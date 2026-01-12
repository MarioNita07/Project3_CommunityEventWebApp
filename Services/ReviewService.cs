using CommunityEvents.Services.Interfaces;
using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly INotificationService _notificationService;

        public ReviewService(IRepositoryWrapper repositoryWrapper, INotificationService notificationService)
        {
            _repositoryWrapper = repositoryWrapper;
            _notificationService = notificationService;
        }
        public bool AddReview(Review review)
        {
            if (_repositoryWrapper.ReviewRepository.HasUserReviewed(review.EventId, review.UserId))
            {
                return false; // User has already reviewed this event
            }

            _repositoryWrapper.ReviewRepository.Create(review);
            _repositoryWrapper.Save();

            // Get event to find organizer
            var evt = _repositoryWrapper.EventRepository.GetEventByIdWithDetails(review.EventId);

            // Notification
            if(evt != null)
            {
                _notificationService.NotifyOrganizerOfComment(evt.EventId, evt.Title, evt.OrganizerId);
            }

            return true;
        }
        public void DeleteReview(int reviewId, string currentUserId, bool isAdmin)
        {
            var review = _repositoryWrapper.ReviewRepository.FindByCondition(r => r.ReviewId == reviewId)
                .Include(r => r.Event)
                .FirstOrDefault();

            if (review != null)
            {
                // Allow delete only if user is Organizer or Admin
                if (isAdmin || review.Event.OrganizerId == currentUserId)
                { 
                _repositoryWrapper.ReviewRepository.Delete(review);
                _repositoryWrapper.Save();
                }
            }
        }
    }
}
