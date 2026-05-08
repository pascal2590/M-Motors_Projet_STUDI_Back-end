using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;
using m_motors_API.DTO;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Administrateur")]
    public class AdminController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public AdminController(MMotorsContext context)
        {
            _context = context;
        }

        // Création du commercial
        [HttpPost("create-commercial")]
        public IActionResult CreateCommercial([FromBody] CreateCommercialRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email et mot de passe requis" });
            }

            // Vérifier si email existe
            if (_context.Utilisateurs.Any(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email déjà utilisé" });
            }

            // Récupérer le rôle Commercial
            var role = _context.Roles.FirstOrDefault(r => r.NomRole == "Commercial");

            if (role == null)
            {
                return BadRequest(new { message = "Rôle Commercial introuvable" });
            }

            var user = new Utilisateur
            {
                Nom = request.Nom,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.IdRole
            };

            _context.Utilisateurs.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Commercial créé avec succès" });
        }

        // lister les commerciaux
        [HttpGet("commerciaux")]
        public IActionResult GetCommerciaux()
        {
            var commerciaux = _context.Utilisateurs
                .Include(u => u.Role)
                .Where(u => u.Role.NomRole == "Commercial")
                .Select(u => new
                {
                    u.IdUser,
                    u.Nom,
                    u.Email
                })
                .ToList();

            return Ok(commerciaux);
        }

        // Suppression d'un commercial 
        [HttpDelete("commercial/{id}")]
        public IActionResult DeleteCommercial(int id)
        {
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound(new { message = "Utilisateur introuvable" });
            }

            if (user.Role?.NomRole != "Commercial")
            {
                return BadRequest(new { message = "Suppression autorisée uniquement pour les commerciaux" });
            }

            _context.Utilisateurs.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = "Commercial supprimé" });
        }

        // Obtenir les détails d'un utilisateur (commercial ou autre) par son ID
        [HttpGet("user/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .Where(u => u.IdUser == id)
                .Select(u => new
                {
                    u.IdUser,
                    u.Nom,
                    u.Email,
                    role = u.Role.NomRole
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound(new { message = "Utilisateur introuvable" });
            }

            return Ok(user);
        }

        // LISTE DE TOUS LES UTILISATEURS
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Utilisateurs
                .Include(u => u.Role)
                .Select(u => new
                {
                    id = u.IdUser,
                    nom = u.Nom,
                    email = u.Email,
                    role = u.Role.NomRole
                })
                .ToList();

            return Ok(users);
        }

        // Mise à jour d'un commercial
        [HttpPut("commercial/{id}")]
        public IActionResult UpdateCommercial(int id, [FromBody] UpdateCommercialRequest request)
        {
            var user = _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound(new { message = "Utilisateur introuvable" });
            }

            if (user.Role?.NomRole != "Commercial")
            {
                return BadRequest(new { message = "Modification autorisée uniquement pour les commerciaux" });
            }

            user.Nom = request.Nom ?? user.Nom;
            user.Email = request.Email ?? user.Email;

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            _context.SaveChanges();

            return Ok(new { message = "Commercial mis à jour" });
        }
    }
}
