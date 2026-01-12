using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;
using CommunityEvents.Services.Interfaces;

namespace CommunityEvents.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public NotificationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public void NotifyAllUsersOfNewEvent(int eventId, string eventTitle)
        {
            // Get all users who want this alert
            var userIds = _repositoryWrapper.NotificationRepository.GetUserIdsForNewEventAlerts();

            // Create notifications
            foreach( var uid in userIds)
            {
                var notif = new Notification
                {
                    UserId = uid,
                    RelatedEventId = eventId,
                    Message = $"New Event Created: {eventTitle}",
                    DateCreated = DateTime.Now,
                    IsRead = false
                };
                _repositoryWrapper.NotificationRepository.Create(notif);
            }
            _repositoryWrapper.Save();
        }
        public void NotifyParticipantsOfUpdate(int eventId, string eventTitle)
        {
            // Get all participants
            var registrations = _repositoryWrapper.RegistrationRepository
                .FindByCondition(r => r.EventId == eventId)
                .ToList();

            foreach(var reg in registrations)
            {
                // Check if they want alerts
                var prefs = _repositoryWrapper.NotificationRepository.GetPreferences(reg.UserId);
                if (prefs.ReceiveUpdateAlerts)
                {
                    var notif = new Notification
                    {
                        UserId = reg.UserId,
                        RelatedEventId = eventId,
                        Message = $"Update: Details for '{eventTitle}' have changed.",
                        DateCreated = DateTime.Now,
                        IsRead = false
                    };
                    _repositoryWrapper.NotificationRepository.Create(notif);
                }
            }
            _repositoryWrapper.Save();
        }
        public void NotifyOrganizerOfComment(int eventId, string eventTitle, string organizerId)
        {
            var prefs = _repositoryWrapper.NotificationRepository.GetPreferences(organizerId);
            if (prefs.ReceiveCommentAlerts)
            {
                var notif = new Notification
                {
                    UserId = organizerId,
                    RelatedEventId = eventId,
                    Message = $"New review on your event: {eventTitle}",
                    DateCreated = DateTime.Now,
                    IsRead = false
                };
                _repositoryWrapper.NotificationRepository.Create(notif);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Notification> GetMyNotifications(string userId)
        {
            return _repositoryWrapper.NotificationRepository.GetUserNotifications(userId);
        }
        public int GetMyUnreadCount(string userId)
        {
            return _repositoryWrapper.NotificationRepository.GetUnreadCount(userId);
        }
        public void MarkAsRead(int notificationId)
        {
            var notif = _repositoryWrapper.NotificationRepository.FindByCondition(n => n.NotificationId == notificationId).FirstOrDefault();
            if (notif != null)
            {
                notif.IsRead = true;
                _repositoryWrapper.NotificationRepository.Update(notif);
                _repositoryWrapper.Save();
            }
        }
        public void DeleteNotification(int notificationId, string userId)
        {
            var notif = _repositoryWrapper.NotificationRepository.FindByCondition(n => n.NotificationId == notificationId && n.UserId == userId).FirstOrDefault();
            if (notif != null)
            {
                _repositoryWrapper.NotificationRepository.Delete(notif);
                _repositoryWrapper.Save();
            }
        }
        public NotificationPreference GetMyPreferences(string userId)
        {
            return _repositoryWrapper.NotificationRepository.GetPreferences(userId);
        }
        public void UpdatePreferences(NotificationPreference pref)
        {
            _repositoryWrapper.NotificationRepository.SavePreference(pref);
        }
    }
}
