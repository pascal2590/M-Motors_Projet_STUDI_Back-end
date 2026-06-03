using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace m_motors_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCommercialToDossier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dossier_financement_dossier_dossier_id",
                table: "dossier_financement");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_dossier_dossier_id",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_utilisateur_user_id",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_utilisateur_role_RoleId",
                table: "utilisateur");

            migrationBuilder.DropTable(
                name: "dossier_service_lld");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "utilisateur",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "utilisateur",
                newName: "nom");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "utilisateur",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "utilisateur",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "utilisateur",
                newName: "id_user");

            migrationBuilder.RenameIndex(
                name: "IX_utilisateur_RoleId",
                table: "utilisateur",
                newName: "IX_utilisateur_role_id");

            migrationBuilder.RenameColumn(
                name: "DateModification",
                table: "suivi_dossier",
                newName: "date_modification");

            migrationBuilder.RenameColumn(
                name: "NomRole",
                table: "role",
                newName: "nom_role");

            migrationBuilder.RenameColumn(
                name: "IdRole",
                table: "role",
                newName: "id_role");

            migrationBuilder.AddColumn<string>(
                name: "prenom",
                table: "utilisateur",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "dossier_financement",
                keyColumn: "Financement",
                keyValue: null,
                column: "Financement",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Financement",
                table: "dossier_financement",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "commercial_id",
                table: "dossier",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "client",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "message_client",
                columns: table => new
                {
                    id_message = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    sujet = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contenu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    date_envoi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_client", x => x.id_message);
                    table.ForeignKey(
                        name: "FK_message_client_client_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "id_client",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_dossier_commercial_id",
                table: "dossier",
                column: "commercial_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_email",
                table: "client",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_client_client_id",
                table: "message_client",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_utilisateur_commercial_id",
                table: "dossier",
                column: "commercial_id",
                principalTable: "utilisateur",
                principalColumn: "id_user");

            migrationBuilder.AddForeignKey(
                name: "fk_dossier_financement",
                table: "dossier_financement",
                column: "dossier_id",
                principalTable: "dossier",
                principalColumn: "id_dossier",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_suivi_dossier_dossier_dossier_id",
                table: "suivi_dossier",
                column: "dossier_id",
                principalTable: "dossier",
                principalColumn: "id_dossier",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_suivi_utilisateur",
                table: "suivi_dossier",
                column: "user_id",
                principalTable: "utilisateur",
                principalColumn: "id_user",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_utilisateur_role_role_id",
                table: "utilisateur",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id_role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dossier_utilisateur_commercial_id",
                table: "dossier");

            migrationBuilder.DropForeignKey(
                name: "fk_dossier_financement",
                table: "dossier_financement");

            migrationBuilder.DropForeignKey(
                name: "FK_suivi_dossier_dossier_dossier_id",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "fk_suivi_utilisateur",
                table: "suivi_dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_utilisateur_role_role_id",
                table: "utilisateur");

            migrationBuilder.DropTable(
                name: "message_client");

            migrationBuilder.DropIndex(
                name: "IX_dossier_commercial_id",
                table: "dossier");

            migrationBuilder.DropIndex(
                name: "IX_client_email",
                table: "client");

            migrationBuilder.DropColumn(
                name: "prenom",
                table: "utilisateur");

            migrationBuilder.DropColumn(
                name: "commercial_id",
                table: "dossier");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "utilisateur",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nom",
                table: "utilisateur",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "utilisateur",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "utilisateur",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "id_user",
                table: "utilisateur",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_utilisateur_role_id",
                table: "utilisateur",
                newName: "IX_utilisateur_RoleId");

            migrationBuilder.RenameColumn(
                name: "date_modification",
                table: "suivi_dossier",
                newName: "DateModification");

            migrationBuilder.RenameColumn(
                name: "nom_role",
                table: "role",
                newName: "NomRole");

            migrationBuilder.RenameColumn(
                name: "id_role",
                table: "role",
                newName: "IdRole");

            migrationBuilder.AlterColumn<decimal>(
                name: "Financement",
                table: "dossier_financement",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "client",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_dossier_service_lld_IdService",
                table: "dossier_service_lld",
                column: "IdService");

            migrationBuilder.AddForeignKey(
                name: "FK_dossier_financement_dossier_dossier_id",
                table: "dossier_financement",
                column: "dossier_id",
                principalTable: "dossier",
                principalColumn: "id_dossier",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_utilisateur_role_RoleId",
                table: "utilisateur",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "IdRole");
        }
    }
}
