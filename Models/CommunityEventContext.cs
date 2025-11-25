using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Models
{
    public class CommunityEventContext : DbContext
    {
        public CommunityEventContext(DbContextOptions<CommunityEventContext> options)
            : base(options)
        { }
        
        public DbSet<Event>? Events { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Registration>? Registrations { get; set; }
        public DbSet<Notification>? Notifications { get; set; }

    }
}
