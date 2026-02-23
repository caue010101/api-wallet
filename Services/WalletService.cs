using Services.Interfaces.Wallets;
using Repository.Interfaces.Wallets;
using Dtos.Wallet;
using Repository.Interfaces.Transactions;
using Models.Transaction;
using static Enums.Transaction.TransactionType;


namespace Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ILogger<WalletService> _logger;
        private readonly DapperContext _context;
        private readonly ITransactionRepository _transactionRepository;

        public WalletService(IWalletRepository repository, ILogger<WalletService> logger,
            DapperContext context, ITransactionRepository transactionRepository)
        {
            this._walletRepository = repository;
            this._logger = logger;
            this._context = context;
            this._transactionRepository = transactionRepository;
        }

        public async Task<ReadWalletDto?> GetByUserAsync(Guid UserId)
        {
            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                var wallet = await _walletRepository.GetByUserAsync(UserId, conn, transaction);

                if (wallet == null)
                {
                    return null;
                }

                var dto = new ReadWalletDto
                {
                    Id = wallet.Id,
                    Balance = wallet.Balance,
                    CreatedAt = DateTime.UtcNow
                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Erro ao encontrar a wallet {e.Message}");
                return null;
            }
        }

        public async Task<bool> WalletExistAsync(Guid walletId)
        {

            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                var wallet = await _walletRepository.WalletExistAsync(walletId, conn, transaction);

                if (!wallet)
                {
                    _logger.LogWarning("Wallet nao encontrada ");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro na busca da wallet {e.Message}");
                return false;
            }
        }

        public async Task<ReadWalletDto?> UpdateBalanceAsync(Guid userId, UpdateWalletDto walletDto)
        {
            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                var wallet = await _walletRepository.GetByUserAsync(userId, conn, transaction);

                if (wallet == null)
                {
                    throw new ArgumentNullException("Wallet nao encontrada ");
                }

                if (walletDto.Balance < 0)
                {
                    throw new InvalidOperationException("Valor invalido ");
                }

                await _walletRepository.UpdateBalanceAsync(userId, walletDto.Balance, conn, transaction);

                var dto = new ReadWalletDto
                {
                    Id = wallet.Id,
                    Balance = walletDto.Balance,
                };
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar a wallet {e.Message}");
                transaction.Rollback();
                throw;
            }
        }

        public async Task<ReadWalletDto?> DepositAsync(Guid userId, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("O valor do deposito precisa ser maior que 0 ");
            }

            using var conn = _context.CreateConnection();
            conn.Open();

            using var transaction = conn.BeginTransaction();


            try
            {
                var wallet = await _walletRepository.GetByUserAsync(userId, conn, transaction);

                if (wallet == null)
                {
                    return null;
                }

                wallet.Balance += amount;

                await _walletRepository.UpdateBalanceAsync(
                    wallet.Id,
                    wallet.Balance,
                    conn,
                    transaction
                );

                var transactionEntity = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    WalletId = wallet.Id,
                    Amount = amount,
                    Type = Deposit,
                    CreatedAt = DateTime.UtcNow
                };

                await _transactionRepository.CreateTransactionAsync(transactionEntity, conn, transaction);

                transaction.Commit();


                var dto = new ReadWalletDto
                {
                    Id = wallet.Id,
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    CreatedAt = DateTime.UtcNow

                };

                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao fazer o deposito {e.Message}");
                transaction.Rollback();
                throw;
            }
        }

        public async Task<ReadWalletDto?> WithDrawAsync(Guid userId, decimal amount)
        {

            if (amount <= 0)
            {
                throw new InvalidOperationException("O saque precisa ser maior que 0 ");
            }


            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {


                var wallet = await _walletRepository.GetByUserAsync(userId, conn, transaction);

                if (wallet == null)
                {
                    return null;
                }

                if (wallet.Balance < amount)
                {
                    throw new ArgumentException("Saldo insuficiente ");
                }

                wallet.Balance -= amount;

                await _walletRepository.UpdateBalanceAsync(

                    wallet.Id,
                    wallet.Balance,
                    conn,
                    transaction
                );

                var transactionEntity = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    WalletId = wallet.Id,
                    Amount = amount,
                    Type = Saque,
                    CreatedAt = DateTime.UtcNow
                };



                await _transactionRepository.CreateTransactionAsync(transactionEntity, conn, transaction);

                transaction.Commit();

                var dto = new ReadWalletDto
                {
                    Id = wallet.Id,
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    CreatedAt = DateTime.UtcNow
                };

                return dto;

            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao fazer o saque {e.Message}");
                transaction.Rollback();
                throw;
            }
        }

        public async Task<TransferResultDto> TransferAsync(TransferWalletDto dto)
        {

            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                if (dto.FromUserId == dto.ToUserId)
                {
                    throw new InvalidOperationException("Voce nao pode transferir pra si mesmo ");
                }

                if (dto.Amount <= 0)
                {

                    throw new InvalidOperationException("O valor precisa ser maior que 0 ");
                }

                var wallet1 = await _walletRepository.GetByUserAsync(dto.FromUserId, conn, transaction);

                if (wallet1?.Balance < dto.Amount)
                {
                    throw new ArgumentException("Saldo insuficiente ");
                }
                var wallet2 = await _walletRepository.GetByUserAsync(dto.ToUserId, conn, transaction);

                if (wallet1 == null || wallet2 == null)
                {
                    throw new ArgumentException("Erro ao encontrar a wallet ");
                }

                wallet1.Balance -= dto.Amount;
                wallet2.Balance += dto.Amount;

                await _walletRepository.UpdateBalanceAsync(wallet1.Id, wallet1.Balance, conn, transaction);
                await _walletRepository.UpdateBalanceAsync(wallet2.Id, wallet2.Balance, conn, transaction);


                var debitTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    WalletId = wallet1.Id,
                    Amount = dto.Amount,
                    Type = Saque,
                    CreatedAt = DateTime.UtcNow
                };

                await _transactionRepository.CreateTransactionAsync(debitTransaction, conn, transaction);

                var creditTransaction = new Transaction
                {
                    TransactionId = Guid.NewGuid(),
                    WalletId = wallet2.Id,
                    Amount = dto.Amount,
                    Type = Deposit,
                    CreatedAt = DateTime.UtcNow
                };

                await _transactionRepository.CreateTransactionAsync(creditTransaction, conn, transaction);

                transaction.Commit();

                var result = new TransferResultDto
                {
                    FromUserId = wallet1.Id,
                    ToUserId = wallet2.Id,
                    Amount = dto.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao realizar a transferencia para {UserId}", dto.ToUserId);
                transaction.Rollback();
                throw;
            }
        }
    }
}
