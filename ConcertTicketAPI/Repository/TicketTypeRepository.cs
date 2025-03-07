using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Repositories
{
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketType?> GetTicketTypeByIdAsync(Guid ticketTypeId)
        {
            return await _context.TicketTypes.FindAsync(ticketTypeId);
        }

        public async Task<List<TicketType>> GetTicketTypesByEventAsync(Guid eventId)
        {
            return await _context.TicketTypes
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<TicketType> AddTicketTypeAsync(TicketType ticketType)
        {
            _context.TicketTypes.Add(ticketType);
            await _context.SaveChangesAsync();
            return ticketType;
        }

        public async Task<bool> UpdateTicketTypeAsync(TicketType ticketType)
        {
            _context.TicketTypes.Update(ticketType);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTicketTypeAsync(Guid ticketTypeId)
        {
            var ticketType = await GetTicketTypeByIdAsync(ticketTypeId);
            if (ticketType == null)
                return false;

            _context.TicketTypes.Remove(ticketType);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}