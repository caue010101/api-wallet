using Dtos.User;
using minhaApi.Dtos.Auth;

namespace Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<ReadUserDto?> GetUserByIdAsync(Guid id);
        Task<ReadUserDto?> GetUserByEmailAsync(string email);
        Task<ReadUserDto?> AddUserAsync(CreateUserDto userDto);
        Task<LoginResponseDto> ValidateUserAsync(LoginRequestDto dto);
    }
}
