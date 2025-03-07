using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventService = new EventService(_eventRepositoryMock.Object);
        }

        [Fact]
        public async Task GetEventDetailAsync_ReturnsEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event
            {
                Id = eventId,
                Name = "Test Event",
                Description = "Description",
                Date = DateTime.UtcNow,
                Venue = "Venue",
                Capacity = 100
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync(eventEntity);

            // Act
            var result = await _eventService.GetEventDetailAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventEntity.Id, result.Id);
            Assert.Equal("Test Event", result.Name);
        }

        [Fact]
        public async Task GetEventDetailAsync_ReturnsNull_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync((Event?)null);

            // Act
            var result = await _eventService.GetEventDetailAsync(eventId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllEventsAsync_ReturnsListOfEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), Name = "Event 1", Description = "Description 1", Date = DateTime.UtcNow, Venue = "Venue 1", Capacity = 200 },
                new Event { Id = Guid.NewGuid(), Name = "Event 2", Description = "Description 2", Date = DateTime.UtcNow.AddDays(1), Venue = "Venue 2", Capacity = 300 }
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetAllEventsAsync())
                .ReturnsAsync(events);

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Name == "Event 1");
            Assert.Contains(result, e => e.Name == "Event 2");
        }

        [Fact]
        public async Task CreateEventAsync_ReturnsCreatedEvent()
        {
            // Arrange
            var eventRequest = new EventRequestDTO
            {
                Name = "New Event",
                Description = "New Event Description",
                Date = DateTime.UtcNow,
                Venue = "New Venue",
                Capacity = 500
            };

            _eventRepositoryMock
                .Setup(repo => repo.AddEventAsync(It.IsAny<Event>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.CreateEventAsync(eventRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventRequest.Name, result.Name);
            Assert.Equal(eventRequest.Venue, result.Venue);
            Assert.Equal(eventRequest.Capacity, result.Capacity);
        }

        [Fact]
        public async Task UpdateEventAsync_ReturnsUpdatedEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventRequest = new EventRequestDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                Date = DateTime.UtcNow,
                Venue = "Updated Venue",
                Capacity = 600
            };

            var existingEvent = new Event
            {
                Id = eventId,
                Name = "Old Event",
                Description = "Old Description",
                Date = DateTime.UtcNow,
                Venue = "Old Venue",
                Capacity = 500
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync(existingEvent);

            _eventRepositoryMock
                .Setup(repo => repo.UpdateEventAsync(It.IsAny<Event>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.UpdateEventAsync(eventId, eventRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventRequest.Name, result.Name);
            Assert.Equal(eventRequest.Venue, result.Venue);
            Assert.Equal(eventRequest.Capacity, result.Capacity);
        }

        [Fact]
        public async Task UpdateEventAsync_ReturnsNull_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventRequest = new EventRequestDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                Date = DateTime.UtcNow,
                Venue = "Updated Venue",
                Capacity = 600
            };

            _eventRepositoryMock
                .Setup(repo => repo.GetEventByIdAsync(eventId))
                .ReturnsAsync((Event?)null);

            // Act
            var result = await _eventService.UpdateEventAsync(eventId, eventRequest);

            // Assert
            Assert.Null(result);
        }
    }
}
