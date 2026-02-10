using Models.Transaction;
using Repository.Interfaces.Transactions;
using Dapper;
using System.Data;



namespace Repository.Transactions
{

    public class TransactionRepository : ITransactionRepository
    {
        private readonly DapperContext _context;

        public TransactionRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionByWalletAsync(Guid walletId, IDbConnection connection, IDbTransaction transaction)
        {



            const string sql = @"SELECT * FROM transactions WHERE wallet_id = @walletId

            ORDER BY created_at DESC";

            return await connection.QueryAsync<Transaction>(sql, new { WalletId = walletId }, transaction);

        }

        public async Task CreateTransactionAsync(Transaction transaction, IDbConnection connection, IDbTransaction transaction1)
        {



            const string sql = @"INSERT INTO transactions (id, wallet_id, amount, type, created_at)
            VALUES (@TransactionId, @WalletId, @Amount, @Type, @CreatedAt)";

            await connection.ExecuteAsync(sql, transaction, transaction1);
        }
    }
}
