using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;

namespace ConcertTicketAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetEventsAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _eventRepository.GetEventByIdAsync(id);
        }

        public async Task<Event> CreateEventAsync(Event eventEntity)
        {
            eventEntity.Id = Guid.NewGuid();
            await _eventRepository.CreateEventAsync(eventEntity);
            return eventEntity;
        }

        public async Task<bool> UpdateEventAsync(Guid id, Event eventEntity)
        {
            if (!await _eventRepository.EventExistsAsync(id))
                return false;

            eventEntity.Id = id;
            await _eventRepository.UpdateEventAsync(eventEntity);
            return true;
        }
    }
}