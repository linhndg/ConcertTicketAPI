using ConcertTicketAPI.Models;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var eventEntity = await _eventService.GetEventByIdAsync(id);
        if (eventEntity == null)
            return NotFound();

        return Ok(eventEntity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] Event eventEntity)
    {
        var createdEvent = await _eventService.CreateEventAsync(eventEntity);
        return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] Event eventEntity)
    {
        var updated = await _eventService.UpdateEventAsync(id, eventEntity);
        if (!updated)
            return NotFound();

        return NoContent();
    }
}