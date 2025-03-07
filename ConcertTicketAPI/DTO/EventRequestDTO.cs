namespace ConcertTicketAPI.DTOs
{
    /// <summary>
    /// DTO for creating or updating an event.
    /// </summary>
    public class EventRequestDTO
    {
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
    }
}
