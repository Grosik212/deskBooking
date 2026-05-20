using deskBooking.Application.Desks.Dtos;
using MediatR;

namespace deskBooking.Application.Desks.Queries;

public record GetDeskByIdQuery(Guid Id) : IRequest<DeskDto?>;
