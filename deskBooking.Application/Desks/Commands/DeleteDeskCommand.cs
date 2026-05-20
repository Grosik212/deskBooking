using MediatR;

namespace deskBooking.Application.Desks.Commands;

public record DeleteDeskCommand(Guid Id) : IRequest;
