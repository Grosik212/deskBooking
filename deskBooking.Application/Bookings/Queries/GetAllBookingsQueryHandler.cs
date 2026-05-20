using deskBooking.Application.Bookings.Dtos;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Bookings.Queries;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetAllAsync();

        return bookings.Select(b => new BookingDto(
            b.Id,
            b.DeskId,
            b.Desk.Name,
            b.UserName,
            b.StartTime,
            b.EndTime));
    }
}
