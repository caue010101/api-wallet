using Dapper;
using Repository.Interfaces.Wallets;
using Model.Wallet;
using System.Data;

namespace Repository.Wallets
{
    public class WalletRepository : IWalletRepository
    {
        private readonly DapperContext _context;

        public WalletRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<Wallet?> GetByUserAsync(Guid userId, IDbConnection connection, IDbTransaction transaction)
        {

            const string sql = @"SELECT 
                id,
                user_id AS UserId,
                balance,
                created_at AS CreatedAt
                  FROM wallets
                  WHERE user_id = @UserId
                ";

            return connection.QueryFirstOrDefault<Wallet?>(sql, new { UserId = userId }, transaction);
        }

        public async Task<bool> WalletExistAsync(Guid walletId, IDbConnection connection, IDbTransaction transaction)
        {

            const string sql = @"SELECT * FROM wallets WHERE wallet_id = @walletId";

            var result = await
              connection.ExecuteScalarAsync<int?>(sql, new { WalletId = walletId }, transaction);

            return result.HasValue;
        }

        public async Task AddWalletAsync(Wallet wallet, IDbConnection connection, IDbTransaction transaction)
        {

            const string sql = @"INSERT INTO wallets (id, user_id, balance, created_at )

            VALUES(@Id, @UserId, @Balance, @CreatedAt)";

            await connection.ExecuteAsync(sql, wallet, transaction);
        }

        public async Task UpdateBalanceAsync(Guid walletId, decimal newBalance, IDbConnection connection, IDbTransaction transaction)
        {

            const string sql = @"UPDATE wallets SET balance = @Balance WHERE id = @WalletId";

            await connection.ExecuteAsync(sql, new { WalletId = walletId, Balance = newBalance }, transaction);
        }
    }
}
