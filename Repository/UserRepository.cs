using Repository.Interfaces.Users;
using Dapper;
using System.Data;
using Model.Users;

namespace Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            using var conn = _context.CreateConnection();
            const string sql = @"SELECT * FROM users WHERE id = @Id
              RETURNING id";

            return await conn.QueryFirstOrDefaultAsync<User?>(sql, new { id = id });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {

            using var conn = _context.CreateConnection();

            const string sql = @"SELECT * FROM users WHERE email = @Email";

            return await conn.QueryFirstOrDefaultAsync<User?>(sql, email);
        }

        public async Task AddUserAsync(User? user, IDbConnection connection, IDbTransaction tran)
        {

            const string sql = @"INSERT INTO users (id, name, email, password_hash, created_at)
              VALUES(@UserId, @Name, @Email, @PasswordHash, @CreatedAt)";

            await connection.ExecuteAsync(sql, user, tran);
        }
    }
}
