using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ConcertTicketAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<User?> RegisterAsync(string username, string password)
        {
            if (await _userRepository.UserExistsAsync(username))
                return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _userRepository.CreateUserAsync(user);
            return user;
        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(Guid userId)
        {
            return await _userRepository.GetUserTicketsAsync(userId);
        }
    }
}
