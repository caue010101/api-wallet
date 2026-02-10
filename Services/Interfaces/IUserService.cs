using Dtos.User;

namespace Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<ReadUserDto?> GetUserByIdAsync(Guid id);
        Task<ReadUserDto?> GetUserByEmailAsync(string email);
        Task<ReadUserDto?> AddUserAsync(CreateUserDto userDto);
        Task DeleteUserAsync(Guid userId);
    }
}
