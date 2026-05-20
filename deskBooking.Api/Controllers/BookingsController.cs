using deskBooking.Application.Bookings.Commands;
using deskBooking.Application.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace deskBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Pobierz wszystkie rezerwacje</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _mediator.Send(new GetAllBookingsQuery());
        return Ok(bookings);
    }

    /// <summary>Pobierz rezerwacje konkretnego biurka</summary>
    [HttpGet("desk/{deskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDesk(Guid deskId)
    {
        var bookings = await _mediator.Send(new GetBookingsByDeskQuery(deskId));
        return Ok(bookings);
    }

    /// <summary>Zarezerwuj biurko</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var bookingId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = bookingId }, new { Id = bookingId });
    }

    /// <summary>Anuluj rezerwację</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        await _mediator.Send(new CancelBookingCommand(id));
        return NoContent();
    }
}
