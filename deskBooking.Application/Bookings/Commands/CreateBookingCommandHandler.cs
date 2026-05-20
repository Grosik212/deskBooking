using deskBooking.Domain.Entities;
using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Bookings.Commands;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IDeskRepository _deskRepository;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IDeskRepository deskRepository)
    {
        _bookingRepository = bookingRepository;
        _deskRepository = deskRepository;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Sprawdź czy biurko istnieje
        var desk = await _deskRepository.GetByIdAsync(request.DeskId)
            ?? throw new NotFoundException(nameof(Desk), request.DeskId);

        // Sprawdź czy termin jest wolny
        var isAvailable = await _bookingRepository.IsAvailableAsync(
            request.DeskId, request.StartTime, request.EndTime);

        if (!isAvailable)
            throw new DeskAlreadyBookedException(request.DeskId, request.StartTime, request.EndTime);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            DeskId = request.DeskId,
            UserName = request.UserName,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return booking.Id;
    }
}
