using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.Models;
using m_motors_API.Enums;
using m_motors_API.DTOs;

namespace m_motors_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculeController : ControllerBase
    {
        private readonly MMotorsContext _context;

        public VehiculeController(
            MMotorsContext context
        )
        {
            _context = context;
        }

        // GET: api/vehicule
        // Liste tous les véhicules disponibles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetVehicules()
        {
            var vehicules = await _context.Vehicules
                .Include(v => v.VehiculeServices)
                .ThenInclude(vs => vs.ServiceLLD)
                .Where(v => v.Disponible)
                .Select(v => new
                {
                    idVehicule = v.IdVehicule,
                    marque = v.Marque,
                    modele = v.Modele,
                    annee = v.Annee,
                    kilometrage = v.Kilometrage,
                    prix = v.Prix,
                    description = v.Description,
                    typeOffre = v.TypeOffre.ToString().ToLower(),
                    disponible = v.Disponible,
                    imageUrl = v.ImageUrl
                })
                .ToListAsync();
            return Ok(vehicules);
        }

        // GET: api/vehicule/5
        // Détail d'un véhicule
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetVehicule(int id)
        {
            var vehicule = await _context.Vehicules
                .Include(v => v.VehiculeServices)
                .ThenInclude(vs => vs.ServiceLLD)
                .Where(v => v.IdVehicule == id)
                .Select(v => new
                {
                    idVehicule = v.IdVehicule,
                    marque = v.Marque,
                    modele = v.Modele,
                    annee = v.Annee,
                    kilometrage = v.Kilometrage,
                    prix = v.Prix,
                    description = v.Description,
                    typeOffre = v.TypeOffre.ToString().ToLower(),
                    disponible = v.Disponible,
                    imageUrl = v.ImageUrl
                })
                .FirstOrDefaultAsync();

            if (vehicule == null)
            {
                return NotFound(new { message = "Véhicule introuvable" });
            }

            return Ok(vehicule);
        }

        // GET: api/vehicule/type/location
        // Filtrer par type d'offre
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<object>>> GetVehiculesByType(string type)
        {
            if (!Enum.TryParse<TypeOffre>(type, true, out var typeEnum))
            {
                return BadRequest(new
                {
                    message = "Type invalide (vente, location)"
                });
            }

            var vehicules = await _context.Vehicules
                .Where(v => v.TypeOffre == typeEnum && v.Disponible)
                .Select(v => new
                {
                    idVehicule = v.IdVehicule,
                    marque = v.Marque,
                    modele = v.Modele,
                    annee = v.Annee,
                    kilometrage = v.Kilometrage,
                    prix = v.Prix,
                    description = v.Description,
                    typeOffre = v.TypeOffre.ToString().ToLower(),
                    disponible = v.Disponible,
                    imageUrl = v.ImageUrl
                })
                .ToListAsync();
            return Ok(vehicules);
        }

        // POST: api/vehicule
        // Création véhicule
        [HttpPost]
        public async Task<
            ActionResult<Vehicule>
        > CreateVehicule(
            VehiculeDto dto
        )
        {
            var vehicule = new Vehicule
            {
                Marque = dto.Marque,
                Modele = dto.Modele,
                Annee = dto.Annee,
                Kilometrage = dto.Kilometrage,
                Prix = dto.Prix,
                Description = dto.Description,
                TypeOffre = dto.TypeOffre,
                Disponible = dto.Disponible,
                ImageUrl = dto.ImageUrl
            };

            _context.Vehicules.Add(
                vehicule
            );

            await _context
                .SaveChangesAsync();

            // SERVICES LLD
            if (
                dto.TypeOffre == TypeOffre.location && dto.ServicesLld != null
            )
            {
                foreach (
                    var serviceId
                    in dto.ServicesLld
                )
                {
                    _context
                        .VehiculeServiceLLDs
                        .Add(

                        new VehiculeServiceLLD
                        {
                            IdVehicule = vehicule.IdVehicule,
                            IdService = serviceId
                        });
                }
                await _context
                    .SaveChangesAsync();
            }

            return CreatedAtAction(

                nameof(GetVehicule),
                new
                {
                    id = vehicule.IdVehicule
                },
                vehicule
            );
        }

        // PUT: api/vehicule/5
        // Modification véhicule
        [HttpPut("{id}")]
        public async Task<IActionResult>
            UpdateVehicule(
                int id,
                VehiculeDto dto
            )
        {
            var vehicule = await _context
                .Vehicules
                .Include(v =>
                    v.VehiculeServices)
                .FirstOrDefaultAsync(
                    v =>
                        v.IdVehicule == id
                );

            if (vehicule == null)
            {
                return NotFound(new
                {
                    message = "Véhicule introuvable"
                });
            }

            // UPDATE VEHICULE
            vehicule.Marque = dto.Marque;
            vehicule.Modele = dto.Modele;
            vehicule.Annee = dto.Annee;
            vehicule.Kilometrage = dto.Kilometrage;
            vehicule.Prix = dto.Prix;
            vehicule.Description = dto.Description;
            vehicule.TypeOffre = dto.TypeOffre;
            vehicule.Disponible = dto.Disponible;
            vehicule.ImageUrl = dto.ImageUrl;

            // Suppression des anciens services liés au véhicule
            var existingServices =
                _context
                .VehiculeServiceLLDs
                .Where(v =>
                    v.IdVehicule == id);
            _context
                .VehiculeServiceLLDs
                .RemoveRange(
                    existingServices
                );

            // Ajout des nouveaux services liés au véhicule
            if (
                dto.TypeOffre == TypeOffre.location && dto.ServicesLld != null
            )
            {
                foreach (
                    var serviceId
                    in dto.ServicesLld
                )
                {
                    _context
                        .VehiculeServiceLLDs
                        .Add(

                        new VehiculeServiceLLD
                        {
                            IdVehicule = id,
                            IdService = serviceId
                        });
                }
            }

            await _context
                .SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/vehicule/5
        // Suppression d'unvéhicule
        [HttpDelete("{id}")]
        public async Task<IActionResult>
            DeleteVehicule(int id)
        {
            var vehicule = await _context
                .Vehicules
                .FindAsync(id);

            if (vehicule == null)
            {
                return NotFound(new
                {
                    message = "Véhicule introuvable"
                });
            }

            // Suppression des services LLD liés au véhicule
            var services = _context
                .VehiculeServiceLLDs
                .Where(v =>
                    v.IdVehicule == id);

            _context
                .VehiculeServiceLLDs
                .RemoveRange(services);

            // Suppression du véhicule
            _context
                .Vehicules
                .Remove(vehicule);

            await _context
                .SaveChangesAsync();

            return NoContent();
        }

        // Vérifier existence véhicule
        private bool VehiculeExists(
            int id
        )
        {
            return _context
                .Vehicules
                .Any(e =>
                    e.IdVehicule == id);
        }
    }
}