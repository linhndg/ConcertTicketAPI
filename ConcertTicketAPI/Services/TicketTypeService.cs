using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Services
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepository _ticketTypeRepository;

        public TicketTypeService(ITicketTypeRepository ticketTypeRepository)
        {
            _ticketTypeRepository = ticketTypeRepository;
        }

        public async Task<TicketTypeResponseDTO> AddTicketTypeAsync(TicketTypeRequestDTO request)
        {
            var ticketType = new TicketType
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                Name = request.Name,
                Price = request.Price,
                QuantityAvailable = request.QuantityAvailable
            };

            var createdTicketType = await _ticketTypeRepository.AddTicketTypeAsync(ticketType);

            return new TicketTypeResponseDTO
            {
                Id = createdTicketType.Id,
                EventId = createdTicketType.EventId,
                Name = createdTicketType.Name,
                Price = createdTicketType.Price,
                QuantityAvailable = createdTicketType.QuantityAvailable
            };
        }

        public async Task<List<TicketTypeResponseDTO>> GetTicketTypesByEventAsync(Guid eventId)
        {
            var ticketTypes = await _ticketTypeRepository.GetTicketTypesByEventAsync(eventId);

            return ticketTypes.Select(t => new TicketTypeResponseDTO
            {
                Id = t.Id,
                EventId = t.EventId,
                Name = t.Name,
                Price = t.Price,
                QuantityAvailable = t.QuantityAvailable
            }).ToList();
        }
    }
}