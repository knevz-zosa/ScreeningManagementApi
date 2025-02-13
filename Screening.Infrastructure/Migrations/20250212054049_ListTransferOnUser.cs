using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Screening.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ListTransferOnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Transfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ApplicationUserId",
                table: "Transfers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AspNetUsers_ApplicationUserId",
                table: "Transfers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AspNetUsers_ApplicationUserId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ApplicationUserId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Transfers");
        }
    }
}
