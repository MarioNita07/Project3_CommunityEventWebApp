using CommunityEvents.Models;

namespace CommunityEvents.Services.Interfaces
{
    public interface IEventService
    {
            IEnumerable<Event> GetUpcomingEvents();
            IEnumerable<Event> GetMyEvents(string organizerId);
            Event? GetEventById(int id);
            Task CreateEvent(Event eventModel);
            Task DeleteEvent(int id, string userId);
            void UpdateEvent(Event eventModel);
            string? RegisterUserForEvent(int eventId, string userId);
            IEnumerable<Event> SearchEvents(string? searchTerm, string? category, string? location, DateTime? date);
            void LeaveEvent(int eventId, string userId);
            IEnumerable<Registration> GetMyRegistrations(string userId);

    }
}
