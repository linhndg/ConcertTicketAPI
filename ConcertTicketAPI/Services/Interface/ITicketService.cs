namespace ConcertTicketAPI.Services
{
    public interface ITicketService
    {
        Task<Guid?> ReserveTicketsAsync(Guid ticketTypeId, int quantity, TimeSpan reservationDuration);
        Task<bool> PurchaseTicketsAsync(Guid reservationId);
        Task<bool> CancelReservationAsync(Guid reservationId);
    }
}