using ConcertTicketAPI.Controllers;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tickettypes")]
public class TicketTypesController : BaseController
{
    private readonly ITicketTypeService _ticketTypeService;

    public TicketTypesController(ITicketTypeService ticketTypeService)
    {
        _ticketTypeService = ticketTypeService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddTicketType([FromBody] TicketTypeRequestDTO request)
    {
        var ticketType = await _ticketTypeService.AddTicketTypeAsync(request);
        return CreatedAtAction(nameof(GetTicketTypesByEvent), new { eventId = ticketType.EventId }, ticketType);
    }

    [HttpGet("{eventId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTicketTypesByEvent(Guid eventId)
    {
        var ticketTypes = await _ticketTypeService.GetTicketTypesByEventAsync(eventId);
        return Ok(ticketTypes);
    }
}