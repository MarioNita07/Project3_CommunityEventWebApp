using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CommunityEvents.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(CommunityEventContext communityEventContext)
            : base(communityEventContext)
        {
        }
        public IEnumerable<Notification> GetUserNotifications(string userId)
        {
            return FindByCondition(n => n.UserId == userId)
                .OrderByDescending(n => n.DateCreated)
                .ToList();
        }
        public int GetUnreadCount(string userId)
        {
            return FindByCondition(n => n.UserId == userId && (n.IsRead == false || n.IsRead == null)).Count();
        }
        public NotificationPreference GetPreferences(string userId)
        {
            var pref = CommunityEventContext.NotificationPreferences.FirstOrDefault(p => p.UserId == userId);

            if (pref == null)
            {
                // Create default if missing
                pref = new NotificationPreference { UserId = userId };
                CommunityEventContext.NotificationPreferences.Add(pref);
                CommunityEventContext.SaveChanges();
            }
            return pref;
        }
        public void SavePreference(NotificationPreference preference)
        {
            if (preference.NotificationPreferenceId == 0)
            {
                CommunityEventContext.NotificationPreferences.Add(preference);
            }
            else
            {
                CommunityEventContext.NotificationPreferences.Update(preference);
            }
            CommunityEventContext.SaveChanges();
        }
        public IEnumerable<string> GetUserIdsForNewEventAlerts()
        {
            // get IDs of users who don't want alerts
            var optedOutUserIds = CommunityEventContext.NotificationPreferences
                .Where(p => p.ReceiveNewEventAlerts == false)
                .Select(p => p.UserId);

            //Get all users who are not in the opted-out list
            return CommunityEventContext.Set<IdentityUser>()
                .Select(u => u.Id)
                .Where(userId => !optedOutUserIds.Contains(userId))
                .ToList();
        }
    }
}
