using deskBooking.Application.Bookings.Dtos;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Bookings.Queries;

public class GetBookingsByDeskQueryHandler : IRequestHandler<GetBookingsByDeskQuery, IEnumerable<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsByDeskQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<BookingDto>> Handle(GetBookingsByDeskQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByDeskIdAsync(request.DeskId);

        return bookings.Select(b => new BookingDto(
            b.Id,
            b.DeskId,
            b.Desk.Name,
            b.UserName,
            b.StartTime,
            b.EndTime));
    }
}
