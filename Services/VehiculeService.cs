using m_motors_API.Data;
using m_motors_API.Models;
using Microsoft.EntityFrameworkCore;

namespace m_motors_API.Services
{
    public class VehiculeService
    {
        private readonly MMotorsContext _context;

        public VehiculeService(MMotorsContext context)
        {
            _context = context;
        }

        // Récupérer tous les véhicules
        public async Task<List<Vehicule>> GetAllVehiculesAsync()
        {
            return await _context.Vehicules.ToListAsync();
        }

        // Récupérer un véhicule par ID
        public async Task<Vehicule?> GetVehiculeByIdAsync(int id)
        {
            return await _context.Vehicules.FindAsync(id);
        }

        // Récupérer les véhicules par type (vente / location)
        public async Task<List<Vehicule>> GetVehiculesByTypeAsync(string type)
        {
            if (!Enum.TryParse<TypeOffre>(type, true, out var typeEnum))
                return new List<Vehicule>();

            return await _context.Vehicules
                .Where(v => v.TypeOffre == typeEnum)
                .ToListAsync();
        }

        // Créer un véhicule
        public async Task<Vehicule> CreateVehiculeAsync(Vehicule vehicule)
        {
            _context.Vehicules.Add(vehicule);
            await _context.SaveChangesAsync();
            return vehicule;
        }

        // Mettre à jour un véhicule
        public async Task<bool> UpdateVehiculeAsync(Vehicule vehicule)
        {
            _context.Entry(vehicule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vehicules.Any(v => v.IdVehicule == vehicule.IdVehicule))
                    return false;
                throw;
            }
        }

        // Supprimer un véhicule
        public async Task<bool> DeleteVehiculeAsync(int id)
        {
            var vehicule = await _context.Vehicules.FindAsync(id);
            if (vehicule == null) return false;

            _context.Vehicules.Remove(vehicule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
