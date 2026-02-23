using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using minhaApi.Utils.Interface;
using Model.Users;
using System.Security.Claims;

namespace minhaApi.Utils
{
    public class JwtService : IJwtService
    {

        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            this._config = config;
        }

        public string GenerateToken(User user)
        {

            var claims = new Claim[]{

            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
          };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var audience = _config["Jwt:Audience"];
            var issuer = _config["Jwt:Issuer"];

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken(
                claims: claims,
                audience: audience,
                issuer: issuer,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
