using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ConcertTicketAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var configurationData = new Dictionary<string, string>
            {
                {"Jwt:Key", "f442ec5fd85ab4ed363b57f674b868613e0c9895c15d2a0a00a18863726400f7b1e445045a85d8035ecff8cc41759b5f8e5f1a9b927e433a79253eb0e170ca3d7552b00f3cee81b9e86cc5774c2c80534c248fe4e173c0dd5fc5e081f07b761827a34a491d077ce87263d180562e66ddfea5e98ac0a82ea1631b75112b0a2316fe765d7f20eff981c25a47929158fc44d110bd4753abb7468c668f7c8879b44d43eabd749e4b6ddb2f6a30c51cd786d96c5f69b92e7cb2133c06d564295dec36d7dfb34d94a1b9dad7c49d4c6729a434f0dad2cc361fdd8a6606aeacf7024d72380cbe88dba05fd148b2053861ea94729f23c283bc3edd520d4904ab15a8063b"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns(configurationData["Jwt:Key"]);
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns(configurationData["Jwt:Issuer"]);
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns(configurationData["Jwt:Audience"]);

            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnUser_WhenUserDoesNotExist()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";

            _userRepositoryMock.Setup(r => r.UserExistsAsync(username))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.RegisterAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            _userRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnNull_WhenUserAlreadyExists()
        {
            // Arrange
            string username = "existinguser";
            string password = "password123";

            _userRepositoryMock.Setup(r => r.UserExistsAsync(username))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterAsync(username, password);

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            string username = "validuser";
            string password = "validpassword";
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);

            // Act
            var token = await _userService.AuthenticateAsync(username, password);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            string username = "invaliduser";
            string password = "wrongpassword";

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username))
                .ReturnsAsync((User?)null);

            // Act
            var token = await _userService.AuthenticateAsync(username, password);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task GetUserTicketsAsync_ShouldReturnTickets_ForGivenUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var tickets = new List<Ticket>
            {
                new Ticket { Id = Guid.NewGuid(), UserId = userId, Status = TicketStatus.Purchased },
                new Ticket { Id = Guid.NewGuid(), UserId = userId, Status = TicketStatus.Purchased }
            };

            _userRepositoryMock.Setup(r => r.GetUserTicketsAsync(userId))
                .ReturnsAsync(tickets);

            // Act
            var result = await _userService.GetUserTicketsAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
