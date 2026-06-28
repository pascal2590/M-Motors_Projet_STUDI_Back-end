using m_motors_API.Data;
using m_motors_API.DTO;
using m_motors_API.Enums;
using m_motors_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

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

            //var already = _context.Dossiers
            //    .Any(d => d.VehiculeId == vehiculeId);

            // if (already)
               // return (null, null, BadRequest("Vehicule déjà utilisé dans un dossier"));

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

            // DOCUMENTS OBLIGATOIRES pour l'ACHAT
            var documents = new List<DocumentClient>
            {
                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "identite",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "domicile",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "revenus",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "rib",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
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

            // DOCUMENTS OBLIGATOIRES pour la LLD
            var documents = new List<DocumentClient>
            {
                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "identite",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "domicile",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "revenus",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "rib",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
                },

                new DocumentClient
                {
                    DossierId = dossier.IdDossier,
                    TypeDocument = "permis",
                    NomDocument = null,
                    CheminFichier = null,
                    DateAjout = null
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
                    idDossier = dossier.IdDossier,

                    typeDossier = dossier.TypeDossier.ToString(),

                    statut = dossier.Statut.ToString(),

                    dateCreation = dossier.DateCreation,

                    financement = new
                    {
                        financement.Id,
                        financement.Apport,
                        financement.Financement,
                        financement.Duree,
                        financement.Kilometrage,
                        financement.Mensualite
                    }
                }
            });
        }

        // DOSSIER PAR ID (Détails + Véhicule + Services + Documents)
        [HttpGet("{id}")]
        public IActionResult GetDossierById(int id)
        {
            var dossier = _context.Dossiers
                .FirstOrDefault(d => d.IdDossier == id);

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
                    v.Description,
                    v.ImageUrl
                })
                .FirstOrDefault();

            var services = new List<object>();

            if (dossier.TypeDossier == TypeDossier.location)
            {
                services = _context.VehiculeServiceLLDs
                    .Where(vs => vs.IdVehicule == dossier.VehiculeId)
                    .Select(vs => new
                    {
                        vs.ServiceLLD.IdService,
                        vs.ServiceLLD.NomService,
                        vs.ServiceLLD.Description
                    })
                    .ToList<object>();
            }

            var documents = _context.Documents
                .Where(d => d.DossierId == id)
                .Select(d => new
                {
                    d.IdDocument,
                    d.TypeDocument,
                    d.NomDocument,

                    CheminFichier = string.IsNullOrEmpty(d.CheminFichier)
                        ? null
                        : $"{Request.Scheme}://{Request.Host}{d.CheminFichier}",

                    d.DateAjout
                })
                .ToList();

            // Dernière mise à jour depuis suivi_dossier
            var lastUpdate = _context.SuiviDossiers
                .Where(s => s.DossierId == id)
                .OrderByDescending(s => s.DateModification)
                .Select(s => s.DateModification)
                .FirstOrDefault();

            var historique = _context.SuiviDossiers
                .Where(s => s.DossierId == id)
                .OrderBy(s => s.DateModification)
                .Select(s => new
                {
                    statut = s.Statut.ToString(),
                    date = s.DateModification
                })
                .ToList();

            var suivis = _context.SuiviDossiers
                .Where(s => s.DossierId == id)
                .OrderByDescending(s => s.DateModification)
                .Select(s => new
                {
                    statut = s.Statut.ToString(),
                    commentaire = s.Commentaire,
                    dateModification = s.DateModification
                })
                .ToList();

            return Ok(new
            {
                dossier = new
                {
                    id = dossier.IdDossier,
                    typeDossier = dossier.TypeDossier.ToString(),
                    statut = dossier.Statut.ToString(),
                    dateCreation = dossier.DateCreation,
                    clientId = dossier.ClientId,
                    vehiculeId = dossier.VehiculeId
                },
                vehicule,
                services,
                documents,
                historique,  // Historique pour le backoffice
                suivis // Suivis détaillés pour le client
            });
        }

        // DOSSIERS PAR CLIENT  
        [HttpGet("client/{clientId}")]
        public IActionResult GetByClient(int clientId)
        {
            var dossiers = _context.Dossiers
                .Include(d => d.Commercial)
                .Where(d => d.ClientId == clientId)
                .Select(d => new
                {
                    d.IdDossier,
                    typeDossier = d.TypeDossier.ToString(),
                    statut = d.Statut.ToString(),
                    dateCreation = d.DateCreation,

                    commercial = d.Commercial != null
                        ? d.Commercial.Prenom + " " + d.Commercial.Nom
                        : "Non assigné",

                    Vehicule = new
                    {
                        d.Vehicule.IdVehicule,
                        d.Vehicule.Marque,
                        d.Vehicule.Modele,
                        d.Vehicule.Annee,
                        d.Vehicule.Kilometrage,
                        d.Vehicule.Prix,
                        d.Vehicule.ImageUrl
                    },

                    Documents = _context.Documents
                        .Where(doc => doc.DossierId == d.IdDossier)
                        .Select(doc => new
                        {
                            doc.TypeDocument,

                            CheminFichier = string.IsNullOrEmpty(doc.CheminFichier)
                                ? null
                                : $"{Request.Scheme}://{Request.Host}{doc.CheminFichier}"
                        })
                        .ToList()
                })
                .ToList();

            return Ok(dossiers);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dossiers = _context.Dossiers
                .Select(d => new
                {
                    d.IdDossier,
                    d.TypeDossier,
                    d.Statut,
                    d.DateCreation,
                    d.ClientId,

                    Client = new
                    {
                        d.Client.Prenom,
                        d.Client.Nom
                    }

                })
                .ToList();

            return Ok(dossiers);
        }

        // MISE À JOUR STATUT + HISTORIQUE + NOTIFICATION CLIENT
        [HttpPut("{id}/statut")]
        public IActionResult UpdateStatut(int id, [FromBody] string nouveauStatut)
        {
            var dossier = _context.Dossiers.FirstOrDefault(d => d.IdDossier == id);

            if (dossier == null)
                return NotFound("Dossier introuvable");

            if (!Enum.TryParse<StatutDossier>(nouveauStatut, true, out var statut))
                return BadRequest("Statut invalide");

            //Interdire la modification si déjà finalisé
            if (dossier.Statut == StatutDossier.accepte || dossier.Statut == StatutDossier.refuse)
            {
                return BadRequest("Ce dossier est déjà finalisé et ne peut plus être modifié");
            }

            // Interdit un retour arrière
            if (dossier.Statut == StatutDossier.en_etude && statut == StatutDossier.en_attente)
            {
                return BadRequest("Impossible de revenir à 'en attente' après étude");
            }

            // DOSSIER FINALISE
            if (
                dossier.Statut == StatutDossier.accepte ||
                dossier.Statut == StatutDossier.refuse
            )
            {
                return BadRequest(new
                {
                    message = "Ce dossier est déjà finalisé"
                });
            }

            // RETOUR EN ARRIERE INTERDIT
            if (
                dossier.Statut == StatutDossier.en_etude &&
                statut == StatutDossier.en_attente
            )
            {
                return BadRequest(new
                {
                    message = "Impossible de revenir en arrière"
                });
            }

            // MEME STATUT - PAS DE MODIFICATION
            if (dossier.Statut == statut)
            {
                return BadRequest(new
                {
                    message = "Le dossier possède déjà ce statut"
                });
            }

            // VERIFICATION DOCUMENTS AVANT ACCEPTATION
            if (statut == StatutDossier.accepte)
            {
                var documentsManquants = _context.Documents
                    .Where(d =>
                        d.DossierId == id &&
                        (
                            d.CheminFichier == null ||
                            d.CheminFichier == ""
                        )
                    )
                    .ToList();

                if (documentsManquants.Any())
                {
                    return BadRequest(new
                    {
                        message = "Impossible d'accepter le dossier : des documents sont manquants"
                    });
                }
            }

            // UPDATE
            dossier.Statut = statut;

            var userIdClaim = User.FindFirst("nameid");
            int? userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : null;

            _context.SuiviDossiers.Add(new SuiviDossier
            {
                DossierId = id,
                Statut = statut,
                Commentaire = $"Statut changé vers {statut}",
                DateModification = DateTime.Now,
                UserId = userId
            });

            string statutLabel = statut switch
            {
                StatutDossier.en_attente => "En attente",
                StatutDossier.en_etude => "En étude",
                StatutDossier.accepte => "Accepté",
                StatutDossier.refuse => "Refusé",
                _ => statut.ToString()
            };

            _context.MessagesClients.Add(new MessageClient
            {
                ClientId = dossier.ClientId ?? 0,

                Sujet = $"Dossier #{id} mis à jour",

                Contenu =
                    "Bonjour,\n\n" +
                    "Le statut de votre dossier a évolué.\n\n" +
                    $"Nouveau statut : {statutLabel}\n\n" +
                    "Cordialement,\n" +
                    "M-Motors"
            });
            
            _context.SaveChanges();

            return Ok(new
            {
                message = "Statut mis à jour",
                statut = dossier.Statut.ToString()
            });
        }

        [HttpPut("{id}/assign")]
        [Authorize(Roles = "Commercial")]
        public IActionResult AssignCommercial(int id)
        {
            var dossier = _context.Dossiers.FirstOrDefault(d => d.IdDossier == id);

            if (dossier == null)
                return NotFound("Dossier introuvable");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            dossier.CommercialId = int.Parse(userId);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Dossier assigné",
                commercialId = dossier.CommercialId
            });
        }
    }
}
