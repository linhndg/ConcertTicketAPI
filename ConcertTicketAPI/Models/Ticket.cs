namespace ConcertTicketAPI.Models
{
    /// <summary>
    /// Defines the possible statuses of a ticket.
    /// </summary>
    public enum TicketStatus
    {
        /// <summary>
        /// The ticket has been reserved but not yet purchased.
        /// </summary>
        Reserved,

        /// <summary>
        /// The ticket has been successfully purchased.
        /// </summary>
        Purchased,

        /// <summary>
        /// The ticket reservation or purchase has been cancelled.
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// Represents a ticket for an event.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Unique identifier for the ticket.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the ticket type this ticket belongs to.
        /// </summary>
        public Guid TicketTypeId { get; set; }

        /// <summary>
        /// ID of the user who purchased or reserved the ticket (nullable if not assigned yet).
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Status of the ticket (Reserved, Purchased, Cancelled).
        /// </summary>
        public string Status { get; set; } = "Reserved";

        /// <summary>
        /// Timestamp indicating until when the reservation is valid.
        /// </summary>
        public DateTime? ReservedUntil { get; set; }
    }
}