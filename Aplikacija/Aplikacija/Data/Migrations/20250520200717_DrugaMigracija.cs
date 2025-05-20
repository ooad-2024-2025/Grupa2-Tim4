using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class DrugaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Termin",
                newName: "IdTermin");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Korisnik",
                newName: "IdKorisnik");

            migrationBuilder.CreateTable(
                name: "Kupovina",
                columns: table => new
                {
                    IdKupovina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumKupovine = table.Column<DateOnly>(type: "date", nullable: false),
                    Artikal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cijena = table.Column<float>(type: "real", nullable: false),
                    Racun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdKorisnik = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupovina", x => x.IdKupovina);
                    table.ForeignKey(
                        name: "FK_Kupovina_Korisnik_IdKorisnik",
                        column: x => x.IdKorisnik,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanIshrane",
                columns: table => new
                {
                    IdPlanishrane = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ciljevi = table.Column<int>(type: "int", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumGenerisanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kilaza = table.Column<double>(type: "float", nullable: false),
                    Godine = table.Column<int>(type: "int", nullable: false),
                    ClanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanIshrane", x => x.IdPlanishrane);
                    table.ForeignKey(
                        name: "FK_PlanIshrane_Korisnik_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavaZaZaposljavanje",
                columns: table => new
                {
                    IdPrijava = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pregledano = table.Column<bool>(type: "bit", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavaZaZaposljavanje", x => x.IdPrijava);
                    table.ForeignKey(
                        name: "FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trening",
                columns: table => new
                {
                    IdTrening = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vrijeme = table.Column<TimeSpan>(type: "time", nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    ClanId = table.Column<int>(type: "int", nullable: false),
                    TrenerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trening", x => x.IdTrening);
                    table.ForeignKey(
                        name: "FK_Trening_Korisnik_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnik",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trening_Korisnik_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnik",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kupovina_IdKorisnik",
                table: "Kupovina",
                column: "IdKorisnik");

            migrationBuilder.CreateIndex(
                name: "IX_PlanIshrane_ClanId",
                table: "PlanIshrane",
                column: "ClanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaZaZaposljavanje_KorisnikId",
                table: "PrijavaZaZaposljavanje",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_ClanId",
                table: "Trening",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_TrenerId",
                table: "Trening",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnik",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin");

            migrationBuilder.DropTable(
                name: "Kupovina");

            migrationBuilder.DropTable(
                name: "PlanIshrane");

            migrationBuilder.DropTable(
                name: "PrijavaZaZaposljavanje");

            migrationBuilder.DropTable(
                name: "Trening");

            migrationBuilder.RenameColumn(
                name: "IdTermin",
                table: "Termin",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdKorisnik",
                table: "Korisnik",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
