using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<EventResponseDTO> CreateEventAsync(EventRequestDTO request)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Venue = request.Venue,
                Capacity = request.Capacity
            };

            await _eventRepository.AddEventAsync(newEvent);

            return new EventResponseDTO
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Venue = newEvent.Venue,
                Capacity = newEvent.Capacity,
            };
        }

        public async Task<EventResponseDTO?> UpdateEventAsync(Guid eventId, EventRequestDTO request)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(eventId);
            if (existingEvent == null)
                return null;

            existingEvent.Name = request.Name;
            existingEvent.Description = request.Description;
            existingEvent.Date = request.Date;
            existingEvent.Venue = request.Venue;
            existingEvent.Capacity = request.Capacity;

            await _eventRepository.UpdateEventAsync(existingEvent);

            return new EventResponseDTO
            {
                Id = existingEvent.Id,
                Name = existingEvent.Name,
                Description = existingEvent.Description,
                Date = existingEvent.Date,
                Venue = existingEvent.Venue,
                Capacity = existingEvent.Capacity
            };
        }

        public async Task<List<EventResponseDTO>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return events.Select(e => new EventResponseDTO
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Venue = e.Venue,
                Capacity = e.Capacity
            }).ToList();
        }

        public async Task<EventResponseDTO?> GetEventDetailAsync(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity == null) return null;

            return new EventResponseDTO
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                Venue = eventEntity.Venue,
                Capacity = eventEntity.Capacity
            };
        }
    }
}
