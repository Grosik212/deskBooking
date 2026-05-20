using deskBooking.Application.Desks.Commands;
using deskBooking.Application.Desks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace deskBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DesksController : ControllerBase
{
    private readonly IMediator _mediator;

    public DesksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Pobierz wszystkie biurka</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var desks = await _mediator.Send(new GetAllDesksQuery());
        return Ok(desks);
    }

    /// <summary>Pobierz biurko po ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var desk = await _mediator.Send(new GetDeskByIdQuery(id));
        return Ok(desk);
    }

    /// <summary>Utwórz nowe biurko</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDesk([FromBody] CreateDeskCommand command)
    {
        var deskId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = deskId }, new { Id = deskId });
    }

    /// <summary>Usuń biurko</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDesk(Guid id)
    {
        await _mediator.Send(new DeleteDeskCommand(id));
        return NoContent();
    }
}