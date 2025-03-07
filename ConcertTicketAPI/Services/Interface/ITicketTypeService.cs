using ConcertTicketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Services
{
    public interface ITicketTypeService
    {
        Task<TicketTypeResponseDTO> AddTicketTypeAsync(TicketTypeRequestDTO request);
        Task<List<TicketTypeResponseDTO>> GetTicketTypesByEventAsync(Guid eventId);
    }
}