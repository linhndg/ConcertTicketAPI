using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace ConcertTicketAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<TicketService> _logger; // ✅ Inject Logger

        public TicketService(
            ITicketRepository ticketRepository,
            ITicketTypeRepository ticketTypeRepository,
            IEventRepository eventRepository,
            ILogger<TicketService> logger) // ✅ Inject Logger in Constructor
        {
            _ticketRepository = ticketRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public async Task<Guid?> ReserveTicketsAsync(Guid ticketTypeId, int quantity, TimeSpan reservationDuration)
        {
            _logger.LogInformation("Attempting to reserve {Quantity} tickets for TicketTypeId {TicketTypeId}", quantity, ticketTypeId);

            var ticketType = await _ticketTypeRepository.GetTicketTypeByIdAsync(ticketTypeId);
            if (ticketType == null)
            {
                _logger.LogWarning("Failed to reserve tickets: TicketTypeId {TicketTypeId} not found", ticketTypeId);
                return null;
            }

            if (ticketType.QuantityAvailable < quantity)
            {
                _logger.LogWarning("Failed to reserve tickets: Only {Available} tickets available for TicketTypeId {TicketTypeId}", ticketType.QuantityAvailable, ticketTypeId);
                return null;
            }

            var eventEntity = await _eventRepository.GetEventByIdAsync(ticketType.EventId);
            if (eventEntity == null)
            {
                _logger.LogError("Failed to reserve tickets: Event not found for TicketTypeId {TicketTypeId}", ticketTypeId);
                return null;
            }

            int reservedOrPurchasedCount = await _ticketRepository.GetReservedOrPurchasedTicketCountAsync(eventEntity.Id);
            if (reservedOrPurchasedCount + quantity > eventEntity.Capacity)
            {
                _logger.LogWarning("Reservation exceeds event capacity for EventId {EventId}", eventEntity.Id);
                return null;
            }

            var reservationId = await _ticketRepository.ReserveTicketsAsync(ticketTypeId, quantity, reservationDuration);
            _logger.LogInformation("Successfully reserved {Quantity} tickets with ReservationId {ReservationId}", quantity, reservationId);

            return reservationId;
        }

        public async Task<bool> PurchaseTicketsAsync(Guid reservationId)
        {
            _logger.LogInformation("Attempting to purchase tickets for ReservationId {ReservationId}", reservationId);

            var success = await _ticketRepository.PurchaseTicketsAsync(reservationId);
            if (!success)
            {
                _logger.LogWarning("Failed to purchase tickets: ReservationId {ReservationId} is invalid or expired", reservationId);
                return false;
            }

            _logger.LogInformation("Successfully purchased tickets for ReservationId {ReservationId}", reservationId);
            return true;
        }

        public async Task<bool> CancelReservationAsync(Guid reservationId)
        {
            _logger.LogInformation("Attempting to cancel reservation for ReservationId {ReservationId}", reservationId);

            var success = await _ticketRepository.CancelReservationAsync(reservationId);
            if (!success)
            {
                _logger.LogWarning("Failed to cancel reservation: ReservationId {ReservationId} is invalid or expired", reservationId);
                return false;
            }

            _logger.LogInformation("Successfully canceled reservation for ReservationId {ReservationId}", reservationId);
            return true;
        }
    }
}
