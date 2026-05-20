using deskBooking.Application.Bookings.Dtos;
using MediatR;

namespace deskBooking.Application.Bookings.Queries;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingDto>>;
