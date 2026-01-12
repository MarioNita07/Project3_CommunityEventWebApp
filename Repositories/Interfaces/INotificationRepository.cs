using CommunityEvents.Models;

namespace CommunityEvents.Repositories.Interfaces
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        IEnumerable<Notification> GetUserNotifications(string userId);
        int GetUnreadCount(string userId);
        NotificationPreference GetPreferences(string userId);
        void SavePreference(NotificationPreference preference);

        // Get all users who want a specific alert type
        IEnumerable<string> GetUserIdsForNewEventAlerts();
    }
}
