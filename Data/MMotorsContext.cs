using m_motors_API.Models;
using Microsoft.EntityFrameworkCore;

namespace m_motors_API.Data
{
    public class MMotorsContext : DbContext
    {
        public MMotorsContext(DbContextOptions<MMotorsContext> options)
            : base(options)
        {
        }

        // Déclaration des tables accessibles via Entity Framework
        public DbSet<Client> Clients { get; set; }
        public DbSet<DocumentClient> Documents { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceLLD> ServiceLLDs { get; set; }
        public DbSet<SuiviDossier> SuiviDossiers { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Vehicule> Vehicules { get; set; }
        public DbSet<VehiculeServiceLLD> VehiculeServiceLLDs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //---------------------------------
            // Table CLIENT
            //---------------------------------
            modelBuilder.Entity<Client>()
                .ToTable("client")
                .HasKey(c => c.IdClient);

            //---------------------------------
            // Table DOCUMENT
            //---------------------------------
            modelBuilder.Entity<DocumentClient>()
                .ToTable("document")
                .HasKey(d => d.IdDocument);

            modelBuilder.Entity<DocumentClient>()
                .HasOne(d => d.Dossier)
                .WithMany(ds => ds.Documents)
                .HasForeignKey(d => d.DossierId);

            //---------------------------------
            // Table DOSSIER
            //---------------------------------
            modelBuilder.Entity<Dossier>()
                .ToTable("dossier")
                .HasKey(d => d.IdDossier);

            modelBuilder.Entity<Dossier>()
                .HasOne(d => d.Client)
                .WithMany(c => c.Dossiers)
                .HasForeignKey(d => d.ClientId);

            modelBuilder.Entity<Dossier>()
                .HasOne(d => d.Vehicule)
                .WithMany(v => v.Dossiers)
                .HasForeignKey(d => d.VehiculeId);

            //---------------------------------
            // Table ROLE
            //---------------------------------
            modelBuilder.Entity<Role>()
                .ToTable("role")
                .HasKey(r => r.IdRole);

            //---------------------------------
            // Table SERVICE_LLD
            //---------------------------------
            modelBuilder.Entity<ServiceLLD>()
                .ToTable("service_lld")
                .HasKey(s => s.IdService);

            //---------------------------------
            // Table SUIVI_DOSSIER
            //---------------------------------
            modelBuilder.Entity<SuiviDossier>()
                .ToTable("suivi_dossier")
                .HasKey(s => s.IdSuivi);

            modelBuilder.Entity<SuiviDossier>()
                .HasOne(s => s.Dossier)
                .WithMany(d => d.Suivis)
                .HasForeignKey(s => s.DossierId);

            modelBuilder.Entity<SuiviDossier>()
                .HasOne(s => s.Utilisateur)
                .WithMany(u => u.Suivis)
                .HasForeignKey(s => s.UserId);

            //---------------------------------
            // Table UTILISATEUR
            //---------------------------------
            modelBuilder.Entity<Utilisateur>()
                .ToTable("utilisateur")
                .HasKey(u => u.IdUser);

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Utilisateurs)
                .HasForeignKey(u => u.RoleId);

            //---------------------------------
            // Table VEHICULE
            //---------------------------------
            modelBuilder.Entity<Vehicule>()
                .ToTable("vehicule")
                .HasKey(v => v.IdVehicule);

            modelBuilder.Entity<Vehicule>()
                .Property(v => v.TypeOffre)
                .HasConversion<string>();

            //---------------------------------
            // Table liaison VEHICULE_SERVICE_LLD
            //---------------------------------
            modelBuilder.Entity<VehiculeServiceLLD>()
                .ToTable("vehicule_service_lld")
                .HasKey(vs => new { vs.IdVehicule, vs.IdService });

            modelBuilder.Entity<VehiculeServiceLLD>()
                .HasOne(vs => vs.Vehicule)
                .WithMany(v => v.VehiculeServices)
                .HasForeignKey(vs => vs.IdVehicule);

            modelBuilder.Entity<VehiculeServiceLLD>()
                .HasOne(vs => vs.ServiceLLD)
                .WithMany(s => s.VehiculeServices)
                .HasForeignKey(vs => vs.IdService);
        }
    }
}
