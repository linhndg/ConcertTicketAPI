using ConcertTicketAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Repositories
{
    public interface IEventRepository
    {
        Task AddEventAsync(Event eventEntity);
        Task<Event?> GetEventByIdAsync(Guid eventId);
        Task UpdateEventAsync(Event eventEntity);
        Task<List<Event>> GetAllEventsAsync();
    }
}