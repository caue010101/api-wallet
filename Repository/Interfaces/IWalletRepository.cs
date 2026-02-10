using Model.Wallet;
using System.Data;

namespace Repository.Interfaces.Wallets
{
    public interface IWalletRepository
    {
        Task AddWalletAsync(Wallet wallet, IDbConnection connection, IDbTransaction transaction);
        Task<Wallet?> GetByUserAsync(Guid UserId, IDbConnection connection, IDbTransaction transaction);
        Task<bool> WalletExistAsync(Guid walletId, IDbConnection connection, IDbTransaction transaction);
        Task UpdateBalanceAsync(Guid walletId, decimal newBalance, IDbConnection connection, IDbTransaction transaction);
    }
}
