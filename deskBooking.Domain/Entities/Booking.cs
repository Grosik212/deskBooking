namespace deskBooking.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid DeskId { get; set; }
    public Desk Desk { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string UserName { get; set; } = string.Empty;
}