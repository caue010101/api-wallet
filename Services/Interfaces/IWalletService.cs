using Dtos.Wallet;

namespace Services.Interfaces.Wallets
{
    public interface IWalletService
    {
        Task<ReadWalletDto?> GetByUserAsync(Guid UserId);
        Task<bool> WalletExistAsync(Guid walletId);
        Task<ReadWalletDto?> DepositAsync(Guid userId, decimal amount);
        Task<ReadWalletDto?> WithDrawAsync(Guid userId, decimal amount);
        Task<ReadWalletDto?> UpdateBalanceAsync(Guid userId, UpdateWalletDto dto);
    }
}
