using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KartApplication.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackModel_SakModels_SakModelId",
                table: "FeedbackModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackModel",
                table: "FeedbackModel");

            migrationBuilder.RenameTable(
                name: "FeedbackModel",
                newName: "FeedbackModels");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackModel_SakModelId",
                table: "FeedbackModels",
                newName: "IX_FeedbackModels_SakModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackModels",
                table: "FeedbackModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackModels_SakModels_SakModelId",
                table: "FeedbackModels",
                column: "SakModelId",
                principalTable: "SakModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackModels_SakModels_SakModelId",
                table: "FeedbackModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackModels",
                table: "FeedbackModels");

            migrationBuilder.RenameTable(
                name: "FeedbackModels",
                newName: "FeedbackModel");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackModels_SakModelId",
                table: "FeedbackModel",
                newName: "IX_FeedbackModel_SakModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackModel",
                table: "FeedbackModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackModel_SakModels_SakModelId",
                table: "FeedbackModel",
                column: "SakModelId",
                principalTable: "SakModels",
                principalColumn: "Id");
        }
    }
}
