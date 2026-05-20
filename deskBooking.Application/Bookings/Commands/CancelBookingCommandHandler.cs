using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Bookings.Commands;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;

    public CancelBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Booking", request.Id);

        await _bookingRepository.DeleteAsync(booking);
        await _bookingRepository.SaveChangesAsync();
    }
}
