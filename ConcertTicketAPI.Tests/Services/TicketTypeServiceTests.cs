using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ConcertTicketAPI.Tests.Services
{
    public class TicketTypeServiceTests
    {
        private readonly Mock<ITicketTypeRepository> _ticketTypeRepositoryMock;
        private readonly TicketTypeService _ticketTypeService;

        public TicketTypeServiceTests()
        {
            _ticketTypeRepositoryMock = new Mock<ITicketTypeRepository>();
            _ticketTypeService = new TicketTypeService(_ticketTypeRepositoryMock.Object);
        }

        [Fact]
        public async Task AddTicketTypeAsync_ShouldReturnCreatedTicketType()
        {
            // Arrange
            var request = new TicketTypeRequestDTO
            {
                EventId = Guid.NewGuid(),
                Name = "VIP",
                Price = 150.00m,
                QuantityAvailable = 50
            };

            var createdTicketType = new TicketType
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                Name = request.Name,
                Price = request.Price,
                QuantityAvailable = request.QuantityAvailable
            };

            _ticketTypeRepositoryMock
                .Setup(repo => repo.AddTicketTypeAsync(It.IsAny<TicketType>()))
                .ReturnsAsync(createdTicketType);

            // Act
            var result = await _ticketTypeService.AddTicketTypeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdTicketType.Id, result.Id);
            Assert.Equal("VIP", result.Name);
            Assert.Equal(150.00m, result.Price);
            Assert.Equal(50, result.QuantityAvailable);
        }

        [Fact]
        public async Task GetTicketTypesByEventAsync_ShouldReturnListOfTicketTypes()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var ticketTypes = new List<TicketType>
            {
                new TicketType { Id = Guid.NewGuid(), EventId = eventId, Name = "General Admission", Price = 50.00m, QuantityAvailable = 100 },
                new TicketType { Id = Guid.NewGuid(), EventId = eventId, Name = "VIP", Price = 150.00m, QuantityAvailable = 50 }
            };

            _ticketTypeRepositoryMock
                .Setup(repo => repo.GetTicketTypesByEventAsync(eventId))
                .ReturnsAsync(ticketTypes);

            // Act
            var result = await _ticketTypeService.GetTicketTypesByEventAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "General Admission");
            Assert.Contains(result, t => t.Name == "VIP");
        }

        [Fact]
        public async Task GetTicketTypesByEventAsync_ShouldReturnEmptyList_WhenNoTicketTypesExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _ticketTypeRepositoryMock
                .Setup(repo => repo.GetTicketTypesByEventAsync(eventId))
                .ReturnsAsync(new List<TicketType>());

            // Act
            var result = await _ticketTypeService.GetTicketTypesByEventAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
