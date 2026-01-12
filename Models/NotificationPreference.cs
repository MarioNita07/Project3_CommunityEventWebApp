using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;    

namespace CommunityEvents.Models
{
    public class NotificationPreference
    {
        [Key]
        public int NotificationPreferenceId { get; set; }
        public string UserId { get; set; } = string.Empty;
       
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        // Settings
        public bool ReceiveNewEventAlerts { get; set; } = true;
        public bool ReceiveUpdateAlerts { get; set; } = true;
        public bool ReceiveCommentAlerts { get; set; } = true;


    }
}
