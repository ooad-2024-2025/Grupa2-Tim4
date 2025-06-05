using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class DevetaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kupovina_Korisnik_IdKorisnik",
                table: "Kupovina");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanIshrane_Korisnik_ClanId",
                table: "PlanIshrane");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId",
                table: "PrijavaZaZaposljavanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Korisnik_ClanId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Korisnik_TrenerId",
                table: "Trening");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropIndex(
                name: "IX_Kupovina_IdKorisnik",
                table: "Kupovina");

            migrationBuilder.DropColumn(
                name: "IdKorisnik",
                table: "Kupovina");

            migrationBuilder.AlterColumn<string>(
                name: "TrenerId",
                table: "Trening",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClanId",
                table: "Trening",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TrenerId",
                table: "Termin",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "PrijavaZaZaposljavanje",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClanId",
                table: "PlanIshrane",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "Kupovina",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Tip",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Kupovina_KorisnikId",
                table: "Kupovina",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kupovina_AspNetUsers_KorisnikId",
                table: "Kupovina",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanIshrane_AspNetUsers_ClanId",
                table: "PlanIshrane",
                column: "ClanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavaZaZaposljavanje_AspNetUsers_KorisnikId",
                table: "PrijavaZaZaposljavanje",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_AspNetUsers_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_AspNetUsers_ClanId",
                table: "Trening",
                column: "ClanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening",
                column: "TrenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kupovina_AspNetUsers_KorisnikId",
                table: "Kupovina");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanIshrane_AspNetUsers_ClanId",
                table: "PlanIshrane");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijavaZaZaposljavanje_AspNetUsers_KorisnikId",
                table: "PrijavaZaZaposljavanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Termin_AspNetUsers_TrenerId",
                table: "Termin");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_ClanId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Kupovina_KorisnikId",
                table: "Kupovina");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Kupovina");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "TrenerId",
                table: "Trening",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ClanId",
                table: "Trening",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrenerId",
                table: "Termin",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "PrijavaZaZaposljavanje",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ClanId",
                table: "PlanIshrane",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "IdKorisnik",
                table: "Kupovina",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    IdKorisnik = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.IdKorisnik);
                    table.ForeignKey(
                        name: "FK_Korisnik_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kupovina_IdKorisnik",
                table: "Kupovina",
                column: "IdKorisnik");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_IdentityUserId",
                table: "Korisnik",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kupovina_Korisnik_IdKorisnik",
                table: "Kupovina",
                column: "IdKorisnik",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanIshrane_Korisnik_ClanId",
                table: "PlanIshrane",
                column: "ClanId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId",
                table: "PrijavaZaZaposljavanje",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Korisnik_ClanId",
                table: "Trening",
                column: "ClanId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Korisnik_TrenerId",
                table: "Trening",
                column: "TrenerId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
