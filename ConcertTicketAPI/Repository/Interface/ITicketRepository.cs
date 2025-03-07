using ConcertTicketAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Repositories
{
    public interface ITicketRepository
    {
        Task<Guid?> ReserveTicketsAsync(Guid ticketTypeId, int quantity, Guid userId, TimeSpan reservationDuration);
        Task<bool> PurchaseTicketsAsync(Guid reservationId, Guid userId);
        Task<List<Ticket>> GetUserPurchasedTicketsAsync(Guid userId);
        Task<bool> CancelReservationAsync(Guid reservationId, Guid userId);
    }
}