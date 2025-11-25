using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEvents.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public bool? IsApproved { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
        public Event? Event { get; set; }

    }
}
