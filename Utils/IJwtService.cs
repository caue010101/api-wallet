using Model.Users;

namespace minhaApi.Utils.Interface
{

    public interface IJwtService
    {
        string GenerateToken(User User);
    }
}
