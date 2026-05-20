using deskBooking.Domain.Entities;
using deskBooking.Domain.Interfaces;
using deskBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deskBooking.Infrastructure.Repositories;

public class DeskRepository : IDeskRepository
{
    private readonly ApplicationDbContext _context;

    public DeskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Desk>> GetAllAsync()
    {
        return await _context.Desks
            .Include(d => d.Bookings)
            .ToListAsync();
    }

    public async Task<Desk?> GetByIdAsync(Guid id)
    {
        return await _context.Desks
            .Include(d => d.Bookings)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Desk desk)
    {
        await _context.Desks.AddAsync(desk);
    }

    public Task DeleteAsync(Desk desk)
    {
        _context.Desks.Remove(desk);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}