using CinemaTicketBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Data;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Auditorium> Auditoriums => Set<Auditorium>();
    public DbSet<ShowTime> ShowTimes => Set<ShowTime>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Business rule: duplicate movie titles are not allowed
        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.Name)
            .IsUnique();

        modelBuilder.Entity<ShowTime>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Shows)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShowTime>()
            .HasOne(s => s.Auditorium)
            .WithMany(a => a.Shows)
            .HasForeignKey(s => s.AuditoriumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.ShowTime)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ShowTimeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}