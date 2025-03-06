using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Moq;
using Xunit;

namespace ConcertTicketAPI.Tests.Services;

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
    public async Task GetEventByIdAsync_ReturnsEvent_WhenEventExists()
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
        var result = await _eventService.GetEventByIdAsync(eventId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventEntity.Id, result.Id);
        Assert.Equal("Test Event", result.Name);
    }

    [Fact]
    public async Task GetEventByIdAsync_ReturnsNull_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        _eventRepositoryMock
            .Setup(repo => repo.GetEventByIdAsync(eventId))
            .ReturnsAsync((Event?)null);

        // Act
        var result = await _eventService.GetEventByIdAsync(eventId);

        // Assert
        Assert.Null(result);
    }
}