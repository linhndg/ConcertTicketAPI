namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for returning purchased ticket history.
    /// </summary>
    public class TicketHistoryResponseDTO
    {
        /// <summary>
        /// The ID of the ticket.
        /// </summary>
        public Guid TicketId { get; set; }

        /// <summary>
        /// The name of the event the ticket is for.
        /// </summary>
        public string EventName { get; set; } = null!;

        /// <summary>
        /// The purchase date and time.
        /// </summary>
        public DateTime PurchaseDate { get; set; }
    }
}