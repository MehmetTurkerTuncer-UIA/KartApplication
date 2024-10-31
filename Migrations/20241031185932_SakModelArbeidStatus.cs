using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class SakModelArbeidStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArbeidStatus",
                table: "SakModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArbeidStatus",
                table: "SakModels");
        }
    }
}
