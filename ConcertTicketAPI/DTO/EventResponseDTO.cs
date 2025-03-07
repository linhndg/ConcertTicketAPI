namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for returning event details.
    /// </summary>
    public class EventResponseDTO
    {
        /// <summary>
        /// The unique identifier of the event.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// A brief description of the event.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The date and time when the event will take place.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The venue where the event is held.
        /// </summary>
        public string Venue { get; set; } = null!;

        /// <summary>
        /// The maximum number of attendees allowed.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// The ID of the user who created the event.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
