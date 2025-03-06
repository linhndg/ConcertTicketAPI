using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _context.Events.Include(e => e.TicketTypes).ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.TicketTypes)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateEventAsync(Event eventEntity)
        {
            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventEntity)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EventExistsAsync(Guid id)
        {
            return await _context.Events.AnyAsync(e => e.Id == id);
        }
    }
}