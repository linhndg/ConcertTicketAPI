using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> ReserveTicketsAsync(Guid ticketTypeId, int quantity, TimeSpan reservationDuration)
        {
            var reservationId = Guid.NewGuid();
            var reservedUntil = DateTime.UtcNow.Add(reservationDuration);

            var tickets = Enumerable.Range(0, quantity).Select(_ => new Ticket
            {
                Id = Guid.NewGuid(),
                TicketTypeId = ticketTypeId,
                Status = TicketStatus.Reserved,
                ReservedUntil = reservedUntil
            });

            await _context.Tickets.AddRangeAsync(tickets);

            var ticketType = await _context.TicketTypes.FindAsync(ticketTypeId);
            ticketType.QuantityAvailable -= quantity;

            await _context.SaveChangesAsync();

            return reservationId;
        }

        public async Task<bool> PurchaseTicketsAsync(Guid reservationId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.Status == TicketStatus.Reserved && t.ReservedUntil > DateTime.UtcNow)
                .ToListAsync();

            if (!tickets.Any())
                return false;

            foreach (var ticket in tickets)
            {
                ticket.Status = TicketStatus.Purchased;
                ticket.ReservedUntil = null;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelReservationAsync(Guid reservationId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.Status == TicketStatus.Reserved && t.ReservedUntil > DateTime.UtcNow)
                .ToListAsync();

            if (!tickets.Any())
                return false;

            foreach (var ticket in tickets)
            {
                ticket.Status = TicketStatus.Cancelled;
                ticket.ReservedUntil = null;

                var ticketType = await _context.TicketTypes.FindAsync(ticket.TicketTypeId);
                ticketType.QuantityAvailable += 1;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetReservedOrPurchasedTicketCountAsync(Guid eventId)
        {
            return await _context.Tickets
                .Include(t => t.TicketType)
                .CountAsync(t => t.TicketType.EventId == eventId &&
                                 (t.Status == TicketStatus.Purchased ||
                                  (t.Status == TicketStatus.Reserved && t.ReservedUntil > DateTime.UtcNow)));
        }
    }
}
