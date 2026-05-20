namespace deskBooking.Application.Desks.Dtos;

public record DeskDto(
    Guid Id,
    string Name,
    bool IsStandingDesk,
    int BookingsCount);
