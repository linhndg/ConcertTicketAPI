using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories
{
    public interface ITicketRepository
    {
        Task<Guid> ReserveTicketsAsync(Guid ticketTypeId, int quantity, TimeSpan reservationDuration);
        Task<bool> PurchaseTicketsAsync(Guid reservationId);
        Task<bool> CancelReservationAsync(Guid reservationId);
        Task<int> GetReservedOrPurchasedTicketCountAsync(Guid eventId);
    }
}