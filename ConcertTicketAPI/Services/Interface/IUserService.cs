using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Services
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(string username, string password);
        Task<string?> AuthenticateAsync(string username, string password);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId);
    }
}