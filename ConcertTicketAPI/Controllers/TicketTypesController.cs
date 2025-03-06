using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[Route("api")]
[ApiController]
public class TicketTypesController : ControllerBase
{
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IEventRepository _eventRepository;

    public TicketTypesController(ITicketTypeRepository ticketTypeRepository, IEventRepository eventRepository)
    {
        _ticketTypeRepository = ticketTypeRepository;
        _eventRepository = eventRepository;
    }

    [HttpPost("events/{eventId}/ticket-types")]
    public async Task<IActionResult> CreateTicketType(Guid eventId, [FromBody] TicketType ticketType)
    {
        if (!await _eventRepository.EventExistsAsync(eventId))
            return NotFound("Event not found.");

        ticketType.Id = Guid.NewGuid();
        ticketType.EventId = eventId;

        await _ticketTypeRepository.CreateTicketTypeAsync(ticketType);

        return CreatedAtAction(nameof(GetTicketType), new { id = ticketType.Id }, ticketType);
    }

    [HttpGet("ticket-types/{id}")]
    public async Task<IActionResult> GetTicketType(Guid id)
    {
        var ticketType = await _ticketTypeRepository.GetTicketTypeByIdAsync(id);
        if (ticketType == null)
            return NotFound();

        return Ok(ticketType);
    }

    [HttpPut("ticket-types/{id}")]
    public async Task<IActionResult> UpdateTicketType(Guid id, [FromBody] TicketType ticketType)
    {
        if (id != ticketType.Id)
            return BadRequest();

        if (!await _ticketTypeRepository.TicketTypeExistsAsync(id))
            return NotFound();

        await _ticketTypeRepository.UpdateTicketTypeAsync(ticketType);
        return NoContent();
    }

    [HttpGet("events/{eventId}/ticket-types")]
    public async Task<IActionResult> GetTicketTypesByEvent(Guid eventId)
    {
        if (!await _eventRepository.EventExistsAsync(eventId))
            return NotFound("Event not found.");

        var ticketTypes = await _ticketTypeRepository.GetTicketTypesByEventAsync(eventId);
        return Ok(ticketTypes);
    }
}
