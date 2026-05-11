using Microsoft.AspNetCore.Mvc;
using m_motors_API.Data;
using m_motors_API.Models;
using Microsoft.EntityFrameworkCore;

namespace m_motors_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccueilController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public AccueilController(MMotorsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccueil()
        {
            // Exemple : On récupére les 5 derniers véhicules disponibles
            var vehicules = _context.Vehicules
                .Where(v => v.Disponible) // Ajout de ce champ pour filtrer les véhicules disponibles
                .OrderByDescending(v => v.DateAjout)
                .Take(5)
                .Select(v => new
                {
                    v.IdVehicule,
                    v.Marque,
                    v.Modele,
                    v.Prix,
                    v.ImageUrl
                })
                .ToList();

            return Ok(vehicules);
        }
    }
}
