using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConcertTicketAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new UnauthorizedAccessException("User ID not found or invalid.");

            return userId;
        }
    }
}