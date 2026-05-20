namespace deskBooking.Application.Bookings.Dtos;

public record BookingDto(
    Guid Id,
    Guid DeskId,
    string DeskName,
    string UserName,
    DateTime StartTime,
    DateTime EndTime);
