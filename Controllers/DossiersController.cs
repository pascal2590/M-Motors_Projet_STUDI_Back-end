using Microsoft.AspNetCore.Mvc;
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

        // -------------------------
        // ACHAT
        // -------------------------
        [HttpPost("achat")]
        public IActionResult CreateAchat([FromBody] DossierAchatDto dto)
        {
            if (dto == null)
                return BadRequest("DTO invalide");

            var clientExists = _context.Clients.Any(c => c.IdClient == dto.ClientId);
            if (!clientExists)
                return BadRequest("Client introuvable");

            var vehiculeExists = _context.Vehicules.Any(v => v.IdVehicule == dto.VehiculeId);
            if (!vehiculeExists)
                return BadRequest("Véhicule introuvable");

            // PROTECTION ANTI DOUBLON
            var alreadyExists = _context.Dossiers.Any(d =>
                d.ClientId == dto.ClientId &&
                d.VehiculeId == dto.VehiculeId &&
                d.TypeDossier == Enums.TypeDossier.achat
            );

            if (alreadyExists)
            {
                return BadRequest("Un dossier existe déjà pour ce véhicule");
            }

            var dossier = new Dossier
            {
                TypeDossier = Enums.TypeDossier.achat,
                Statut = Enums.StatutDossier.en_attente,
                DateCreation = DateTime.Now,
                ClientId = dto.ClientId,
                VehiculeId = dto.VehiculeId
            };

            _context.Dossiers.Add(dossier);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Dossier achat créé",
                dossier.IdDossier
            });
        }

        // -------------------------
        // LLD
        // -------------------------
        [HttpPost("lld")]
        public IActionResult CreateLld([FromBody] DossierLldDto dto)
        {
            if (dto == null)
                return BadRequest("DTO invalide");

            // -----------------------------
            // VALIDATION DU CLIENT
            // -----------------------------
            var clientExists = _context.Clients.Any(c => c.IdClient == dto.ClientId);
            if (!clientExists)
                return BadRequest("Client introuvable");

            // -----------------------------
            // VALIDATION DU VEHICULE
            // -----------------------------
            var vehiculeExists = _context.Vehicules.Any(v => v.IdVehicule == dto.VehiculeId);
            if (!vehiculeExists)
                return BadRequest("Véhicule introuvable");

            // -----------------------------
            // ANTI-DOUBLON
            // -----------------------------
            var alreadyExists = _context.Dossiers.Any(d =>
                d.ClientId == dto.ClientId &&
                d.VehiculeId == dto.VehiculeId &&
                d.TypeDossier == Enums.TypeDossier.location
            );

            if (alreadyExists)
            {
                return BadRequest("Un dossier LLD existe déjà pour ce véhicule");
            }

            // -----------------------------
            // CREATION DU DOSSIER
            // -----------------------------
            var dossier = new Dossier
            {
                TypeDossier = Enums.TypeDossier.location,
                Statut = Enums.StatutDossier.en_attente,
                DateCreation = DateTime.Now,
                ClientId = dto.ClientId,
                VehiculeId = dto.VehiculeId
            };

            _context.Dossiers.Add(dossier);
            _context.SaveChanges(); // pour IdDossier

            // -----------------------------
            // FINANCEMENT LLD
            // -----------------------------
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

            // -----------------------------
            // REPONSE DETAILLEE
            // -----------------------------
            return Ok(new
            {
                message = "Dossier LLD créé avec succès",
                dossier = new
                {
                    dossier.IdDossier,
                    dossier.TypeDossier,
                    dossier.Statut,
                    dossier.DateCreation
                },
                financement
            });
        }


    }
}
