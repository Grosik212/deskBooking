using MediatR;

namespace deskBooking.Application.Bookings.Commands;

public record CreateBookingCommand(
    Guid DeskId,
    string UserName,
    DateTime StartTime,
    DateTime EndTime) : IRequest<Guid>;
