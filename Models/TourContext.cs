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

        DbSet<Booking> bookings { set; get; }
        DbSet<Location> locations { set; get; }
        DbSet<Tours> tours { set; get; }
    }
}
