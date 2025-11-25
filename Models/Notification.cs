using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEvents.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool? IsRead { get; set; }
        public int? RelatedEventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
        public Event? RelatedEvent { get; set; }

    }
}
