using CommunityEvents.Models;
using CommunityEvents.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Repositories
{
    public class RegistrationRepository : RepositoryBase<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(CommunityEventContext communityEventContext)
           : base(communityEventContext)
        {
        }

        public bool IsUserRegistered(int eventId, string userId)
        {
            return FindByCondition(r => r.EventId == eventId && r.UserId == userId).Any();
        }

        public int GetRegistrationCount(int eventId)
        {
            return FindByCondition(r => r.EventId == eventId).Count();
        }

        public IEnumerable<Registration> GetRegistrationsByUser(string userId)
        {
            return FindByCondition(r => r.UserId == userId)
                .Include(r =>r.Event)
                .ToList();

        }

        public void DeleteRegistration(int eventId, string userId)
        {
            var registration = FindByCondition(r => r.EventId == eventId && r.UserId == userId).FirstOrDefault();
            if (registration != null)
            {
                Delete(registration);
            }
        }
    }
}
