using CommunityEvents.Models;

namespace CommunityEvents.Services.Interfaces
{
    public interface IEventService
    {
            IEnumerable<Event> GetUpcomingEvents();
            IEnumerable<Event> GetMyEvents(string organizerId);
            Event? GetEventById(int id);
            void CreateEvent(Event eventModel);
            void DeleteEvent(int id);
            string? RegisterUserForEvent(int eventId, string userId);
    }
}
