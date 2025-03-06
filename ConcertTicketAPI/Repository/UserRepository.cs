using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId)
        {
            return await _context.Tickets
                .Include(t => t.TicketType)
                .ThenInclude(tt => tt.Event)
                .Where(t => t.UserId == userId && t.Status == TicketStatus.Purchased)
                .ToListAsync();
        }
    }
}