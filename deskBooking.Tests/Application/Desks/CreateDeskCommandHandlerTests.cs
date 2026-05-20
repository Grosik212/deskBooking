using deskBooking.Application.Desks.Commands;
using deskBooking.Domain.Entities;
using deskBooking.Domain.Interfaces;
using Moq;

namespace deskBooking.Tests.Application.Desks;

public class CreateDeskCommandHandlerTests
{
    private readonly Mock<IDeskRepository> _deskRepositoryMock;
    private readonly CreateDeskCommandHandler _handler;

    public CreateDeskCommandHandlerTests()
    {
        _deskRepositoryMock = new Mock<IDeskRepository>();
        _handler = new CreateDeskCommandHandler(_deskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateDeskAndReturnGuid()
    {
        // Arrange
        var command = new CreateDeskCommand("Desk A", true);
        _deskRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Desk>())).Returns(Task.CompletedTask);
        _deskRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _deskRepositoryMock.Verify(r => r.AddAsync(It.Is<Desk>(d =>
            d.Name == "Desk A" && d.IsStandingDesk == true)), Times.Once);
        _deskRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldSetCorrectProperties()
    {
        // Arrange
        var command = new CreateDeskCommand("Standing Desk B", false);
        Desk? capturedDesk = null;

        _deskRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Desk>()))
            .Callback<Desk>(d => capturedDesk = d)
            .Returns(Task.CompletedTask);
        _deskRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedDesk);
        Assert.Equal("Standing Desk B", capturedDesk.Name);
        Assert.False(capturedDesk.IsStandingDesk);
        Assert.Equal(result, capturedDesk.Id);
    }
}
