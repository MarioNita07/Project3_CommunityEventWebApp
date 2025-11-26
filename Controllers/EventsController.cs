using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CommunityEvents.Models;
using CommunityEvents.Services.Interfaces;

namespace CommunityEvents.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(IEventService eventService, UserManager<IdentityUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        // GET: Events
        // Shows all upcoming events (Visible to everyone)
        public IActionResult Index(string? searchTerm, string? category, string? location, DateTime? date)
        {
            ViewBag.CurrentSearchTerm = searchTerm;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentLocation = location;
            ViewBag.CurrentDate = date?.ToString("yyyy-MM-dd");

            var events = _eventService.SearchEvents(searchTerm, category, location, date);
            return View(events);
        }

        // GET: Events/MyEvents
        // Shows only events created by the logged-in Organizer
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> MyEvents()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var myEvents = _eventService.GetMyEvents(user.Id);
            return View(myEvents);
        }

        // GET: Events/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _eventService.GetEventById(id.Value);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "Participant,Organizer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Participant,Organizer")]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,CategoryName,Date,ParticipantLimit")] Event @event)
        {
            // Remove Organizer validation because we set it manually below
            ModelState.Remove("Organizer");
            ModelState.Remove("OrganizerId");

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Challenge();

                // AUTOMATICALLY ASSIGN THE LOGGED-IN USER AS ORGANIZER
                @event.OrganizerId = user.Id;

                await _eventService.CreateEvent(@event);

                _eventService.CreateEvent(@event);

                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        //GET: Events/Edit/5
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = _eventService.GetEventById(id.Value);
            if (@event == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && @event.OrganizerId != user.Id)
            {
                return Forbid(); // 403 Forbidden
            }

            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Description,Location,CategoryName,Date,ParticipantLimit,OrganizerId")] Event @event)
        {
            if (id != @event.EventId) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && @event.OrganizerId != user.Id)
            {
                return Forbid();
            }

            ModelState.Remove("Organizer");
            ModelState.Remove("Reviews");
            ModelState.Remove("Registrations");
            ModelState.Remove("Notifications");

            if (ModelState.IsValid)
            {
                try
                {
                    _eventService.UpdateEvent(@event);
                }
                catch (Exception)
                {
                    if (_eventService.GetEventById(id) == null) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // POST: Events/Join/5
        // Action to handle registrations
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Call the service to attempt registration
            // Returns null if successful, or an error string if failed
            string? errorMessage = _eventService.RegisterUserForEvent(id, user.Id);

            if (errorMessage == null)
            {
                TempData["SuccessMessage"] = "You have successfully registered for this event!";
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
            }

            // Redirect back to the details page to show the message
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = _eventService.GetEventById(id.Value);
            if (@event == null) return NotFound();

            // Ensure the user deleting it is the one who created it (or is Admin)
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && @event.OrganizerId != user.Id)
            {
                return Forbid(); 
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            await _eventService.DeleteEvent(id, user.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}