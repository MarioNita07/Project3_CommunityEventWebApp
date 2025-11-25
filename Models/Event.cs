using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEvents.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? CategoryName { get; set; }
        public DateTime Date { get; set; }
        public int? ParticipantLimit { get; set; }
        public string OrganizerId { get; set; } = string.Empty;
        public IdentityUser? Organizer { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
