using System.Linq;
using BCrypt.Net;
using m_motors_API.Models;

namespace m_motors_API.Data
{
    public static class SeedData
    {
        public static void Initialize(MMotorsContext context)
        {
            // Vérifie si l'administrateur existe déjà
            if (!context.Utilisateurs.Any(u => u.Email == "admin@mmotors.fr"))
            {
                var admin = new Utilisateur
                {
                    Nom = "Admin",
                    Prenom = "Pascal",
                    Email = "admin@mmotors.fr",

                    // Hash BCrypt du mot de passe
                    Password = BCrypt.Net.BCrypt.HashPassword("Y6$sb&WYNu?Z"),

                    // ID du rôle ADMIN
                    RoleId = 1
                };

                // Ajout dans la base
                context.Utilisateurs.Add(admin);

                // Sauvegarde
                context.SaveChanges();
            }
        }
    }
}