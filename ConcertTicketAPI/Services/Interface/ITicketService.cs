using ConcertTicketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcertTicketAPI.DTO;

namespace ConcertTicketAPI.Services
{
    public interface ITicketService
    {
        Task<TicketReservationResponseDTO?> ReserveTicketsAsync(TicketReservationRequestDTO request, Guid userId);
        Task<TicketPurchaseResponseDTO> PurchaseTicketsAsync(TicketPurchaseRequestDTO request, Guid userId);
        Task<List<TicketHistoryResponseDTO>> GetUserPurchaseHistoryAsync(Guid userId);
        Task<bool> CancelReservationAsync(Guid reservationId, Guid userId);
    }
}