using System.ComponentModel.DataAnnotations;

namespace ConcertTicketAPI.Models
{
    /// <summary>
    /// Represents a user of the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Username of the user.
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Hashed password for security.
        /// </summary>
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Role of the user (e.g., Admin, User).
        /// </summary>
        public string Role { get; set; } = "User";
    }
}