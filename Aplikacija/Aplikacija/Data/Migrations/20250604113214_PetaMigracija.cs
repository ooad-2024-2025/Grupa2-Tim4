using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class PetaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipCilja",
                table: "PlanIshrane",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Korisnik",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_IdentityUserId",
                table: "Korisnik",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnik_AspNetUsers_IdentityUserId",
                table: "Korisnik",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnik_AspNetUsers_IdentityUserId",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "IX_Korisnik_IdentityUserId",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "TipCilja",
                table: "PlanIshrane");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Korisnik");
        }
    }
}
