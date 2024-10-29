using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class SakModelUpdateForApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SakModels",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SakModels_UserId",
                table: "SakModels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SakModels_AspNetUsers_UserId",
                table: "SakModels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SakModels_AspNetUsers_UserId",
                table: "SakModels");

            migrationBuilder.DropIndex(
                name: "IX_SakModels_UserId",
                table: "SakModels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SakModels");
        }
    }
}
