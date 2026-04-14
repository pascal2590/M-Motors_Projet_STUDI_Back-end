using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace m_motors_API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(
            string email,
            string role,
            string typeUtilisateur,
            int userId)
        {
            var secret = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("Type", typeUtilisateur),

                // IMPORTANT
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,

                claims: claims,

                expires: DateTime.UtcNow.AddHours(2),

                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
