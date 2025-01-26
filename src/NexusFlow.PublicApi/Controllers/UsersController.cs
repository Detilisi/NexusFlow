using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        [HttpGet("validate-user")]
        [Authorize] // Ensure this endpoint requires authentication
        public IActionResult ValidateUser()
        {
            // Return a success response if the user is authenticated
            return Ok(new { Message = "User is authenticated." });
        }
    }
}
