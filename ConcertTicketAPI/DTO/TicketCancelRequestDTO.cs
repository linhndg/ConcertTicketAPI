namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for canceling a ticket reservation.
    /// </summary>
    public class TicketCancelRequestDTO
    {
        /// <summary>
        /// The reservation ID to cancel.
        /// </summary>
        public Guid ReservationId { get; set; }
    }
}