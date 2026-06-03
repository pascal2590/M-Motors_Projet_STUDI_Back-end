using m_motors_API.Data;
using m_motors_API.DTO;
using m_motors_API.DTO.Logs;
using m_motors_API.Models;
using m_motors_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Administrateur")]
    public class AdminController : ControllerBase
    {
        // Contexte de la base de données pour accéder aux données des utilisateurs, des rôles et des logs
        private readonly MMotorsContext _context;

        // Service de gestion des logs pour permettre à l'administrateur de consulter les logs d'activité et d'erreur de l'application
        private readonly ILogService _logService;

        // Constructeur pour injecter les dépendances nécessaires, comme le contexte de la base de données et le service de logs
        public AdminController( MMotorsContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        // Endpoint pour récupérer les statistiques des logs, comme le nombre total de logs, le nombre d'erreurs, les logs du jour, etc.
        [HttpGet("logs/stats")]
        public IActionResult GetLogStats()
        {
            var today = DateTime.Today;

            var result = new
            {
                totalLogs = _context.ApplicationLogs.Count(),
                totalErrors = _context.ApplicationLogs.Count(l => l.Niveau == "ERROR"),
                logsToday = _context.ApplicationLogs.Count(l => l.DateLog >= today),
                errorsToday = _context.ApplicationLogs.Count(l => l.Niveau == "ERROR" && l.DateLog >= today)
            };

            return Ok(result);
        }

        // Endpoint pour récupérer les logs avec des filtres
        [HttpPost("logs")]
        public async Task<IActionResult> GetLogs([FromBody] LogFilterRequest? filter)
        {
            filter ??= new LogFilterRequest();
            var logs = await _logService.GetLogsAsync(filter);
            return Ok(logs);
        }

        // Création du commercial
        [HttpPost("create-commercial")]
        public async Task<IActionResult> CreateCommercialAsync([FromBody] CreateCommercialRequest request)
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
                Prenom = request.Prenom,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.IdRole
            };

            _context.Utilisateurs.Add(user);
            _context.SaveChanges();

            await _logService.LogInfoAsync(
                $"Création du commercial : {user.Email}",
                User.Identity?.Name,
                endpoint: HttpContext.Request.Path,
                methodeHttp: HttpContext.Request.Method
            );

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
                    u.Prenom,
                    u.Email
                })
                .ToList();

            return Ok(commerciaux);
        }

        // Suppression d'un commercial 
        [HttpDelete("commercial/{id}")]
        public async Task<IActionResult> DeleteCommercialAsync(int id)
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

            await _logService.LogInfoAsync(
                $"Suppression du commercial : {user.Email}",
                User.Identity?.Name,
                endpoint: HttpContext.Request.Path,
                methodeHttp: HttpContext.Request.Method
            );

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
                    prenom = u.Prenom, // Ajouté le 14/05/2026
                    email = u.Email,
                    role = u.Role.NomRole
                })
                .ToList();

            return Ok(users);
        }

        // Mise à jour d'un commercial
        [HttpPut("commercial/{id}")]
        public async Task<IActionResult> UpdateCommercialAsync(int id, [FromBody] UpdateCommercialRequest request)
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

            await _logService.LogInfoAsync(
                $"Modification du commercial : {user.Email}",
                User.Identity?.Name,
                endpoint: HttpContext.Request.Path,
                methodeHttp: HttpContext.Request.Method
            );

            return Ok(new { message = "Commercial mis à jour" });
        }
    }
}
