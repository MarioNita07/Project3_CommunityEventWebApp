using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Models
{
    public class CommunityEventContext : IdentityDbContext<IdentityUser>
    {
        public CommunityEventContext(DbContextOptions<CommunityEventContext> options)
            : base(options)
        { }
        
        public DbSet<Event>? Events { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Registration>? Registrations { get; set; }
        public DbSet<Notification>? Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
