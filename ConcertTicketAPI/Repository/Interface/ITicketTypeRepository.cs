using ConcertTicketAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Repositories
{
    public interface ITicketTypeRepository
    {
        Task<TicketType?> GetTicketTypeByIdAsync(Guid ticketTypeId);
        Task<List<TicketType>> GetTicketTypesByEventAsync(Guid eventId);
        Task<TicketType> AddTicketTypeAsync(TicketType ticketType);
        Task<bool> UpdateTicketTypeAsync(TicketType ticketType);
        Task<bool> DeleteTicketTypeAsync(Guid ticketTypeId);
    }
}