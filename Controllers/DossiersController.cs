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

        // =====================================================
        // ACHAT
        // =====================================================
        [HttpPost("achat")]
        public IActionResult CreateAchat([FromBody] DossierAchatDto dto)
        {
            if (dto == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "DTO invalide"
                });
            }

            // -----------------------------
            // VALIDATION CLIENT
            // -----------------------------
            var clientExists = _context.Clients
                .Any(c => c.IdClient == dto.ClientId);

            if (!clientExists)
            {
                return Ok(new
                {
                    success = false,
                    message = "Client introuvable"
                });
            }

            // -----------------------------
            // VALIDATION VEHICULE
            // -----------------------------
            var vehicule = _context.Vehicules
                .FirstOrDefault(v => v.IdVehicule == dto.VehiculeId);

            if (vehicule == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Véhicule introuvable"
                });
            }

            // -----------------------------
            // VERIFICATION DISPONIBILITE
            // -----------------------------
            if (!vehicule.Disponible)
            {
                return Ok(new
                {
                    success = false,
                    message = "Ce véhicule n'est plus disponible"
                });
            }

            // -----------------------------
            // ANTI DOUBLON VEHICULE
            // -----------------------------
            var vehiculeAlreadyUsed = _context.Dossiers
                .Any(d => d.VehiculeId == dto.VehiculeId);

            if (vehiculeAlreadyUsed)
            {
                return Ok(new
                {
                    success = false,
                    message = "Ce véhicule possède déjà un dossier actif"
                });
            }

            // -----------------------------
            // CREATION DOSSIER
            // -----------------------------
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

            // -----------------------------
            // RENDRE VEHICULE INDISPONIBLE
            // -----------------------------
            vehicule.Disponible = false;
            _context.SaveChanges();

            // -----------------------------
            // RESPONSE
            // -----------------------------
            return Ok(new
            {
                success = true,
                message = "Dossier Achat créé avec succès",
                dossier = new
                {
                    dossier.IdDossier,
                    dossier.TypeDossier,
                    dossier.Statut,
                    dossier.DateCreation
                }
            });
        }



        // =====================================================
        // LLD
        // =====================================================
        [HttpPost("lld")]
        public IActionResult CreateLld([FromBody] DossierLldDto dto)
        {
            if (dto == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "DTO invalide"
                });
            }

            // -----------------------------
            // VALIDATION CLIENT
            // -----------------------------
            var clientExists = _context.Clients
                .Any(c => c.IdClient == dto.ClientId);

            if (!clientExists)
            {
                return Ok(new
                {
                    success = false,
                    message = "Client introuvable"
                });
            }

            // -----------------------------
            // VALIDATION VEHICULE
            // -----------------------------
            var vehicule = _context.Vehicules
                .FirstOrDefault(v => v.IdVehicule == dto.VehiculeId);

            if (vehicule == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Véhicule introuvable"
                });
            }

            // -----------------------------
            // VERIFICATION DISPONIBILITE
            // -----------------------------
            if (!vehicule.Disponible)
            {
                return Ok(new
                {
                    success = false,
                    message = "Ce véhicule n'est plus disponible"
                });
            }

            // -----------------------------
            // ANTI DOUBLON VEHICULE
            // -----------------------------
            var vehiculeAlreadyUsed = _context.Dossiers
                .Any(d => d.VehiculeId == dto.VehiculeId);

            if (vehiculeAlreadyUsed)
            {
                return Ok(new
                {
                    success = false,
                    message = "Ce véhicule possède déjà un dossier actif"
                });
            }

            // -----------------------------
            // CREATION DOSSIER
            // -----------------------------
            var dossier = new Dossier
            {
                TypeDossier = TypeDossier.location,
                Statut = StatutDossier.en_attente,
                DateCreation = DateTime.Now,
                ClientId = dto.ClientId,
                VehiculeId = dto.VehiculeId
            };

            _context.Dossiers.Add(dossier);
            _context.SaveChanges(); // pour récupérer IdDossier

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
            // RENDRE VEHICULE INDISPONIBLE
            // -----------------------------
            vehicule.Disponible = false;
            _context.SaveChanges();

            // -----------------------------
            // RESPONSE
            // -----------------------------
            return Ok(new
            {
                success = true,
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
