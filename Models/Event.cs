using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEvents.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string? Title { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Range(1, 5000, ErrorMessage = "Participant limit must be a non-negative number between 1 and 5000")]
        public int? ParticipantLimit { get; set; }
        public string OrganizerId { get; set; } = string.Empty;
        public IdentityUser? Organizer { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
