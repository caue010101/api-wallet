using Microsoft.AspNetCore.Mvc;
using minhaApi.Dtos.Auth;
using Services.Interfaces.Users;
using Microsoft.AspNetCore.RateLimiting;

namespace minhaApi.Controllers.Auth
{

    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            this._userService = userService;
        }

        [EnableRateLimiting("register")]
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {

            var user = await _userService.ValidateUserAsync(dto);

            if (user == null)
            {
                return Unauthorized("Credenciais invalidas ");
            }

            return Ok(user);
        }
    }


}
