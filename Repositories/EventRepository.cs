using CommunityEvents.Models;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Repositories.Interfaces;

namespace CommunityEvents.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(CommunityEventContext communityEventContext)
            : base(communityEventContext)
        {
        }

        public IEnumerable<Event> GetAllEventsWithDetails()
        {
            return FindAll()
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .OrderBy(e => e.Date)
                .ToList();
        }

        public Event? GetEventByIdWithDetails(int eventId)
        {
            return FindByCondition(e => e.EventId == eventId)
                .Include(e => e.Organizer)
                .Include(e => e.Registrations).ThenInclude(r => r.User)
                .Include(e => e.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault();
        }

        public IEnumerable<Event> GetEventsByOrganizer(string organizerId)
        {
            return FindByCondition(e => e.OrganizerId == organizerId)
                .Include(e => e.Registrations)
                .ToList();
        }

        public IEnumerable<Event> GetEventsByConditions(string? searchTerm, string? category, string? location, DateTime? date)
        {
            var query = FindAll()
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .AsQueryable();

            //  Filter by keyword
            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lowerTerm = searchTerm.ToLower();
                query = query.Where(e =>
                    e.Title.ToLower().Contains(lowerTerm) ||
                    (e.Description.ToLower().Contains(lowerTerm)));
            }

            //  Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.CategoryName == category);
            }

            //  Filter by location
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.ToLower().Contains(location.ToLower()));
            }

            //  Filter by date
            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date >= date.Value.Date);
            }
            else
            {
                // Default to upcoming events if no date is provided
                query = query.Where(e => e.Date >= DateTime.Now);
            }
            return query.OrderBy(e => e.Date).ToList();

        }
    }
}

