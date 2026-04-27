using deskBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace deskBooking.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Desk> Desks { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Desk)
            .WithMany(d => d.Bookings)
            .HasForeignKey(b => b.DeskId);
    }
}