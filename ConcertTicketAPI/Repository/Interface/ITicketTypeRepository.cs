using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories
{
    public interface ITicketTypeRepository
    {
        Task<IEnumerable<TicketType>> GetTicketTypesByEventAsync(Guid eventId);
        Task<TicketType?> GetTicketTypeByIdAsync(Guid id);
        Task CreateTicketTypeAsync(TicketType ticketType);
        Task UpdateTicketTypeAsync(TicketType ticketType);
        Task<bool> TicketTypeExistsAsync(Guid id);
    }
}