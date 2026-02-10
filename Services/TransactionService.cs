using Dtos.Transaction;
using Services.Interfaces.Transactions;
using Repository.Interfaces.Transactions;
using Repository.Wallets;
using System.Data;
using Dapper;
using Repository.Interfaces.Wallets;

namespace Services.Transactions
{

    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionService> _logger;
        private readonly IWalletRepository _walletRepository;
        private readonly DapperContext _context;

        public TransactionService(ITransactionRepository transactionRepository, ILogger<TransactionService> logger,

            IWalletRepository walletRepository, DapperContext context)
        {
            this._transactionRepository = transactionRepository;
            this._logger = logger;
            this._walletRepository = walletRepository;
            this._context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionByWalletAsync(Guid walletId)
        {
            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                var walletExist = await _walletRepository.WalletExistAsync(walletId, conn, transaction);

                if (!walletExist)
                {
                    _logger.LogWarning("Wallet nao encontrada ");
                    return Enumerable.Empty<TransactionDto>();
                }

                var transactions = await _transactionRepository
                  .GetTransactionByWalletAsync(walletId, conn, transaction);

                transaction.Commit();


                return transactions.Select(t => new TransactionDto
                {
                    Id = t.TransactionId,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao obter as transaçoes da wallet {e.Message}");
                throw;
            }
        }

    }
}
