using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace m_motors_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDateUploadToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateUpload = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.IdClient);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomRole = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.IdRole);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_lld",
                columns: table => new
                {
                    IdService = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomService = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_lld", x => x.IdService);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicule",
                columns: table => new
                {
                    id_vehicule = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    marque = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modele = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    annee = table.Column<int>(type: "int", nullable: true),
                    kilometrage = table.Column<int>(type: "int", nullable: true),
                    prix = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type_offre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    disponible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_ajout = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicule", x => x.id_vehicule);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "utilisateur",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilisateur", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_utilisateur_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "IdRole");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dossier",
                columns: table => new
                {
                    IdDossier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeDossier = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    VehiculeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dossier", x => x.IdDossier);
                    table.ForeignKey(
                        name: "FK_dossier_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "IdClient");
                    table.ForeignKey(
                        name: "FK_dossier_vehicule_VehiculeId",
                        column: x => x.VehiculeId,
                        principalTable: "vehicule",
                        principalColumn: "id_vehicule");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicule_service_lld",
                columns: table => new
                {
                    IdVehicule = table.Column<int>(type: "int", nullable: false),
                    IdService = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicule_service_lld", x => new { x.IdVehicule, x.IdService });
                    table.ForeignKey(
                        name: "FK_vehicule_service_lld_service_lld_IdService",
                        column: x => x.IdService,
                        principalTable: "service_lld",
                        principalColumn: "IdService",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicule_service_lld_vehicule_IdVehicule",
                        column: x => x.IdVehicule,
                        principalTable: "vehicule",
                        principalColumn: "id_vehicule",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "document",
                columns: table => new
                {
                    IdDocument = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomDocument = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheminFichier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateUpload = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DossierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document", x => x.IdDocument);
                    table.ForeignKey(
                        name: "FK_document_dossier_DossierId",
                        column: x => x.DossierId,
                        principalTable: "dossier",
                        principalColumn: "IdDossier");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "suivi_dossier",
                columns: table => new
                {
                    IdSuivi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateModification = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DossierId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suivi_dossier", x => x.IdSuivi);
                    table.ForeignKey(
                        name: "FK_suivi_dossier_dossier_DossierId",
                        column: x => x.DossierId,
                        principalTable: "dossier",
                        principalColumn: "IdDossier");
                    table.ForeignKey(
                        name: "FK_suivi_dossier_utilisateur_UserId",
                        column: x => x.UserId,
                        principalTable: "utilisateur",
                        principalColumn: "IdUser");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_document_DossierId",
                table: "document",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_dossier_ClientId",
                table: "dossier",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_dossier_VehiculeId",
                table: "dossier",
                column: "VehiculeId");

            migrationBuilder.CreateIndex(
                name: "IX_suivi_dossier_DossierId",
                table: "suivi_dossier",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_suivi_dossier_UserId",
                table: "suivi_dossier",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_RoleId",
                table: "utilisateur",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicule_service_lld_IdService",
                table: "vehicule_service_lld",
                column: "IdService");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document");

            migrationBuilder.DropTable(
                name: "suivi_dossier");

            migrationBuilder.DropTable(
                name: "vehicule_service_lld");

            migrationBuilder.DropTable(
                name: "dossier");

            migrationBuilder.DropTable(
                name: "utilisateur");

            migrationBuilder.DropTable(
                name: "service_lld");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "vehicule");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
