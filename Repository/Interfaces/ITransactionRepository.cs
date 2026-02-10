using Models.Transaction;
using System.Data;

namespace Repository.Interfaces.Transactions
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionByWalletAsync(Guid WalletId, IDbConnection connection, IDbTransaction transaction);
        Task CreateTransactionAsync(Transaction transaction, IDbConnection connection, IDbTransaction transaction1);
    }
}
