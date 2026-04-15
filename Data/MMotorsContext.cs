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
        public DbSet<DossierFinancement> DossierFinancements { get; set; }


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
                .Property(d => d.IdDocument)
                .HasColumnName("id_document");

            modelBuilder.Entity<DocumentClient>()
                .HasOne(d => d.Dossier)
                .WithMany(ds => ds.Documents)
                .HasForeignKey(d => d.DossierId);

            modelBuilder.Entity<DocumentClient>()
                .Property(d => d.DossierId)
                .HasColumnName("dossier_id");

            //---------------------------------
            // Table DOSSIER
            //---------------------------------
            modelBuilder.Entity<Dossier>()
                .ToTable("dossier")
                .HasKey(d => d.IdDossier);

            modelBuilder.Entity<Dossier>()
                .Property(d => d.IdDossier)
                .HasColumnName("id_dossier");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.TypeDossier)
                .HasConversion<string>()
                .HasColumnName("type_dossier");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.Statut)
                .HasConversion<string>()
                .HasColumnName("statut");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.DateCreation)
                .HasColumnName("date_creation");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.ClientId)
                .HasColumnName("client_id");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.VehiculeId)
                .HasColumnName("vehicule_id");

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
                .Property(s => s.IdSuivi)
                .HasColumnName("id_suivi");

            modelBuilder.Entity<SuiviDossier>()
                .HasOne(s => s.Dossier)
                .WithMany(d => d.Suivis)
                .HasForeignKey(s => s.DossierId);

            modelBuilder.Entity<SuiviDossier>()
                .HasOne(s => s.Utilisateur)
                .WithMany(u => u.Suivis)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<SuiviDossier>()
                .Property(s => s.DossierId)
                .HasColumnName("dossier_id");

            modelBuilder.Entity<SuiviDossier>()
                .Property(s => s.UserId)
                .HasColumnName("user_id");


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

            //---------------------------------
            // Table DOSSIER_FINANCEMENT
            //---------------------------------
            modelBuilder.Entity<DossierFinancement>()
                .ToTable("dossier_financement");

            modelBuilder.Entity<DossierFinancement>()
                .HasKey(df => df.Id);

            modelBuilder.Entity<DossierFinancement>()
                .Property(df => df.Id)
                .HasColumnName("id");

            modelBuilder.Entity<DossierFinancement>()
                .Property(df => df.DossierId)
                .HasColumnName("dossier_id");

            modelBuilder.Entity<DossierFinancement>()
                .HasOne(df => df.Dossier)
                .WithMany(d => d.Financements)
                .HasForeignKey(df => df.DossierId);

        }
    }
}
