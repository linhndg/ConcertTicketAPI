namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for reserving tickets.
    /// </summary>
    public class TicketReservationRequestDTO
    {
        /// <summary>
        /// The ID of the ticket type being reserved.
        /// </summary>
        public Guid TicketTypeId { get; set; }

        /// <summary>
        /// The number of tickets to reserve.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The duration (in minutes) for which the ticket will be reserved.
        /// </summary>
        public int ReservationDurationMinutes { get; set; }
    }

    
}