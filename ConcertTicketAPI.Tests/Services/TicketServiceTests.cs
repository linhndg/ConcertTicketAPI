using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<ITicketTypeRepository> _ticketTypeRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ILogger<TicketService>> _loggerMock;
        private readonly TicketService _ticketService;

        public TicketServiceTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _ticketTypeRepositoryMock = new Mock<ITicketTypeRepository>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _loggerMock = new Mock<ILogger<TicketService>>();

            _ticketService = new TicketService(
                _ticketRepositoryMock.Object,
                _ticketTypeRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task ReserveTicketsAsync_ShouldReturnReservationId_WhenTicketsAreAvailable()
        {
            // Arrange
            var ticketTypeId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var quantity = 2;
            var duration = TimeSpan.FromMinutes(15);

            _ticketTypeRepositoryMock
                .Setup(repo => repo.GetTicketTypeByIdAsync(ticketTypeId))
                .ReturnsAsync(new TicketType
                {
                    Id = ticketTypeId,
                    EventId = eventId,
                    QuantityAvailable = 10
                });

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync(new Event
                {
                    Id = eventId,
                    Capacity = 100
                });

            _ticketRepositoryMock
                .Setup(repo => repo.GetReservedOrPurchasedTicketCountAsync(eventId))
                .ReturnsAsync(90); // 90 already sold/reserved

            var expectedReservationId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.ReserveTicketsAsync(ticketTypeId, quantity, duration))
                .ReturnsAsync(expectedReservationId);

            // Act
            var result = await _ticketService.ReserveTicketsAsync(ticketTypeId, quantity, duration);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReservationId, result);
        }

        [Fact]
        public async Task ReserveTicketsAsync_ShouldReturnNull_WhenExceedingEventCapacity()
        {
            // Arrange
            var ticketTypeId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var quantity = 15;
            var duration = TimeSpan.FromMinutes(15);

            _ticketTypeRepositoryMock
                .Setup(repo => repo.GetTicketTypeByIdAsync(ticketTypeId))
                .ReturnsAsync(new TicketType
                {
                    Id = ticketTypeId,
                    EventId = eventId,
                    QuantityAvailable = 20
                });

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync(new Event
                {
                    Id = eventId,
                    Capacity = 100
                });

            _ticketRepositoryMock
                .Setup(repo => repo.GetReservedOrPurchasedTicketCountAsync(eventId))
                .ReturnsAsync(90); // 90 tickets already reserved/purchased

            // Act
            var result = await _ticketService.ReserveTicketsAsync(ticketTypeId, quantity, duration);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PurchaseTicketsAsync_ShouldReturnTrue_WhenReservationIsValid()
        {
            // Arrange
            var reservationId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.PurchaseTicketsAsync(reservationId))
                .ReturnsAsync(true);

            // Act
            var result = await _ticketService.PurchaseTicketsAsync(reservationId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CancelReservationAsync_ShouldReturnTrue_WhenCancellationSucceeds()
        {
            // Arrange
            var reservationId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.CancelReservationAsync(reservationId))
                .ReturnsAsync(true);

            // Act
            var result = await _ticketService.CancelReservationAsync(reservationId);

            // Assert
            Assert.True(result);
        }
    }
}
