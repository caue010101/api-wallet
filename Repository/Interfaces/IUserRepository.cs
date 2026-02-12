using Model.Users;
using System.Data;

namespace Repository.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User? user, IDbConnection conn, IDbTransaction tran);
        Task<IEnumerable<User>> GetUsersAsync(int Page, int pageSize, IDbConnection connection);
    }
}
