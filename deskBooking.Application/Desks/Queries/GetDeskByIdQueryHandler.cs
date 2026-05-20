using deskBooking.Application.Desks.Dtos;
using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Desks.Queries;

public class GetDeskByIdQueryHandler : IRequestHandler<GetDeskByIdQuery, DeskDto?>
{
    private readonly IDeskRepository _deskRepository;

    public GetDeskByIdQueryHandler(IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<DeskDto?> Handle(GetDeskByIdQuery request, CancellationToken cancellationToken)
    {
        var desk = await _deskRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Desk", request.Id);

        return new DeskDto(
            desk.Id,
            desk.Name,
            desk.IsStandingDesk,
            desk.Bookings.Count);
    }
}
