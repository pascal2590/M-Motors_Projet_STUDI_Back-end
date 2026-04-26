using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace m_motors_API.Migrations
{
    /// <inheritdoc />
    public partial class InitLLD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_document_dossier_DossierId",
                table: "document");

            migrationBuilder.DropForeignKey(
                name: "FK_dossier_client_ClientId",
                table: "dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_dossier_vehicule_VehiculeId",
                table: "dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_dossier_DossierId",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_utilisateur_UserId",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicule_service_lld_service_lld_IdService",
                table: "vehicule_service_lld");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicule_service_lld_vehicule_IdVehicule",
                table: "vehicule_service_lld");

            migrationBuilder.RenameColumn(
                name: "IdService",
                table: "vehicule_service_lld",
                newName: "id_service");

            migrationBuilder.RenameColumn(
                name: "IdVehicule",
                table: "vehicule_service_lld",
                newName: "id_vehicule");

            migrationBuilder.RenameIndex(
                name: "IX_vehicule_service_lld_IdService",
                table: "vehicule_service_lld",
                newName: "IX_vehicule_service_lld_id_service");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "suivi_dossier",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "DossierId",
                table: "suivi_dossier",
                newName: "dossier_id");

            migrationBuilder.RenameColumn(
                name: "IdSuivi",
                table: "suivi_dossier",
                newName: "id_suivi");

            migrationBuilder.RenameIndex(
                name: "IX_suivi_dossier_UserId",
                table: "suivi_dossier",
                newName: "IX_suivi_dossier_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_suivi_dossier_DossierId",
                table: "suivi_dossier",
                newName: "IX_suivi_dossier_dossier_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "service_lld",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "NomService",
                table: "service_lld",
                newName: "nom_service");

            migrationBuilder.RenameColumn(
                name: "IdService",
                table: "service_lld",
                newName: "id_service");

            migrationBuilder.RenameColumn(
                name: "Statut",
                table: "dossier",
                newName: "statut");

            migrationBuilder.RenameColumn(
                name: "VehiculeId",
                table: "dossier",
                newName: "vehicule_id");

            migrationBuilder.RenameColumn(
                name: "TypeDossier",
                table: "dossier",
                newName: "type_dossier");

            migrationBuilder.RenameColumn(
                name: "DateCreation",
                table: "dossier",
                newName: "date_creation");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "dossier",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "IdDossier",
                table: "dossier",
                newName: "id_dossier");

            migrationBuilder.RenameIndex(
                name: "IX_dossier_VehiculeId",
                table: "dossier",
                newName: "IX_dossier_vehicule_id");

            migrationBuilder.RenameIndex(
                name: "IX_dossier_ClientId",
                table: "dossier",
                newName: "IX_dossier_client_id");

            migrationBuilder.RenameColumn(
                name: "NomDocument",
                table: "document",
                newName: "nom_document");

            migrationBuilder.RenameColumn(
                name: "DossierId",
                table: "document",
                newName: "dossier_id");

            migrationBuilder.RenameColumn(
                name: "DateUpload",
                table: "document",
                newName: "date_upload");

            migrationBuilder.RenameColumn(
                name: "CheminFichier",
                table: "document",
                newName: "chemin_fichier");

            migrationBuilder.RenameColumn(
                name: "IdDocument",
                table: "document",
                newName: "id_document");

            migrationBuilder.RenameIndex(
                name: "IX_document_DossierId",
                table: "document",
                newName: "IX_document_dossier_id");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "client",
                newName: "telephone");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "client",
                newName: "prenom");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "client",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "client",
                newName: "nom");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "client",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Adresse",
                table: "client",
                newName: "adresse");

            migrationBuilder.RenameColumn(
                name: "IdClient",
                table: "client",
                newName: "id_client");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vehicule",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "vehicule",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "statut",
                table: "dossier",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "type_dossier",
                table: "dossier",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "nom_document",
                table: "document",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_upload",
                table: "document",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "chemin_fichier",
                table: "document",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "type_document",
                table: "document",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dossier_financement",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dossier_id = table.Column<int>(type: "int", nullable: false),
                    Apport = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Financement = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Duree = table.Column<int>(type: "int", nullable: true),
                    Kilometrage = table.Column<int>(type: "int", nullable: true),
                    Mensualite = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dossier_financement", x => x.id);
                    table.ForeignKey(
                        name: "FK_dossier_financement_dossier_dossier_id",
                        column: x => x.dossier_id,
                        principalTable: "dossier",
                        principalColumn: "id_dossier",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dossier_service_lld",
                columns: table => new
                {
                    IdDossier = table.Column<int>(type: "int", nullable: false),
                    IdService = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dossier_service_lld", x => new { x.IdDossier, x.IdService });
                    table.ForeignKey(
                        name: "FK_dossier_service_lld_dossier_IdDossier",
                        column: x => x.IdDossier,
                        principalTable: "dossier",
                        principalColumn: "id_dossier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dossier_service_lld_service_lld_IdService",
                        column: x => x.IdService,
                        principalTable: "service_lld",
                        principalColumn: "id_service",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_dossier_financement_dossier_id",
                table: "dossier_financement",
                column: "dossier_id");

            migrationBuilder.CreateIndex(
                name: "IX_dossier_service_lld_IdService",
                table: "dossier_service_lld",
                column: "IdService");

            migrationBuilder.AddForeignKey(
                name: "FK_document_dossier_dossier_id",
                table: "document",
                column: "dossier_id",
                principalTable: "dossier",
                principalColumn: "id_dossier");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_client_client_id",
                table: "dossier",
                column: "client_id",
                principalTable: "client",
                principalColumn: "id_client");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_vehicule_vehicule_id",
                table: "dossier",
                column: "vehicule_id",
                principalTable: "vehicule",
                principalColumn: "id_vehicule");

            migrationBuilder.AddForeignKey(
                name: "FK_suivi_dossier_dossier_dossier_id",
                table: "suivi_dossier",
                column: "dossier_id",
                principalTable: "dossier",
                principalColumn: "id_dossier");

            migrationBuilder.AddForeignKey(
                name: "FK_suivi_dossier_utilisateur_user_id",
                table: "suivi_dossier",
                column: "user_id",
                principalTable: "utilisateur",
                principalColumn: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicule_service_lld_service_lld_id_service",
                table: "vehicule_service_lld",
                column: "id_service",
                principalTable: "service_lld",
                principalColumn: "id_service",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicule_service_lld_vehicule_id_vehicule",
                table: "vehicule_service_lld",
                column: "id_vehicule",
                principalTable: "vehicule",
                principalColumn: "id_vehicule",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_document_dossier_dossier_id",
                table: "document");

            migrationBuilder.DropForeignKey(
                name: "FK_dossier_client_client_id",
                table: "dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_dossier_vehicule_vehicule_id",
                table: "dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_dossier_dossier_id",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_utilisateur_user_id",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicule_service_lld_service_lld_id_service",
                table: "vehicule_service_lld");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicule_service_lld_vehicule_id_vehicule",
                table: "vehicule_service_lld");

            migrationBuilder.DropTable(
                name: "dossier_financement");

            migrationBuilder.DropTable(
                name: "dossier_service_lld");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "vehicule");

            migrationBuilder.DropColumn(
                name: "type_document",
                table: "document");

            migrationBuilder.RenameColumn(
                name: "id_service",
                table: "vehicule_service_lld",
                newName: "IdService");

            migrationBuilder.RenameColumn(
                name: "id_vehicule",
                table: "vehicule_service_lld",
                newName: "IdVehicule");

            migrationBuilder.RenameIndex(
                name: "IX_vehicule_service_lld_id_service",
                table: "vehicule_service_lld",
                newName: "IX_vehicule_service_lld_IdService");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "suivi_dossier",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "dossier_id",
                table: "suivi_dossier",
                newName: "DossierId");

            migrationBuilder.RenameColumn(
                name: "id_suivi",
                table: "suivi_dossier",
                newName: "IdSuivi");

            migrationBuilder.RenameIndex(
                name: "IX_suivi_dossier_user_id",
                table: "suivi_dossier",
                newName: "IX_suivi_dossier_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_suivi_dossier_dossier_id",
                table: "suivi_dossier",
                newName: "IX_suivi_dossier_DossierId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "service_lld",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "nom_service",
                table: "service_lld",
                newName: "NomService");

            migrationBuilder.RenameColumn(
                name: "id_service",
                table: "service_lld",
                newName: "IdService");

            migrationBuilder.RenameColumn(
                name: "statut",
                table: "dossier",
                newName: "Statut");

            migrationBuilder.RenameColumn(
                name: "vehicule_id",
                table: "dossier",
                newName: "VehiculeId");

            migrationBuilder.RenameColumn(
                name: "type_dossier",
                table: "dossier",
                newName: "TypeDossier");

            migrationBuilder.RenameColumn(
                name: "date_creation",
                table: "dossier",
                newName: "DateCreation");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "dossier",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "id_dossier",
                table: "dossier",
                newName: "IdDossier");

            migrationBuilder.RenameIndex(
                name: "IX_dossier_vehicule_id",
                table: "dossier",
                newName: "IX_dossier_VehiculeId");

            migrationBuilder.RenameIndex(
                name: "IX_dossier_client_id",
                table: "dossier",
                newName: "IX_dossier_ClientId");

            migrationBuilder.RenameColumn(
                name: "nom_document",
                table: "document",
                newName: "NomDocument");

            migrationBuilder.RenameColumn(
                name: "dossier_id",
                table: "document",
                newName: "DossierId");

            migrationBuilder.RenameColumn(
                name: "date_upload",
                table: "document",
                newName: "DateUpload");

            migrationBuilder.RenameColumn(
                name: "chemin_fichier",
                table: "document",
                newName: "CheminFichier");

            migrationBuilder.RenameColumn(
                name: "id_document",
                table: "document",
                newName: "IdDocument");

            migrationBuilder.RenameIndex(
                name: "IX_document_dossier_id",
                table: "document",
                newName: "IX_document_DossierId");

            migrationBuilder.RenameColumn(
                name: "telephone",
                table: "client",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "prenom",
                table: "client",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "client",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nom",
                table: "client",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "client",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "adresse",
                table: "client",
                newName: "Adresse");

            migrationBuilder.RenameColumn(
                name: "id_client",
                table: "client",
                newName: "IdClient");

            migrationBuilder.UpdateData(
                table: "vehicule",
                keyColumn: "description",
                keyValue: null,
                column: "description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vehicule",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Statut",
                table: "dossier",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "TypeDossier",
                table: "dossier",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "document",
                keyColumn: "NomDocument",
                keyValue: null,
                column: "NomDocument",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NomDocument",
                table: "document",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpload",
                table: "document",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "document",
                keyColumn: "CheminFichier",
                keyValue: null,
                column: "CheminFichier",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CheminFichier",
                table: "document",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_document_dossier_DossierId",
                table: "document",
                column: "DossierId",
                principalTable: "dossier",
                principalColumn: "IdDossier");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_client_ClientId",
                table: "dossier",
                column: "ClientId",
                principalTable: "client",
                principalColumn: "IdClient");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_vehicule_VehiculeId",
                table: "dossier",
                column: "VehiculeId",
                principalTable: "vehicule",
                principalColumn: "id_vehicule");

            migrationBuilder.AddForeignKey(
                name: "FK_suivi_dossier_dossier_DossierId",
                table: "suivi_dossier",
                column: "DossierId",
                principalTable: "dossier",
                principalColumn: "IdDossier");

            migrationBuilder.AddForeignKey(
                name: "FK_suivi_dossier_utilisateur_UserId",
                table: "suivi_dossier",
                column: "UserId",
                principalTable: "utilisateur",
                principalColumn: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicule_service_lld_service_lld_IdService",
                table: "vehicule_service_lld",
                column: "IdService",
                principalTable: "service_lld",
                principalColumn: "IdService",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicule_service_lld_vehicule_IdVehicule",
                table: "vehicule_service_lld",
                column: "IdVehicule",
                principalTable: "vehicule",
                principalColumn: "id_vehicule",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
