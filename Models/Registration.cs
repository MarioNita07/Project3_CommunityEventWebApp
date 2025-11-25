using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEvents.Models
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
        public Event? Event { get; set; }


    }
}
