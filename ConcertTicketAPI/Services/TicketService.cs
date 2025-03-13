using ConcertTicketAPI.DTO;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ITicketRepository ticketRepository, ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        public async Task<TicketReservationResponseDTO?> ReserveTicketsAsync(TicketReservationRequestDTO request, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Reservation failed: UserId is required.");
                return null;
            }

            _logger.LogInformation("User {UserId} reserving {Quantity} tickets for TicketTypeId {TicketTypeId}", userId, request.Quantity, request.TicketTypeId);

            var reservationId = await _ticketRepository.ReserveTicketsAsync(request.TicketTypeId, request.Quantity, userId, TimeSpan.FromMinutes(request.ReservationDurationMinutes));

            if (reservationId == null)
            {
                _logger.LogWarning("Failed to reserve tickets: Not enough availability.");
                return null;
            }

            return new TicketReservationResponseDTO
            {
                ReservationId = reservationId.Value,
                Status = TicketStatus.Reserved.ToString(),
                ReservedUntil = DateTime.UtcNow.AddMinutes(request.ReservationDurationMinutes)
            };
        }

        public async Task<TicketPurchaseResponseDTO> PurchaseTicketsAsync(TicketPurchaseRequestDTO request, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Purchase failed: UserId is required.");
                return new TicketPurchaseResponseDTO { Success = false };
            }

            _logger.LogInformation("User {UserId} purchasing ticket for ReservationId {ReservationId}", userId, request.ReservationId);

            var success = await _ticketRepository.PurchaseTicketsAsync(request.ReservationId, userId);

            return new TicketPurchaseResponseDTO { Success = success };
        }

        public async Task<List<TicketHistoryResponseDTO>> GetUserPurchaseHistoryAsync(Guid userId)
        {
            var purchasedTickets = await _ticketRepository.GetUserPurchasedTicketsAsync(userId);

            return purchasedTickets.Select(t => new TicketHistoryResponseDTO
            {
                TicketId = t.Id,
                EventName = t.TicketType.Event.Name,
                PurchaseDate = t.ReservedUntil ?? DateTime.UtcNow,
                TicketTypeName = t.TicketType.Name

            }).ToList();
        }
        public async Task<bool> CancelReservationAsync(Guid reservationId, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Cancel failed: UserId is required.");
                return false;
            }

            _logger.LogInformation("User {UserId} attempting to cancel reservation {ReservationId}", userId, reservationId);

            var success = await _ticketRepository.CancelReservationAsync(reservationId, userId);

            if (!success)
            {
                _logger.LogWarning("Failed to cancel reservation: ReservationId {ReservationId} is invalid or does not belong to UserId {UserId}", reservationId, userId);
            }

            return success;
        }

    }
}
