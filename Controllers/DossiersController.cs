using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.DTO;
using m_motors_API.Models;
using m_motors_API.Data;
using m_motors_API.Enums;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/dossiers")]
    public class DossiersController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public DossiersController(MMotorsContext context)
        {
            _context = context;
        }

        // =====================================================
        // VALIDATION VEHICULE + CLIENT (helper interne)
        // =====================================================
        private (Client? client, Vehicule? vehicule, IActionResult? error) Validate(int clientId, int vehiculeId)
        {
            var client = _context.Clients.FirstOrDefault(c => c.IdClient == clientId);
            if (client == null)
                return (null, null, NotFound("Client introuvable"));

            var vehicule = _context.Vehicules.FirstOrDefault(v => v.IdVehicule == vehiculeId);
            if (vehicule == null)
                return (null, null, NotFound("Vehicule introuvable"));

            if (!vehicule.Disponible)
                return (null, null, BadRequest("Vehicule non disponible"));

            var already = _context.Dossiers.Any(d => d.VehiculeId == vehiculeId);
            if (already)
                return (null, null, BadRequest("Vehicule deja utilise dans un dossier"));

            return (client, vehicule, null);
        }

        // =====================================================
        // ACHAT
        // =====================================================
        [HttpPost("achat")]
        public IActionResult CreateAchat([FromBody] DossierAchatDto dto)
        {
            var (client, vehicule, error) = Validate(dto.ClientId, dto.VehiculeId);
            if (error != null) return error;

            var dossier = new Dossier
            {
                TypeDossier = TypeDossier.achat,
                Statut = StatutDossier.en_attente,
                DateCreation = DateTime.Now,
                ClientId = dto.ClientId,
                VehiculeId = dto.VehiculeId
            };

            _context.Dossiers.Add(dossier);
            _context.SaveChanges();

            vehicule!.Disponible = false;
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Dossier créé avec succès",
                data = dossier
            });

        }

        // =====================================================
        // LLD
        // =====================================================
        [HttpPost("lld")]
        public IActionResult CreateLld([FromBody] DossierLldDto dto)
        {
            var (client, vehicule, error) = Validate(dto.ClientId, dto.VehiculeId);
            if (error != null) return error;

            var dossier = new Dossier
            {
                TypeDossier = TypeDossier.location,
                Statut = StatutDossier.en_attente,
                DateCreation = DateTime.Now,
                ClientId = dto.ClientId,
                VehiculeId = dto.VehiculeId
            };

            _context.Dossiers.Add(dossier);
            _context.SaveChanges();

            var financement = new DossierFinancement
            {
                DossierId = dossier.IdDossier,
                Apport = dto.Apport,
                Financement = dto.Financement,
                Duree = dto.Duree,
                Kilometrage = dto.Kilometrage,
                Mensualite = dto.Mensualite
            };

            _context.DossierFinancements.Add(financement);
            _context.SaveChanges();

            vehicule!.Disponible = false;
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Dossier LLD créé avec succès",
                data = new
                {
                    dossier,
                    financement
                }
            });

        }
    }
}
