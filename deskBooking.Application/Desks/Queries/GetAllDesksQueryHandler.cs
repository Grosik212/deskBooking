using deskBooking.Application.Desks.Dtos;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Desks.Queries;

public class GetAllDesksQueryHandler : IRequestHandler<GetAllDesksQuery, IEnumerable<DeskDto>>
{
    private readonly IDeskRepository _deskRepository;

    public GetAllDesksQueryHandler(IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<IEnumerable<DeskDto>> Handle(GetAllDesksQuery request, CancellationToken cancellationToken)
    {
        var desks = await _deskRepository.GetAllAsync();

        return desks.Select(d => new DeskDto(
            d.Id,
            d.Name,
            d.IsStandingDesk,
            d.Bookings.Count));
    }
}
