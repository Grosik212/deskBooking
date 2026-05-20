using deskBooking.Application.Bookings.Commands;
using deskBooking.Domain.Entities;
using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using Moq;

namespace deskBooking.Tests.Application.Bookings;

public class CancelBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly CancelBookingCommandHandler _handler;

    public CancelBookingCommandHandlerTests()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _handler = new CancelBookingCommandHandler(_bookingRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBooking_ShouldCancelSuccessfully()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            DeskId = Guid.NewGuid(),
            UserName = "jan.kowalski",
            StartTime = DateTime.Today,
            EndTime = DateTime.Today.AddHours(8)
        };

        _bookingRepoMock.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync(booking);
        _bookingRepoMock.Setup(r => r.DeleteAsync(booking)).Returns(Task.CompletedTask);
        _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(new CancelBookingCommand(bookingId), CancellationToken.None);

        // Assert
        _bookingRepoMock.Verify(r => r.DeleteAsync(booking), Times.Once);
        _bookingRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBooking_ShouldThrowNotFoundException()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingRepoMock.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync((Booking?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(new CancelBookingCommand(bookingId), CancellationToken.None));

        _bookingRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Booking>()), Times.Never);
    }
}
