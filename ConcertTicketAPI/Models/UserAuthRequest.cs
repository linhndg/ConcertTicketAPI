using System.ComponentModel.DataAnnotations;

namespace ConcertTicketAPI.Models
{
    /// <summary>
    /// Represents a request for user authentication (login or registration).
    /// </summary>
    public class UserAuthRequest
    {
        /// <summary>
        /// Username of the user.
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}
