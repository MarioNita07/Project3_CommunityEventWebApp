namespace CommunityEvents.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime Date { get; set; }
        public int? ParticipantLimit { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Registration>? Registrations { get; set; }
        public Category? Category { get; set; }
    }
}
