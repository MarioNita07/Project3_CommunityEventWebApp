namespace CommunityEvents.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public bool? IsApproved { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }

    }
}
