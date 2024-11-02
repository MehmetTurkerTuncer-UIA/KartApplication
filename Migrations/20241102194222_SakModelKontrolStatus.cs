using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class SakModelKontrolStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KontrolStatus",
                table: "SakModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KontrolStatus",
                table: "SakModels");
        }
    }
}
