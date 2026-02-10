using Model.Users;
using System.Data;

namespace Repository.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User? user, IDbConnection conn, IDbTransaction tran);
        Task DeleteUserAsync(Guid userId);
    }
}
