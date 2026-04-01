using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicules()
        {
            return await _context.Vehicules.ToListAsync();
        }

        // GET: api/vehicule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> GetVehicule(int id)
        {
            var vehicule = await _context.Vehicules.FindAsync(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            return vehicule;
        }

        // GET: api/vehicule/type/location
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehiculesByType(string type)
        {
            if (!Enum.TryParse<TypeOffre>(type, true, out var typeEnum))
            {
                return BadRequest("Type d'offre invalide. Valeurs possibles : vente, location");
            }

            return await _context.Vehicules
                .Where(v => v.TypeOffre == typeEnum)
                .ToListAsync();
        }

        // POST: api/vehicule
        [HttpPost]
        public async Task<ActionResult<Vehicule>> CreateVehicule(Vehicule vehicule)
        {
            _context.Vehicules.Add(vehicule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicule), new { id = vehicule.IdVehicule }, vehicule);
        }

        // PUT: api/vehicule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicule(int id, Vehicule vehicule)
        {
            if (id != vehicule.IdVehicule)
            {
                return BadRequest();
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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/vehicule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            var vehicule = await _context.Vehicules.FindAsync(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            _context.Vehicules.Remove(vehicule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiculeExists(int id)
        {
            return _context.Vehicules.Any(e => e.IdVehicule == id);
        }
    }
}
