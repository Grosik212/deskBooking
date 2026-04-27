using deskBooking.Domain.Entities;

namespace deskBooking.Domain.Interfaces;

public interface IDeskRepository
{
    Task<IEnumerable<Desk>> GetAllAsync();
    Task<Desk?> GetByIdAsync(Guid id);
    Task AddAsync(Desk desk);
    Task SaveChangesAsync();
}