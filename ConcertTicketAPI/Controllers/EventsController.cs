using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : BaseController
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateEvent([FromBody] EventRequestDTO request)
    {
        var createdEvent = await _eventService.CreateEventAsync(request);
        return CreatedAtAction(nameof(GetEventDetail), new { eventId = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{eventId}")]
    [Authorize]
    public async Task<IActionResult> UpdateEvent(Guid eventId, [FromBody] EventRequestDTO request)
    {
        var updatedEvent = await _eventService.UpdateEventAsync(eventId, request);
        if (updatedEvent == null) return NotFound("Event not found or unauthorized.");
        return Ok(updatedEvent);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{eventId}")]
    [Authorize]
    public async Task<IActionResult> GetEventDetail(Guid eventId)
    {
        var eventDetail = await _eventService.GetEventDetailAsync(eventId);
        if (eventDetail == null) return NotFound("Event not found.");
        return Ok(eventDetail);
    }
}
