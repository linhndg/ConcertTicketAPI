using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; } // for future use
    }
}