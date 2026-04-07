using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.DTO;
using m_motors_API.Services;

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

        //---------------------------------
        // Login client
        //---------------------------------
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
                client.IdClient
            );

            return Ok(new { token });
        }

        //---------------------------------
        // REGISTER CLIENT
        //---------------------------------
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
                client.IdClient
            );

            return Ok(new { token });
        }

        //---------------------------------
        // LOGIN BACK-OFFICE
        //---------------------------------
        [HttpPost("backoffice/login")]
        public IActionResult LoginBackOffice([FromBody] BackOfficeLoginRequest request)
        {
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Utilisateur invalide" });
            }

            var roleName = user.Role?.NomRole ?? "Unknown";

            var token = _tokenService.GenerateToken(
                user.Email,
                roleName,
                "BackOffice",
                user.IdUser
            );

            return Ok(new { token });
        }
    }
}
