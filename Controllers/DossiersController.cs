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

        // VALIDATION (CLIENT + VEHICULE)
        private (Client? client, Vehicule? vehicule, IActionResult? error)
            Validate(int clientId, int vehiculeId)
        {
            var client = _context.Clients
                .FirstOrDefault(c => c.IdClient == clientId);

            if (client == null)
                return (null, null, NotFound("Client introuvable"));

            var vehicule = _context.Vehicules
                .FirstOrDefault(v => v.IdVehicule == vehiculeId);

            if (vehicule == null)
                return (null, null, NotFound("Vehicule introuvable"));

            if (!vehicule.Disponible)
                return (null, null, BadRequest("Vehicule non disponible"));

            var already = _context.Dossiers
                .Any(d => d.VehiculeId == vehiculeId);

            if (already)
                return (null, null, BadRequest("Vehicule déjà utilisé dans un dossier"));

            return (client, vehicule, null);
        }


        // ACHAT
        [HttpPost("achat")]
        public IActionResult CreateAchat([FromBody] DossierAchatDto dto)
        {
            var (client, vehicule, error) =
                Validate(dto.ClientId, dto.VehiculeId);

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

            // DOCUMENTS OBLIGATOIRES ACHAT
            var documents = new List<DocumentClient>
            {
                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "identite",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "domicile",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "revenus",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "rib",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                }
            };

            _context.Documents.AddRange(documents);

            // Rendre le véhicule indisponible
            vehicule!.Disponible = false;

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Dossier Achat créé avec succès",
                data = new
                {
                    dossier.IdDossier,
                    dossier.TypeDossier,
                    dossier.Statut,
                    dossier.DateCreation
                }
            });
        }

        // LLD
        [HttpPost("lld")]
        public IActionResult CreateLld([FromBody] DossierLldDto dto)
        {
            var (client, vehicule, error) =
                Validate(dto.ClientId, dto.VehiculeId);

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

            // FINANCEMENT

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

            // DOCUMENTS OBLIGATOIRES LLD
            var documents = new List<DocumentClient>
            {
                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "identite",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "domicile",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "revenus",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "rib",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "permis",
                    NomDocument = null,
                    CheminFichier = null,
                    DateUpload = null
                }
            };

            _context.Documents.AddRange(documents);

            // Rendre le véhicule indisponible
            vehicule!.Disponible = false;

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Dossier LLD créé avec succès",
                data = new
                {
                    dossier.IdDossier,
                    dossier.TypeDossier,
                    dossier.Statut,
                    dossier.DateCreation,
                    financement
                }
            });
        }


        [HttpGet("{id}")]
        public IActionResult GetDossierById(int id)
        {
            try
            {
                var dossier = _context.Dossiers
                    .Where(d => d.IdDossier == id)
                    .Select(d => new
                    {
                        d.IdDossier,
                        d.TypeDossier,
                        d.Statut,
                        d.DateCreation,
                        d.ClientId,
                        d.VehiculeId
                    })
                    .FirstOrDefault();

                if (dossier == null)
                    return NotFound();

                var vehicule = _context.Vehicules
                    .Where(v => v.IdVehicule == dossier.VehiculeId)
                    .Select(v => new
                    {
                        v.IdVehicule,
                        v.Marque,
                        v.Modele,
                        v.Annee,
                        v.Kilometrage,
                        v.Prix,
                        Description = v.Description ?? "",
                        ImageUrl = v.ImageUrl ?? ""
                    })
                    .FirstOrDefault();

                var documents = _context.Documents
                    .Where(d => d.DossierId == id)
                    .Select(d => new
                    {
                        d.IdDocument,
                        NomDocument = d.NomDocument ?? "",
                        TypeDocument = d.TypeDocument ?? "",
                        CheminFichier = d.CheminFichier ?? "",
                        DateUpload = d.DateUpload
                    })
                    .ToList();

                return Ok(new
                {
                    dossier,
                    vehicule,
                    documents
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erreur serveur dossier",
                    detail = ex.Message
                });
            }
        }

        // DOSSIERS PAR CLIENT  
        [HttpGet("client/{clientId}")]
        public IActionResult GetByClient(int clientId)
        {
            var dossiers = _context.Dossiers
                .Where(d => d.ClientId == clientId)
                .Select(d => new
                {
                    d.IdDossier,
                    d.TypeDossier,
                    d.Statut,
                    d.DateCreation,
                    d.VehiculeId
                })
                .ToList();

            return Ok(dossiers);
        }






    }
}
