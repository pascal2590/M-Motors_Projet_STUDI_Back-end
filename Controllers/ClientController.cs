using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace m_motors_API.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public ClientController(MMotorsContext context)
        {
            _context = context;
        }

        
        // CREATE CLIENT        
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            client.DateUpload = DateTime.Now;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(CreateClient),
                new { id = client.IdClient },
                client
            );
        }

        
        // GET MY DOSSIERS (JWT - PROPRE)        
        [Authorize]
        [HttpGet("dossiers")]
        public IActionResult GetMyDossiers()
        {
            var clientIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (clientIdClaim == null)
                return Unauthorized();

            var clientId = int.Parse(clientIdClaim);

            var dossiers = _context.Dossiers
                .Where(d => d.ClientId == clientId)
                .Select(d => new DossierDto
                {
                    IdDossier = d.IdDossier,
                    TypeDossier = d.TypeDossier.ToString(),
                    Statut = d.Statut.ToString(),
                    DateCreation = d.DateCreation,

                    Vehicule = new VehiculeDto
                    {
                        IdVehicule = d.Vehicule.IdVehicule,
                        Marque = d.Vehicule.Marque,
                        Modele = d.Vehicule.Modele,
                        Annee = d.Vehicule.Annee ?? 0,
                        Prix = d.Vehicule.Prix ?? 0,
                        ImageUrl = d.Vehicule.ImageUrl
                    },

                    Documents = d.Documents.Select(doc => new DocumentDto
                    {
                        IdDocument = doc.IdDocument,
                        NomDocument = doc.NomDocument,
                        TypeDocument = doc.TypeDocument,
                        DateUpload = doc.DateUpload,
                        CheminFichier = doc.CheminFichier
                    }).ToList()
                })

                .ToList();

            return Ok(dossiers);
        }

        // GET DOSSIER DETAIL (IMPORTANT POUR TON FRONT)        
        [Authorize]
        [HttpGet("dossiers/{id}")]
        public IActionResult GetDossierById(int id)
        {
            var clientIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (clientIdClaim == null)
                return Unauthorized();

            var clientId = int.Parse(clientIdClaim);

            var dossier = _context.Dossiers
                .Include(d => d.Vehicule)
                .Include(d => d.Documents)
                .FirstOrDefault(d =>
                    d.IdDossier == id &&
                    d.ClientId == clientId
                );

            if (dossier == null)
                return NotFound();

            return Ok(dossier);
        }
    }
}
