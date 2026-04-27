using MediatR;

namespace deskBooking.Application.Desks.Commands;

public record CreateDeskCommand(string Name, bool IsStandingDesk) : IRequest<Guid>;