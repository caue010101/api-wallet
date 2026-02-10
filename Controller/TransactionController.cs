using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;

namespace Controller.Transactions
{

    [ApiController]
    [Route("api/[controller]")]

    public class TransactionController : ControllerBase
    {

        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transaction)
        {
            this._transactionService = transaction;
        }

        [HttpGet("user/{userId}")]

        public async Task<IActionResult> GetTransactionByWalletAsync(Guid userId)
        {

            var wallet = await _transactionService.GetTransactionByWalletAsync(userId);

            if (wallet == null)
            {
                return NotFound(new { mensagem = "wallet nao encontrada " });
            }

            return Ok(wallet);
        }
    }
}
