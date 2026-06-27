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
            int userId,
            string? prenom,
            string? nom)
        {
            var secret = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("Jwt : La clé Jwt:Key est manquante dans la configuration");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                throw new Exception("Jwt : Le paramètre Issuer est manquant dans la configuration");
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


            // CLAIMS JWT
            var claims = new[]
            {
                // identité
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("Type", typeUtilisateur),

                // id utilisateur
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

                // identité utilisateur (client uniquement)
                new Claim(ClaimTypes.GivenName, prenom ?? ""),
                new Claim(ClaimTypes.Surname, nom ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
