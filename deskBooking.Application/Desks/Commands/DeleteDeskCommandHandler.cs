using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using MediatR;

namespace deskBooking.Application.Desks.Commands;

public class DeleteDeskCommandHandler : IRequestHandler<DeleteDeskCommand>
{
    private readonly IDeskRepository _deskRepository;

    public DeleteDeskCommandHandler(IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task Handle(DeleteDeskCommand request, CancellationToken cancellationToken)
    {
        var desk = await _deskRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Desk", request.Id);

        await _deskRepository.DeleteAsync(desk);
        await _deskRepository.SaveChangesAsync();
    }
}
