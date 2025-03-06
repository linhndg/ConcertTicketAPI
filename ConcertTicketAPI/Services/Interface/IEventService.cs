using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(Guid id);
        Task<Event> CreateEventAsync(Event eventEntity);
        Task<bool> UpdateEventAsync(Guid id, Event eventEntity);
    }
}