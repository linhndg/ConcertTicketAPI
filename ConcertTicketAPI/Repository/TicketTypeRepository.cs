using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketAPI.Repositories
{
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketType>> GetTicketTypesByEventAsync(Guid eventId)
        {
            return await _context.TicketTypes
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<TicketType?> GetTicketTypeByIdAsync(Guid id)
        {
            return await _context.TicketTypes.FindAsync(id);
        }

        public async Task CreateTicketTypeAsync(TicketType ticketType)
        {
            await _context.TicketTypes.AddAsync(ticketType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketTypeAsync(TicketType ticketType)
        {
            _context.TicketTypes.Update(ticketType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> TicketTypeExistsAsync(Guid id)
        {
            return await _context.TicketTypes.AnyAsync(t => t.Id == id);
        }
    }
}