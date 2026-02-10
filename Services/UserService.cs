using Services.Interfaces.Users;
using Repository.Interfaces.Users;
using Dtos.User;
using Repository.Interfaces.Wallets;
using Model.Wallet;
using Npgsql;
using Model.Users;

namespace Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IWalletRepository _walletRepository;
        private readonly DapperContext _context;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IWalletRepository walletRepository,
            DapperContext context)
        {
            this._userRepository = userRepository;
            this._walletRepository = walletRepository;
            this._logger = logger;
            this._context = context;
        }

        public async Task<ReadUserDto?> GetUserByIdAsync(Guid id)
        {
            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {

                var user = await _userRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogInformation("Usuario nao encontrado");
                    return null;
                }

                var dto = new ReadUserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                };

                return dto;

            }
            catch (Exception e)
            {
                _logger.LogError($"Erro na busca do usuario pelo id {e.Message}");
                return null;
            }
        }

        public async Task<ReadUserDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("Usuario nao encontrado ");
                    return null;
                }

                var dto = new ReadUserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                };

                return dto;
            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao buscar o usuario pelo email {e.Message}");
                return null;

            }
        }

        public async Task<ReadUserDto?> AddUserAsync(CreateUserDto userDto)
        {
            using var conn = _context.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var model = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = userDto.Name,
                    PasswordHash = passwordHash,
                    Email = userDto.Email,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddUserAsync(model, conn, transaction);

                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = model.UserId,
                    Balance = 0m,
                    CreatedAt = DateTime.UtcNow
                };


                await _walletRepository.AddWalletAsync(wallet, conn, transaction);

                transaction.Commit();

                var dto = new ReadUserDto
                {
                    UserId = model.UserId,
                    Name = model.Name,
                    Email = model.Email,
                    CreatedAt = model.CreatedAt
                };

                return dto;
            }

            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                transaction.Rollback();
                throw new InvalidOperationException("Email ja cadastrado ");
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError($"Erro no cadastro do usuario {e.Message}");
                return null;
            }

        }

        public async Task DeleteUserAsync(Guid userId)
        {
            try
            {
                await _userRepository.DeleteUserAsync(userId);

                _logger.LogInformation("Usuario {UserId} deletado com sucesso ", userId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao deletar o usuario {UserId}", userId);
                throw;
            }
        }
    }
}
