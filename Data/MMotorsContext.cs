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

        // TABLES
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
        public DbSet<MessageClient> MessagesClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region MESSAGE_CLIENT
            modelBuilder.Entity<MessageClient>().ToTable("message_client").HasKey(m => m.IdMessage);
            modelBuilder.Entity<MessageClient>().HasOne(m => m.Client).WithMany(c => c.MessagesClients).HasForeignKey(m => m.ClientId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageClient>().Property(m => m.IdMessage).HasColumnName("id_message");
            modelBuilder.Entity<MessageClient>().Property(m => m.ClientId).HasColumnName("client_id");
            modelBuilder.Entity<MessageClient>().Property(m => m.Sujet).HasColumnName("sujet");
            modelBuilder.Entity<MessageClient>().Property(m => m.Contenu).HasColumnName("contenu");
            modelBuilder.Entity<MessageClient>().Property(m => m.Lu).HasColumnName("lu");
            modelBuilder.Entity<MessageClient>().Property(m => m.DateEnvoi).HasColumnName("date_envoi");

            modelBuilder.Entity<MessageClient>()
                .HasOne(m => m.Client)
                .WithMany(c => c.MessagesClients)
                .HasForeignKey(m => m.ClientId);
            #endregion

            #region CLIENT
            modelBuilder.Entity<Client>()
                .ToTable("client")
                .HasKey(c => c.IdClient);
            modelBuilder.Entity<Client>().HasIndex(c => c.Email).IsUnique();
            #endregion

            #region DOCUMENT
            modelBuilder.Entity<DocumentClient>()
                .ToTable("document")
                .HasKey(d => d.IdDocument);

            modelBuilder.Entity<DocumentClient>().Property(d => d.IdDocument).HasColumnName("id_document");
            modelBuilder.Entity<DocumentClient>().Property(d => d.NomDocument).HasColumnName("nom_document");
            modelBuilder.Entity<DocumentClient>().Property(d => d.TypeDocument).HasColumnName("type_document");
            modelBuilder.Entity<DocumentClient>().Property(d => d.CheminFichier).HasColumnName("chemin_fichier");
            modelBuilder.Entity<DocumentClient>().Property(d => d.DateAjout).HasColumnName("date_upload");
            modelBuilder.Entity<DocumentClient>().Property(d => d.DossierId).HasColumnName("dossier_id");

            modelBuilder.Entity<DocumentClient>()
                .HasOne(d => d.Dossier)
                .WithMany(d => d.Documents)
                .HasForeignKey(d => d.DossierId);
            #endregion

            #region DOSSIER
            modelBuilder.Entity<Dossier>()
                .ToTable("dossier")
                .HasKey(d => d.IdDossier);

            modelBuilder.Entity<Dossier>().Property(d => d.IdDossier).HasColumnName("id_dossier");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.TypeDossier)
                .HasConversion<string>()
                .HasColumnName("type_dossier");

            modelBuilder.Entity<Dossier>()
                .Property(d => d.Statut)
                .HasConversion<string>()
                .HasColumnName("statut");

            modelBuilder.Entity<Dossier>().Property(d => d.DateCreation).HasColumnName("date_creation");
            modelBuilder.Entity<Dossier>().Property(d => d.ClientId).HasColumnName("client_id");
            modelBuilder.Entity<Dossier>().Property(d => d.VehiculeId).HasColumnName("vehicule_id");

            modelBuilder.Entity<Dossier>()
                .HasOne(d => d.Client)
                .WithMany(c => c.Dossiers)
                .HasForeignKey(d => d.ClientId);

            modelBuilder.Entity<Dossier>()
                .HasOne(d => d.Vehicule)
                .WithMany(v => v.Dossiers)
                .HasForeignKey(d => d.VehiculeId);
            #endregion

            #region ROLE
            modelBuilder.Entity<Role>()
                .ToTable("role")
                .HasKey(r => r.IdRole);

            modelBuilder.Entity<Role>().Property(r => r.IdRole).HasColumnName("id_role");
            modelBuilder.Entity<Role>().Property(r => r.NomRole).HasColumnName("nom_role");
            #endregion

            #region UTILISATEUR
            modelBuilder.Entity<Utilisateur>()
                .ToTable("utilisateur")
                .HasKey(u => u.IdUser);

            modelBuilder.Entity<Utilisateur>().Property(u => u.IdUser).HasColumnName("id_user");
            modelBuilder.Entity<Utilisateur>().Property(u => u.Nom).HasColumnName("nom");
            modelBuilder.Entity<Utilisateur>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<Utilisateur>().Property(u => u.Password).HasColumnName("password");
            modelBuilder.Entity<Utilisateur>().Property(u => u.RoleId).HasColumnName("role_id");

            modelBuilder.Entity<Utilisateur>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Utilisateurs)
                .HasForeignKey(u => u.RoleId);
            #endregion

            #region VEHICULE
            modelBuilder.Entity<Vehicule>(entity =>
            {
                entity.ToTable("vehicule");
                entity.HasKey(v => v.IdVehicule);

                entity.Property(v => v.IdVehicule).HasColumnName("id_vehicule");
                entity.Property(v => v.Marque).HasColumnName("marque");
                entity.Property(v => v.Modele).HasColumnName("modele");
                entity.Property(v => v.Annee).HasColumnName("annee");
                entity.Property(v => v.Kilometrage).HasColumnName("kilometrage");
                entity.Property(v => v.Prix).HasColumnName("prix");
                entity.Property(v => v.Description).HasColumnName("description");

                entity.Property(v => v.TypeOffre)
                    .HasColumnName("type_offre")
                    .HasConversion<string>();

                entity.Property(v => v.Disponible).HasColumnName("disponible");
                entity.Property(v => v.DateAjout).HasColumnName("date_ajout");
                entity.Property(v => v.ImageUrl).HasColumnName("image_url");
            });
            #endregion

            #region SERVICE_LLD
            modelBuilder.Entity<ServiceLLD>()
                .ToTable("service_lld")
                .HasKey(s => s.IdService);

            modelBuilder.Entity<ServiceLLD>().Property(s => s.IdService).HasColumnName("id_service");
            modelBuilder.Entity<ServiceLLD>().Property(s => s.NomService).HasColumnName("nom_service");
            modelBuilder.Entity<ServiceLLD>().Property(s => s.Description).HasColumnName("description");
            #endregion

            #region VEHICULE_SERVICE_LLD (FIX DEFINITIF)
            modelBuilder.Entity<VehiculeServiceLLD>(entity =>
            {
                entity.ToTable("vehicule_service_lld");

                entity.HasKey(vs => new { vs.IdVehicule, vs.IdService });

                entity.Property(vs => vs.IdVehicule).HasColumnName("id_vehicule");
                entity.Property(vs => vs.IdService).HasColumnName("id_service");

                // Verrouillage des relations (évite ServiceLLDIdService)
                entity.HasOne(vs => vs.Vehicule)
                    .WithMany(v => v.VehiculeServices)
                    .HasForeignKey(vs => vs.IdVehicule)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(vs => vs.ServiceLLD)
                    .WithMany(s => s.VehiculeServices)
                    .HasForeignKey(vs => vs.IdService)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region DOSSIER_FINANCEMENT
            modelBuilder.Entity<DossierFinancement>().ToTable("dossier_financement").HasKey(df => df.Id);
            modelBuilder.Entity<DossierFinancement>().Property(df => df.Id).HasColumnName("id");
            modelBuilder.Entity<DossierFinancement>().Property(df => df.DossierId).HasColumnName("dossier_id");
            modelBuilder.Entity<DossierFinancement>().HasOne<Dossier>().WithOne().HasForeignKey<DossierFinancement>(df => df.DossierId).OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region SUIVI_DOSSIER
            modelBuilder.Entity<SuiviDossier>().ToTable("suivi_dossier").HasKey(s => s.IdSuivi);
            modelBuilder.Entity<SuiviDossier>().HasOne<Dossier>().WithMany().HasForeignKey(s => s.DossierId);
            modelBuilder.Entity<SuiviDossier>().HasOne<Utilisateur>().WithMany().HasForeignKey(s => s.UserId);

            modelBuilder.Entity<SuiviDossier>().Property(s => s.IdSuivi).HasColumnName("id_suivi");
            modelBuilder.Entity<SuiviDossier>().Property(s => s.DossierId).HasColumnName("dossier_id");
            modelBuilder.Entity<SuiviDossier>().Property(s => s.UserId).HasColumnName("user_id");
            #endregion
        }
    }
}