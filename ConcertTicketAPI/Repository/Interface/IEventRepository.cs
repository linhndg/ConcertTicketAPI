using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetEventsAsync();
        Task<Event?> GetEventByIdAsync(Guid id);
        Task CreateEventAsync(Event eventEntity);
        Task UpdateEventAsync(Event eventEntity);
        Task<bool> EventExistsAsync(Guid id);
    }
}