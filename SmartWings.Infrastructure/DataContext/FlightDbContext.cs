using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.DataContext
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔗 Booking → User (Many-to-One)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // 🔗 Booking → Flight (Many-to-One)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId);

            // 🔗 Flight → AirCraft (Many-to-One)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.AirCraft)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AircraftId);

            // 🔗 Seat → Flight (Many-to-One)
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId);

            // 🔗 Booking → Payment (One-to-One)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            // 🔗 Payment → User (Many-to-One)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade loop

            // 🔗 Booking → Passengers (One-to-Many)
            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Passengers)
                .HasForeignKey(p => p.BookingId);

            // 🔗 Passenger → Seat (One-to-One)
            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.Seat)
                .WithMany() // No navigation from Seat to Passenger
                .HasForeignKey(p => p.SeatId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of assigned seat

            // 🔗 Notification → User (Many-to-One)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade loop

            // 🔗 Notification → Booking (Many-to-One)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Booking)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Safe cascade

            // 🔒 Unique Seat per Flight
            modelBuilder.Entity<Seat>()
                .HasIndex(s => new { s.FlightId, s.SeatNumber })
                .IsUnique(); // Prevent duplicate seat assignment

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2); // or .HasColumnType("decimal(18,2)")

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<AirCraft> AirCrafts { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        public DbSet<OtpRecord> OtpRecords { get; set; } // For OTP functionality for password reset
    }
}
