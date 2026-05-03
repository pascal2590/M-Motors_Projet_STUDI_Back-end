using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;
using m_motors_API.Enums;

namespace m_motors_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculeController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public VehiculeController(MMotorsContext context)
        {
            _context = context;
        }

        // GET: api/vehicule
        // Liste tous les véhicules disponibles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicules()
        {
            return await _context.Vehicules
                .Where(v => v.Disponible)
                .ToListAsync();
        }

        // GET: api/vehicule/5
        // Détail d'un véhicule
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> GetVehicule(int id)
        {
            var vehicule = await _context.Vehicules
                .Include(v => v.VehiculeServices)
                    .ThenInclude(vs => vs.ServiceLLD)
                .FirstOrDefaultAsync(v => v.IdVehicule == id);


            if (vehicule == null)
            {
                return NotFound(new
                {
                    message = "Véhicule introuvable"
                });
            }

            return Ok(vehicule);
        }

        // GET: api/vehicule/type/location
        // Filtrer par type d'offre
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehiculesByType(string type)
        {
            if (!Enum.TryParse<TypeOffre>(type, true, out var typeEnum))
            {
                return BadRequest(new
                {
                    message = "Type invalide (vente, location)"
                });
            }

            var vehicules = await _context.Vehicules
                .Include(v => v.VehiculeServices)
                    .ThenInclude(vs => vs.ServiceLLD)
                .Where(v =>
                    v.TypeOffre == typeEnum &&
                    v.Disponible
                )
                .ToListAsync();

            return Ok(vehicules);
        }


        // POST: api/vehicule
        // Création véhicule     
        [HttpPost]
        public async Task<ActionResult<Vehicule>> CreateVehicule(Vehicule vehicule)
        {
            vehicule.Disponible = true;

            _context.Vehicules.Add(vehicule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetVehicule),
                new { id = vehicule.IdVehicule },
                vehicule
            );
        }

  
        // PUT: api/vehicule/5
        // Modification véhicule    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicule(int id, Vehicule vehicule)
        {
            if (id != vehicule.IdVehicule)
            {
                return BadRequest(new
                {
                    message = "ID incohérent"
                });
            }

            _context.Entry(vehicule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }


        // Valider la disponibilité d'un véhicule (ex: après une vente ou une location)     
        private bool VehiculeExists(int id)
        {
            return _context.Vehicules.Any(e => e.IdVehicule == id);
        }
    }
}
