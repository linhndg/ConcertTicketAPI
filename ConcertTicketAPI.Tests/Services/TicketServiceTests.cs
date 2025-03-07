using ConcertTicketAPI.DTO;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ConcertTicketAPI.Tests.Services
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<ILogger<TicketService>> _loggerMock;
        private readonly TicketService _ticketService;

        public TicketServiceTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _loggerMock = new Mock<ILogger<TicketService>>();
            _ticketService = new TicketService(_ticketRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ReserveTicketsAsync_ShouldReturnReservationId_WhenTicketsAreAvailable()
        {
            // Arrange
            var ticketTypeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new TicketReservationRequestDTO
            {
                TicketTypeId = ticketTypeId,
                Quantity = 2,
                ReservationDurationMinutes = 15
            };

            var expectedReservationId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.ReserveTicketsAsync(ticketTypeId, request.Quantity, userId, TimeSpan.FromMinutes(request.ReservationDurationMinutes)))
                .ReturnsAsync(expectedReservationId);

            // Act
            var result = await _ticketService.ReserveTicketsAsync(request, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReservationId, result.ReservationId);
        }

        [Fact]
        public async Task ReserveTicketsAsync_ShouldReturnNull_WhenNotEnoughTicketsAvailable()
        {
            // Arrange
            var ticketTypeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new TicketReservationRequestDTO
            {
                TicketTypeId = ticketTypeId,
                Quantity = 5,
                ReservationDurationMinutes = 15
            };

            _ticketRepositoryMock
                .Setup(repo => repo.ReserveTicketsAsync(ticketTypeId, request.Quantity, userId, TimeSpan.FromMinutes(request.ReservationDurationMinutes)))
                .ReturnsAsync((Guid?)null);

            // Act
            var result = await _ticketService.ReserveTicketsAsync(request, userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PurchaseTicketsAsync_ShouldReturnSuccess_WhenReservationIsValid()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new TicketPurchaseRequestDTO { ReservationId = reservationId };

            _ticketRepositoryMock
                .Setup(repo => repo.PurchaseTicketsAsync(reservationId, userId))
                .ReturnsAsync(true);

            // Act
            var result = await _ticketService.PurchaseTicketsAsync(request, userId);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task PurchaseTicketsAsync_ShouldReturnFailure_WhenReservationIsInvalid()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new TicketPurchaseRequestDTO { ReservationId = reservationId };

            _ticketRepositoryMock
                .Setup(repo => repo.PurchaseTicketsAsync(reservationId, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _ticketService.PurchaseTicketsAsync(request, userId);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CancelReservationAsync_ShouldReturnTrue_WhenCancellationSucceeds()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.CancelReservationAsync(reservationId, userId))
                .ReturnsAsync(true);

            // Act
            var result = await _ticketService.CancelReservationAsync(reservationId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CancelReservationAsync_ShouldReturnFalse_WhenReservationIsInvalid()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(repo => repo.CancelReservationAsync(reservationId, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _ticketService.CancelReservationAsync(reservationId, userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUserPurchaseHistoryAsync_ShouldReturnListOfPurchasedTickets()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var purchasedTickets = new List<Ticket>
            {
                new Ticket { Id = Guid.NewGuid(), TicketType = new TicketType { Event = new Event { Name = "Concert 1" } }, ReservedUntil = DateTime.UtcNow },
                new Ticket { Id = Guid.NewGuid(), TicketType = new TicketType { Event = new Event { Name = "Concert 2" } }, ReservedUntil = DateTime.UtcNow }
            };

            _ticketRepositoryMock
                .Setup(repo => repo.GetUserPurchasedTicketsAsync(userId))
                .ReturnsAsync(purchasedTickets);

            // Act
            var result = await _ticketService.GetUserPurchaseHistoryAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.EventName == "Concert 1");
            Assert.Contains(result, t => t.EventName == "Concert 2");
        }
    }
}
