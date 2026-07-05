using HotelBooking.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Adding unique constraint on RoomNo
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.RoomNo)
                .IsUnique();
        }
    }
}
