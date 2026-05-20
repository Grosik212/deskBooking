using deskBooking.Application.Bookings.Commands;
using deskBooking.Domain.Entities;
using deskBooking.Domain.Exceptions;
using deskBooking.Domain.Interfaces;
using Moq;

namespace deskBooking.Tests.Application.Bookings;

public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly Mock<IDeskRepository> _deskRepoMock;
    private readonly CreateBookingCommandHandler _handler;

    public CreateBookingCommandHandlerTests()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _deskRepoMock = new Mock<IDeskRepository>();
        _handler = new CreateBookingCommandHandler(_bookingRepoMock.Object, _deskRepoMock.Object);
    }

    [Fact]
    public async Task Handle_DeskAvailable_ShouldCreateBookingAndReturnGuid()
    {
        // Arrange
        var deskId = Guid.NewGuid();
        var desk = new Desk { Id = deskId, Name = "Desk A", IsStandingDesk = false };
        var command = new CreateBookingCommand(deskId, "jan.kowalski", DateTime.Today, DateTime.Today.AddHours(8));

        _deskRepoMock.Setup(r => r.GetByIdAsync(deskId)).ReturnsAsync(desk);
        _bookingRepoMock.Setup(r => r.IsAvailableAsync(deskId, command.StartTime, command.EndTime)).ReturnsAsync(true);
        _bookingRepoMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
        _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _bookingRepoMock.Verify(r => r.AddAsync(It.Is<Booking>(b =>
            b.DeskId == deskId && b.UserName == "jan.kowalski")), Times.Once);
        _bookingRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeskNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var deskId = Guid.NewGuid();
        var command = new CreateBookingCommand(deskId, "user", DateTime.Today, DateTime.Today.AddHours(8));

        _deskRepoMock.Setup(r => r.GetByIdAsync(deskId)).ReturnsAsync((Desk?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeskAlreadyBooked_ShouldThrowDeskAlreadyBookedException()
    {
        // Arrange
        var deskId = Guid.NewGuid();
        var desk = new Desk { Id = deskId, Name = "Desk A" };
        var command = new CreateBookingCommand(deskId, "user", DateTime.Today, DateTime.Today.AddHours(8));

        _deskRepoMock.Setup(r => r.GetByIdAsync(deskId)).ReturnsAsync(desk);
        _bookingRepoMock.Setup(r => r.IsAvailableAsync(deskId, command.StartTime, command.EndTime)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<DeskAlreadyBookedException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _bookingRepoMock.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Never);
    }
}
