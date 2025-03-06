using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task CreateUserAsync(User user);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId);
    }
}