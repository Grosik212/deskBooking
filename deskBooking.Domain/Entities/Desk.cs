namespace deskBooking.Domain.Entities;

public class Desk
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsStandingDesk { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}