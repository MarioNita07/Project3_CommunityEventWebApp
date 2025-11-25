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
        }
    }
