using deskBooking.Domain.Entities;
using deskBooking.Domain.Interfaces;
using deskBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deskBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Desk)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByDeskIdAsync(Guid deskId)
    {
        return await _context.Bookings
            .Include(b => b.Desk)
            .Where(b => b.DeskId == deskId)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Desk)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public Task DeleteAsync(Booking booking)
    {
        _context.Bookings.Remove(booking);
        return Task.CompletedTask;
    }

    public async Task<bool> IsAvailableAsync(Guid deskId, DateTime startTime, DateTime endTime)
    {
        return !await _context.Bookings.AnyAsync(b =>
            b.DeskId == deskId &&
            b.StartTime < endTime &&
            b.EndTime > startTime);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
