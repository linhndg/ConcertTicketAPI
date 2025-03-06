using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveTickets([FromBody] ReserveTicketsRequest request)
    {
        var reservationId = await _ticketService.ReserveTicketsAsync(
            request.TicketTypeId,
            request.Quantity,
            TimeSpan.FromMinutes(request.ReservationDurationMinutes));

        if (reservationId == null)
            return BadRequest("Unable to reserve tickets (check availability).");

        return Ok(new { ReservationId = reservationId });
    }

    [HttpPost("purchase/{reservationId}")]
    public async Task<IActionResult> PurchaseTickets(Guid reservationId)
    {
        var result = await _ticketService.PurchaseTicketsAsync(reservationId);
        if (!result)
            return BadRequest("Unable to purchase tickets (reservation invalid or expired).");

        return Ok("Tickets purchased successfully.");
    }

    [HttpPost("cancel/{reservationId}")]
    public async Task<IActionResult> CancelReservation(Guid reservationId)
    {
        var result = await _ticketService.CancelReservationAsync(reservationId);
        if (!result)
            return BadRequest("Unable to cancel reservation.");

        return Ok("Reservation cancelled successfully.");
    }
}

public class ReserveTicketsRequest
{
    public Guid TicketTypeId { get; set; }
    public int Quantity { get; set; }
    public int ReservationDurationMinutes { get; set; }
}