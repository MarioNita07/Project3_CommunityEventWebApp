using CommunityEvents.Models;
using Microsoft.AspNetCore.Identity;
using CommunityEvents.Repositories.Interfaces;
using CommunityEvents.Services.Interfaces;
using System.Threading.Tasks;

namespace CommunityEvents.Services
{
    public class EventService : IEventService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EventService(IRepositoryWrapper repositoryWrapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IEnumerable<Event> GetUpcomingEvents()
        {
            var events = _repositoryWrapper.EventRepository.GetAllEventsWithDetails();
            return events.Where(e => e.Date >= DateTime.Now);
        }

        public IEnumerable<Event> GetMyEvents(string organizerId)
        {
            return _repositoryWrapper.EventRepository.GetEventsByOrganizer(organizerId);
        }

        public Event? GetEventById(int id)
        {
            return _repositoryWrapper.EventRepository.GetEventByIdWithDetails(id);
        }

        public async Task CreateEvent(Event eventModel)
        {
            _repositoryWrapper.EventRepository.Create(eventModel);
            _repositoryWrapper.Save();

            // If the user created an event, they are now an organizer.
            var user = await _userManager.FindByIdAsync(eventModel.OrganizerId);
            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, "Organizer"))
                {
                    await _userManager.AddToRoleAsync(user, "Organizer");
                    await _userManager.RemoveFromRoleAsync(user, "Participant");

                    await _signInManager.RefreshSignInAsync(user);
                }
            }
        }

        public async Task DeleteEvent(int id, string userId)
        {
            var eventToDelete = _repositoryWrapper.EventRepository.FindByCondition(e => e.EventId == id).FirstOrDefault();
            if (eventToDelete != null)
            {
                _repositoryWrapper.EventRepository.Delete(eventToDelete);
                _repositoryWrapper.Save();

                // Check if the user has any events left
                var remainingEvents = _repositoryWrapper.EventRepository.GetEventsByOrganizer(userId);

                if (!remainingEvents.Any())
                {
                    // If no events left, downgrade role to Participant
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Organizer");
                        await _userManager.AddToRoleAsync(user, "Participant");

                        await _signInManager.RefreshSignInAsync(user);
                    }
                }
            }
        }

        public void UpdateEvent(Event eventModel)
        {
            _repositoryWrapper.EventRepository.Update(eventModel);
            _repositoryWrapper.Save();
        }

        // Returns string? as an error message. If null, it succeeded.
        public string? RegisterUserForEvent(int eventId, string userId)
        {
            // 1. Check if event exists
            var eventDetails = _repositoryWrapper.EventRepository.GetEventByIdWithDetails(eventId);
            if (eventDetails == null) return "Event not found.";

            // 2. Check if user is the organizer itself
            if (eventDetails.OrganizerId == userId)
            {
                return "You cannot join your own event.";
            }
            // 3. Check if already registered
            if (_repositoryWrapper.RegistrationRepository.IsUserRegistered(eventId, userId))
            {
                return "User is already registered.";
            }

            // 4. Check Participant Limit
            int currentCount = _repositoryWrapper.RegistrationRepository.GetRegistrationCount(eventId);
            if (eventDetails.ParticipantLimit.HasValue && currentCount >= eventDetails.ParticipantLimit.Value)
            {
                return "Event is full.";
            }

            // 5. Create Registration
            var newRegistration = new Registration
            {
                EventId = eventId,
                UserId = userId,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _repositoryWrapper.RegistrationRepository.Create(newRegistration);
            _repositoryWrapper.Save();

            return null; // Success
        }
        public IEnumerable<Event> SearchEvents(string? searchTerm, string? category, string? location, DateTime? date)
        {
            return _repositoryWrapper.EventRepository.GetEventsByConditions(searchTerm, category, location, date);
        }

        public void LeaveEvent(int eventId, string userId)
        {
            _repositoryWrapper.RegistrationRepository.DeleteRegistration(eventId, userId);
            _repositoryWrapper.Save();
        }

        public IEnumerable<Registration> GetMyRegistrations(string userId)
        {
            return _repositoryWrapper.RegistrationRepository.GetRegistrationsByUser(userId);
        }
    }
}
