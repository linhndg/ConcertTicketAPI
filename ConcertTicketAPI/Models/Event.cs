using System.ComponentModel.DataAnnotations;

namespace ConcertTicketAPI.Models
{
    /// <summary>
    /// Represents a concert event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the concert event.
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the event.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Date and time of the event.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Venue where the event is taking place.
        /// </summary>
        [Required]
        public string Venue { get; set; } = null!;

        /// <summary>
        /// Maximum number of attendees allowed.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

    }
}