namespace deskBooking.Domain.Exceptions;

public class DeskAlreadyBookedException : Exception
{
    public DeskAlreadyBookedException(Guid deskId, DateTime startTime, DateTime endTime)
        : base($"Desk '{deskId}' is already booked from {startTime:g} to {endTime:g}.")
    {
    }
}
