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
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var user = await _service.GetByUserAsync(userid);

            return Ok(user);
        }

        [HttpGet("wallet/{walletId}")]

        public async Task<IActionResult> WalletExistAsync(Guid walletId)
        {

            var wallet = await _service.WalletExistAsync(walletId);

            if (!wallet)
            {
                return NotFound(new { mensagem = "Wallet nao encontrada " });
            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("deposit/{userId}")]

        public async Task<IActionResult> DepositAsync(Guid userId, [FromBody] DepositWalletDto dto)
        {

            var wallet = await _service.DepositAsync(userId, dto.Amount);

            if (wallet == null)
            {
                return NotFound(new { mensagem = "Wallet nao encontrada" });

            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("draw/{userid}")]

        public async Task<IActionResult> WithDrawAsync(Guid userid, [FromBody] WithDrawWalletDto dto)
        {
            var wallet = await _service.WithDrawAsync(userid, dto.Amount);

            if (wallet == null)
            {
                return BadRequest(new { mensagem = "valor invalido " });
            }

            return Ok(wallet);
        }

        [EnableRateLimiting("register")]
        [Authorize]
        [HttpPost("transfer")]

        public async Task<IActionResult> TransferAsync([FromBody] TransferWalletDto dto)
        {

            var transfer = await _service.TransferAsync(dto);

            if (transfer == null)
            {
                return BadRequest(new { mensagem = "Usuario nao encontrado" });
            }

            return Ok(transfer);
        }
    }
}
