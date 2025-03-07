namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for creating or updating a ticket type.
    /// </summary>
    public class TicketTypeRequestDTO
    {
        /// <summary>
        /// The ID of the event this ticket type belongs to.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// The name of the ticket type (e.g., VIP, General Admission).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The price of the ticket type.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of tickets available for this type.
        /// </summary>
        public int QuantityAvailable { get; set; }
    }
}