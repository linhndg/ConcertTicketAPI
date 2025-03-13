namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for returning ticket reservation details.
    /// </summary>
    public class TicketReservationResponseDTO
    {
        /// <summary>
        /// The unique reservation ID.
        /// </summary>
        public Guid ReservationId { get; set; }

        /// <summary>
        /// Status of the reservation.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The expiration time for the reservation.
        /// </summary>
        public DateTime ReservedUntil { get; set; }
    }


}