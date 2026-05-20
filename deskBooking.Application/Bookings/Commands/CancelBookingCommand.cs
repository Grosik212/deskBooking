using MediatR;

namespace deskBooking.Application.Bookings.Commands;

public record CancelBookingCommand(Guid Id) : IRequest;
