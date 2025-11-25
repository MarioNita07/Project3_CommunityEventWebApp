using CommunityEvents.Models;

namespace CommunityEvents.Repositories.Interfaces
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
            IEnumerable<Event> GetAllEventsWithDetails();
            Event? GetEventByIdWithDetails(int eventId);
            IEnumerable<Event> GetEventsByOrganizer(string organizerId);
    }
}
