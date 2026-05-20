using deskBooking.Domain.Entities;

namespace deskBooking.Domain.Interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<IEnumerable<Booking>> GetByDeskIdAsync(Guid deskId);
    Task<Booking?> GetByIdAsync(Guid id);
    Task AddAsync(Booking booking);
    Task DeleteAsync(Booking booking);
    Task<bool> IsAvailableAsync(Guid deskId, DateTime startTime, DateTime endTime);
    Task SaveChangesAsync();
}
