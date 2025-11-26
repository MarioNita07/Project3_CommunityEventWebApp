using CommunityEvents.Models;

namespace CommunityEvents.Repositories.Interfaces
{
    public interface IRegistrationRepository : IRepositoryBase<Registration>
    {
            bool IsUserRegistered(int eventId, string userId);
            int GetRegistrationCount(int eventId);
            IEnumerable<Registration> GetRegistrationsByUser(string userId);
            void DeleteRegistration(int eventId, string userId);

    }
}
