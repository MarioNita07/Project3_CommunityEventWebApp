namespace CommunityEvents.Models
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }

    }
}
