using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingTour.Models
{
    public class TourContext : IdentityDbContext<AppUser>
    {
        public TourContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Booking>? bookings { set; get; }
        public DbSet<Location> locations { set; get; }
        public DbSet<Tours> tours { set; get; }
    }
}
