using Microsoft.EntityFrameworkCore;
using TheaterEventPlanning.Models;

namespace TheaterEventPlanning.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) 
            :base(options)
        { }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .OwnsMany(e => e.CastMembers, cm =>
                {
                    cm.Property<int>("EventId"); // Foreign key
                    cm.HasKey("EventId", nameof(CastMember.CastMemberId)); // Composite key
                });
        }

    }
}
