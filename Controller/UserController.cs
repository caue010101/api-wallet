using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Users;
using Dtos.User;
using Microsoft.AspNetCore.RateLimiting;

namespace Controller.Users

{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [EnableRateLimiting("global")]
        [HttpGet("{id:guid}", Name = "GetUserById")]

        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { mensagem = "Usuario nao encontrado " });

            }

            return Ok(user);
        }


        [EnableRateLimiting("global")]
        [HttpGet("by-email/{email}")]

        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { mensagem = "Usuario nao encontrado " });
            }

            return Ok(user);

        }

        [EnableRateLimiting("register")]
        [HttpPost]

        public async Task<IActionResult> AddUserAsync([FromBody] CreateUserDto userDto)
        {

            var user = await _userService.AddUserAsync(userDto);

            if (user == null)
            {
                return BadRequest(new { mensagem = "Erro no cadastro do usuario, verifique os dados " });
            }

            return CreatedAtAction("GetUserById", new { id = user.UserId }, user);
        }
    }
}
