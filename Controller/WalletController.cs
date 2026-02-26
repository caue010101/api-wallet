using Services.Interfaces.Wallets;
using Microsoft.AspNetCore.Mvc;
using Dtos.Wallet;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace Controller.Wallets
{

    [ApiController]
    [Route("api/[controller]")]

    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        public WalletController(IWalletService walletService)
        {
            this._service = walletService;
        }

        [EnableRateLimiting("global")]
        [Authorize]
        [HttpGet("me")]

        public async Task<IActionResult> GetByUserAsync()
        {

            var userid = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = await _service.GetByUserAsync(userid);

            return Ok(user);
        }

        [HttpGet("wallet/{walletId}")]

        public async Task<IActionResult> WalletExistAsync(Guid walletId)
        {

            var wallet = await _service.WalletExistAsync(walletId);

            if (!wallet)
            {
                return NotFound(new { mensagem = "Wallet not found! " });
            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("deposit")]

        public async Task<IActionResult> DepositAsync([FromBody] DepositWalletDto dto)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var wallet = await _service.DepositAsync(userId, dto.Amount);

            if (wallet == null)
            {
                return NotFound(new { message = "Wallet not found " });
            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("draw")]

        public async Task<IActionResult> WithDrawAsync([FromBody] WithDrawWalletDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var wallet = await _service.WithDrawAsync(userId, dto.Amount);


            if (wallet == null)
            {
                return NotFound(new { message = "Wallet not found " });
            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("transfer")]

        public async Task<IActionResult> TransferAsync([FromBody] TransferWalletDto dto)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var fromUserId))
            {
                return Unauthorized();
            }

            var transfer = await _service.TransferAsync(fromUserId, dto);

            return Ok(transfer);
        }
    }
}
