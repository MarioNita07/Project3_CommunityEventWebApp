using CommunityEvents.Models;

namespace CommunityEvents.Services.Interfaces
{
    public interface INotificationService
    {
        // Triggers
        void NotifyAllUsersOfNewEvent(int eventId, string eventTitle);
        void NotifyParticipantsOfUpdate(int eventId, string eventTitle);
        void NotifyOrganizerOfComment(int eventId, string eventTitle, string organizerId);

        // Actions
        IEnumerable<Notification> GetMyNotifications(string userId);
        int GetMyUnreadCount(string userId);
        void MarkAsRead(int notificationId);
        void DeleteNotification(int notificationId, string userId);

        // Settings
        NotificationPreference GetMyPreferences(string userId);
        void UpdatePreferences(NotificationPreference pref);
    }
}
