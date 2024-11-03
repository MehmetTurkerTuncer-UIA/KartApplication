using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class SakModelUpdateSaksbehandlerKontrollerenDescriptionForString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KontrollerensDescription",
                table: "SakModels");

            migrationBuilder.DropColumn(
                name: "SaksbehandlersDescription",
                table: "SakModels");

            migrationBuilder.AddColumn<string>(
                name: "KontrollerenDescription",
                table: "SakModels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SaksBehandlerDescription",
                table: "SakModels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KontrollerenDescription",
                table: "SakModels");

            migrationBuilder.DropColumn(
                name: "SaksBehandlerDescription",
                table: "SakModels");

            migrationBuilder.AddColumn<string>(
                name: "KontrollerensDescription",
                table: "SakModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SaksbehandlersDescription",
                table: "SakModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
