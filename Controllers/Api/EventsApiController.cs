using Microsoft.AspNetCore.Mvc;
using CommunityEvents.Services.Interfaces;
using CommunityEvents.Models;

namespace CommunityEvents.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsApiController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsApiController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/EventsApi
        [HttpGet]
        public IEnumerable<Event> GetEvents()
        {
            // Returns raw JSON data instead of an HTML view
            return _eventService.GetUpcomingEvents();
        }

        // GET: api/EventsApi/5
        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            var @event = _eventService.GetEventById(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }
    }
}