using ConcertTicketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Services
{
    public interface IEventService
    {
        Task<EventResponseDTO> CreateEventAsync(EventRequestDTO request);
        Task<EventResponseDTO?> UpdateEventAsync(Guid eventId, EventRequestDTO request);
        Task<List<EventResponseDTO>> GetAllEventsAsync();
        Task<EventResponseDTO?> GetEventDetailAsync(Guid eventId);
    }
}