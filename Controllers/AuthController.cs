using m_motors_API.Data;
using m_motors_API.DTO;
using m_motors_API.Models;
using m_motors_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LoginRequest = m_motors_API.DTO.LoginRequest;
using RegisterRequest = m_motors_API.DTO.RegisterRequest;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MMotorsContext _context;
        private readonly TokenService _tokenService;

        public AuthController(
            MMotorsContext context,
            TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // PING temporaire pour tester le controller
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("OK");
        }

        // Test temporaire pour vérifier que le controller est bien fonctionnel
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("AuthController OK");
        }

        // LOGIN CLIENT- Récupère le client dans la table Clients, vérifie le mot de passe et génère un token avec les infos nécessaires
        [HttpPost("client/login")]
        public IActionResult LoginClient([FromBody] ClientLoginRequest request)
        {
            var client = _context.Clients
                .FirstOrDefault(c => c.Email == request.Email);

            if (client == null || !BCrypt.Net.BCrypt.Verify(request.Password, client.Password))
            {
                return Unauthorized(new { message = "Client invalide" });
            }

            var token = _tokenService.GenerateToken(
                client.Email,
                "Client",
                "Client",
                client.IdClient,
                client.Prenom,
                client.Nom
            );

            return Ok(new { token });
        }

        // Créer un client (inscription)
        [HttpPost("client/register")]
        public IActionResult RegisterClient([FromBody] RegisterRequest request)
        {
            if (_context.Clients.Any(c => c.Email == request.Email))
                return BadRequest(new { message = "Email déjà utilisé" });

            var client = new Models.Client
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Telephone = request.Telephone,
                Adresse = request.Adresse
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            var token = _tokenService.GenerateToken(
                client.Email,
                "Client",
                "Client",
                client.IdClient,
                client.Prenom,
                client.Nom
            );

            return Ok(new { token });
        }

        // LOGIN BACK-OFFICE - Récupère l'utilisateur dans la table Utilisateurs, vérifie le mot de passe et génère un token avec le rôle et les infos nécessaires
        [HttpPost("backoffice/login")]
        public IActionResult LoginBackOffice([FromBody] BackOfficeLoginRequest request)
        {
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == request.Email);

            Console.WriteLine("EMAIL: " + request.Email);
            Console.WriteLine("PASSWORD CHECK START");
            Console.WriteLine($"HASH EXISTS: {!string.IsNullOrEmpty(user?.Password)}");

            Console.WriteLine("USER FOUND: " + (user != null));

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Utilisateur invalide" });
            }

            var roleName = user.Role?.NomRole ?? "Unknown";

            var token = _tokenService.GenerateToken(
                user.Email,
                roleName,
                "BackOffice",
                user.IdUser,
                "",
                ""
            );
            return Ok(new { token });
        }

        // GET CURRENT CLIENT (/me) - Récupère les informations du client actuellement connecté
        [Authorize]
        [HttpGet("client/me")]
        public IActionResult GetCurrentClient()
        {
            var userIdClaim = User.FindFirst("nameid")?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token invalide ou userId manquant" });
            }

            if (!int.TryParse(userIdClaim.Value, out int clientId))
            {
                return Unauthorized(new { message = "UserId invalide" });
            }

            var client = _context.Clients.FirstOrDefault(c => c.IdClient == clientId);

            if (client == null)
            {
                return NotFound(new { message = "Client introuvable" });
            }

            return Ok(new
            {
                id = client.IdClient,
                nom = client.Nom,
                prenom = client.Prenom,
                email = client.Email,
                telephone = client.Telephone,
                adresse = client.Adresse
            });
        }

        // LOGIN GÉNÉRAL (client ou back-office) - Récupère l'utilisateur dans la table Clients ou Utilisateurs, vérifie le mot de passe et génère un token avec les infos nécessaires
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // CLIENT
            var client = _context.Clients
                .FirstOrDefault(c => c.Email == request.Email);

            if (client != null &&
                BCrypt.Net.BCrypt.Verify(request.Password, client.Password))
            {
                var token = _tokenService.GenerateToken(
                    client.Email,
                    "Client",
                    "Client",
                    client.IdClient,
                    client.Prenom,
                    client.Nom
                );

                return Ok(new
                {
                    token,
                    user = new
                    {
                        id = client.IdClient,
                        nom = client.Nom,
                        prenom = client.Prenom,
                        email = client.Email,
                        role = "Client"
                    }
                });
            }

            // BACKOFFICE
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == request.Email);

            if (user != null &&
                BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                var roleName = user.Role?.NomRole ?? "Unknown";

                var token = _tokenService.GenerateToken(
                    user.Email,
                    roleName,
                    "BackOffice",
                    user.IdUser,
                    "", // pas de prenom actuellement
                    user.Nom
                );

                return Ok(new
                {
                    token,
                    user = new
                    {
                        id = user.IdUser,
                        nom = user.Nom,
                        prenom = "", // ⚠️ à améliorer plus tard
                        email = user.Email,
                        role = roleName
                    }
                });
            }
            return Unauthorized(new { message = "Identifiants invalides" });
        }
    }
}
