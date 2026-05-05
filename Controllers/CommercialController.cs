using m_motors_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/commercial")]
    [Authorize(Roles = "Commercial")]
    public class CommercialController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public CommercialController(MMotorsContext context)
        {
            _context = context;
        }

        // Exemple : récupérer les dossiers
        [HttpGet("dossiers")]
        public IActionResult GetDossiers()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dossiers = _context.Dossiers
                .Include(d => d.Client)
                .Include(d => d.Vehicule)
                // .Where(d => d.CommercialId == int.Parse(userId)) // si on ajoute la relation
                .Select(d => new
                {
                    id = d.IdDossier,
                    client = d.Client != null ? d.Client.Nom : "N/A",
                    vehicule = d.Vehicule != null
                        ? d.Vehicule.Marque + " " + d.Vehicule.Modele
                        : "N/A",
                    status = d.Statut.ToString(),
                    typeDossier = d.TypeDossier.ToString(),
                    dateCreation = d.DateCreation.ToString("dd/MM/yyyy")
                })
                .ToList();

            return Ok(dossiers);
        }

    }
}
