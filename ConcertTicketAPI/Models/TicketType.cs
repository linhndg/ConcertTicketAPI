using System.ComponentModel.DataAnnotations;

namespace ConcertTicketAPI.Models
{
    /// <summary>
    /// Represents a type of ticket available for an event.
    /// </summary>
    public class TicketType
    {
        /// <summary>
        /// Unique identifier for the ticket type.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the event this ticket belongs to.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Name of the ticket type (e.g., VIP, General Admission).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Price of the ticket in USD.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Number of tickets available for this type.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int QuantityAvailable { get; set; }

        public Event Event { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}