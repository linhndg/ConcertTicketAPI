namespace ConcertTicketAPI.DTO
{
    /// <summary>
    /// DTO for returning purchased ticket details.
    /// </summary>
    public class TicketPurchaseResponseDTO
    {
        /// <summary>
        /// Indicates whether the ticket purchase was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}
