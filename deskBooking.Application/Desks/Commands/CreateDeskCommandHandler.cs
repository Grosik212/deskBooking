using deskBooking.Domain.Entities;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Desks.Commands;

public class CreateDeskCommandHandler : IRequestHandler<CreateDeskCommand, Guid>
{
    private readonly IDeskRepository _deskRepository;

    public CreateDeskCommandHandler(IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<Guid> Handle(CreateDeskCommand request, CancellationToken cancellationToken)
    {
        var desk = new Desk
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsStandingDesk = request.IsStandingDesk
        };

        await _deskRepository.AddAsync(desk);
        await _deskRepository.SaveChangesAsync();

        return desk.Id;
    }
}