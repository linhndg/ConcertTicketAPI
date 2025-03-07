namespace ConcertTicketAPI.DTO
{
    /// <summary>
    /// DTO for purchasing tickets.
    /// </summary>
    public class TicketPurchaseRequestDTO
    {
        /// <summary>
        /// The reservation ID for the purchase.
        /// </summary>
        public Guid ReservationId { get; set; }
    }
}
