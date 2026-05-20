using deskBooking.Application.Bookings.Dtos;
using MediatR;

namespace deskBooking.Application.Bookings.Queries;

public record GetBookingsByDeskQuery(Guid DeskId) : IRequest<IEnumerable<BookingDto>>;
