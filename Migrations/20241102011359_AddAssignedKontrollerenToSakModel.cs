using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedKontrollerenToSakModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedKontrollerenId",
                table: "SakModels",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SakModels_AssignedKontrollerenId",
                table: "SakModels",
                column: "AssignedKontrollerenId");

            migrationBuilder.AddForeignKey(
                name: "FK_SakModels_AspNetUsers_AssignedKontrollerenId",
                table: "SakModels",
                column: "AssignedKontrollerenId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SakModels_AspNetUsers_AssignedKontrollerenId",
                table: "SakModels");

            migrationBuilder.DropIndex(
                name: "IX_SakModels_AssignedKontrollerenId",
                table: "SakModels");

            migrationBuilder.DropColumn(
                name: "AssignedKontrollerenId",
                table: "SakModels");
        }
    }
}
