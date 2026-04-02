using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;
using m_motors_API.Services;

namespace m_motors_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculeController : ControllerBase
    {
        private readonly VehiculeService _vehiculeService;

        public VehiculeController(MMotorsContext context)
        {
            _vehiculeService = new VehiculeService(context);
        }

        // GET: api/vehicule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehicules()
        {
            var vehicules = await _vehiculeService.GetAllVehiculesAsync();
            return Ok(vehicules);
        }

        // GET: api/vehicule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> GetVehicule(int id)
        {
            var vehicule = await _vehiculeService.GetVehiculeByIdAsync(id);
            if (vehicule == null) return NotFound();
            return Ok(vehicule);
        }

        // GET: api/vehicule/type/vente
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetVehiculesByType(string type)
        {
            var vehicules = await _vehiculeService.GetVehiculesByTypeAsync(type);

            if (!vehicules.Any())
                return NotFound(new { message = $"Aucun véhicule de type {type} trouvé." });

            return Ok(vehicules);
        }

        // POST: api/vehicule
        [HttpPost]
        public async Task<ActionResult<Vehicule>> CreateVehicule(Vehicule vehicule)
        {
            var created = await _vehiculeService.CreateVehiculeAsync(vehicule);
            return CreatedAtAction(nameof(GetVehicule), new { id = created.IdVehicule }, created);
        }

        // PUT: api/vehicule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicule(int id, Vehicule vehicule)
        {
            if (id != vehicule.IdVehicule)
                return BadRequest();

            var updated = await _vehiculeService.UpdateVehiculeAsync(vehicule);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/vehicule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            var deleted = await _vehiculeService.DeleteVehiculeAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
