using deskBooking.Application.Desks.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace deskBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesksController : ControllerBase
{
    private readonly IMediator _mediator;

    public DesksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDesk([FromBody] CreateDeskCommand command)
    {
        var deskId = await _mediator.Send(command);
        return Ok(new { Id = deskId });
    }
}