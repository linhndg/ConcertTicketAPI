using ConcertTicketAPI.DTO;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAuthRequest request)
    {
        var user = await _userService.RegisterAsync(request.Username, request.Password);
        if (user == null)
            return BadRequest("User already exists.");

        return Ok(new { user.Id, user.Username });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthRequest request)
    {
        var token = await _userService.AuthenticateAsync(request.Username, request.Password);
        if (token == null)
            return Unauthorized("Invalid username or password.");

        return Ok(new { Token = token });
    }
}

