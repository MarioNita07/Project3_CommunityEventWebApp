namespace CommunityEvents.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? Message { get; set; }
        public DateOnly DateCreated { get; set; }
        public bool? IsRead { get; set; }
        public int? RelatedEventId { get; set; }
        public Event? RelatedEvent { get; set; }

    }
}
