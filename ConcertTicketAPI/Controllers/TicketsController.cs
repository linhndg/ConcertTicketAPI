using ConcertTicketAPI.Controllers;
using ConcertTicketAPI.DTO;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tickets")]
public class TicketsController : BaseController
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("reserve")]
    [Authorize]
    public async Task<IActionResult> ReserveTickets([FromBody] TicketReservationRequestDTO request)
    {
        Guid userId = GetUserIdFromToken();
        var reservation = await _ticketService.ReserveTicketsAsync(request, userId);

        if (reservation == null)
            return BadRequest("Unable to reserve tickets (check availability).");

        return Ok(reservation);
    }

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> PurchaseTickets([FromBody] TicketPurchaseRequestDTO request)
    {
        Guid userId = GetUserIdFromToken();
        var purchaseResult = await _ticketService.PurchaseTicketsAsync(request, userId);

        if (!purchaseResult.Success)
            return BadRequest("Unable to complete ticket purchase.");

        return Ok(purchaseResult);
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> GetUserPurchaseHistory()
    {
        Guid userId = GetUserIdFromToken();
        var history = await _ticketService.GetUserPurchaseHistoryAsync(userId);

        if (history.Count == 0)
            return NotFound("No ticket purchases found.");

        return Ok(history);
    }

    [HttpPost("cancel")]
    [Authorize]
    public async Task<IActionResult> CancelReservation([FromBody] TicketCancelRequestDTO request)
    {
        Guid userId = GetUserIdFromToken();
        var success = await _ticketService.CancelReservationAsync(request.ReservationId, userId);

        if (!success)
            return BadRequest("Unable to cancel reservation (check ownership or expiration).");

        return Ok(new { message = "Reservation successfully cancelled." });
    }

}