using Dtos.Transaction;


namespace Services.Interfaces.Transactions
{

    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionByWalletAsync(Guid walletId);
    }
}
