using Services.Interfaces.Wallets;
using Microsoft.AspNetCore.Mvc;
using Dtos.Wallet;

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

        [HttpGet("{userid}")]

        public async Task<IActionResult> GetByUserAsync(Guid userId)
        {

            var wallet = await _service.GetByUserAsync(userId);

            if (wallet == null)
            {
                return NotFound(new { mensagem = "Wallet nao encontrada " });
            }

            return Ok(wallet);
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

        [HttpPut("user/{userId}")]

        public async Task<IActionResult> UpdateBalanceAsync(Guid userId, [FromBody] UpdateWalletDto dto)
        {

            var wallet = await _service.UpdateBalanceAsync(userId, dto);

            if (wallet == null)
            {
                return NotFound(new { mensagem = "Carteira nao encontrada " });
            }
            return Ok(new { mensagem = "Carteira atualizada com sucesso " });
        }

        [HttpPost("deposit/{userId}")]

        public async Task<IActionResult> DepositAsync(Guid userId, [FromBody] decimal amount)
        {

            var wallet = await _service.DepositAsync(userId, amount);

            if (wallet == null)
            {
                return NotFound(new { mensagem = "Wallet nao encontrada" });

            }

            return Ok(wallet);
        }

        [HttpPost("draw/{userid}")]

        public async Task<IActionResult> WithDrawAsync(Guid userid, [FromBody] decimal amount)
        {
            var wallet = await _service.WithDrawAsync(userid, amount);

            if (wallet == null)
            {
                return BadRequest(new { mensagem = "valor invalido " });
            }

            return Ok(wallet);
        }

        [HttpPost("transfer")]

        public async Task<IActionResult> TransferAsync([FromBody] TransferWalletDto dto)
        {

            var transfer = await _service.TransferAsync(dto);

            if (transfer == null)
            {
                return BadRequest(new { mensagem = "Wallet nao encontrada " });
            }

            return Ok(transfer);
        }
    }
}
