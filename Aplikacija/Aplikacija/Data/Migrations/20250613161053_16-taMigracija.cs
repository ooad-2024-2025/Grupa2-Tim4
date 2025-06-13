using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class _16taMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

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
                name: "FK_PrijaveNaTermine_AspNetUsers_ClanId",
                table: "PrijaveNaTermine");

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
                name: "IX_Trening_TrenerId",
                table: "Trening");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrenerId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Vrijeme",
                table: "Trening");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Korisnik");

            migrationBuilder.RenameColumn(
                name: "Tip",
                table: "Trening",
                newName: "TerminId");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Trening",
                newName: "DatumKreiranja");

            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "Trening",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Napomene",
                table: "Trening",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanTreninga",
                table: "Trening",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PoslednaIzmena",
                table: "Trening",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Trening",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Vezba",
                columns: table => new
                {
                    IdVezba = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreningId = table.Column<int>(type: "int", nullable: false),
                    NazivVezbe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Serije = table.Column<int>(type: "int", nullable: false),
                    Ponavljanja = table.Column<int>(type: "int", nullable: false),
                    Tezina = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Trajanje = table.Column<TimeSpan>(type: "time", nullable: true),
                    Napomene = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Redosled = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vezba", x => x.IdVezba);
                    table.ForeignKey(
                        name: "FK_Vezba_Trening_TreningId",
                        column: x => x.TreningId,
                        principalTable: "Trening",
                        principalColumn: "IdTrening",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trening_KorisnikId",
                table: "Trening",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_TerminId",
                table: "Trening",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Vezba_TreningId",
                table: "Vezba",
                column: "TreningId");

            migrationBuilder.CreateIndex(
                name: "IX_Vezba_TreningId_Redosled",
                table: "Vezba",
                columns: new[] { "TreningId", "Redosled" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Korisnik_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Korisnik_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Korisnik_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Korisnik_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kupovina_Korisnik_KorisnikId",
                table: "Kupovina",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanIshrane_Korisnik_ClanId",
                table: "PlanIshrane",
                column: "ClanId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId",
                table: "PrijavaZaZaposljavanje",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijaveNaTermine_Korisnik_ClanId",
                table: "PrijaveNaTermine",
                column: "ClanId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Korisnik_ClanId",
                table: "Trening",
                column: "ClanId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Korisnik_KorisnikId",
                table: "Trening",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Termin_TerminId",
                table: "Trening",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "IdTermin",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Korisnik_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Korisnik_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Korisnik_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Korisnik_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Kupovina_Korisnik_KorisnikId",
                table: "Kupovina");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanIshrane_Korisnik_ClanId",
                table: "PlanIshrane");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId",
                table: "PrijavaZaZaposljavanje");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijaveNaTermine_Korisnik_ClanId",
                table: "PrijaveNaTermine");

            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Korisnik_TrenerId",
                table: "Termin");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Korisnik_ClanId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Korisnik_KorisnikId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Termin_TerminId",
                table: "Trening");

            migrationBuilder.DropTable(
                name: "Vezba");

            migrationBuilder.DropIndex(
                name: "IX_Trening_KorisnikId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_TerminId",
                table: "Trening");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Napomene",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "PlanTreninga",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "PoslednaIzmena",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trening");

            migrationBuilder.RenameTable(
                name: "Korisnik",
                newName: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TerminId",
                table: "Trening",
                newName: "Tip");

            migrationBuilder.RenameColumn(
                name: "DatumKreiranja",
                table: "Trening",
                newName: "Datum");

            migrationBuilder.AddColumn<string>(
                name: "TrenerId",
                table: "Trening",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Vrijeme",
                table: "Trening",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_TrenerId",
                table: "Trening",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_PrijaveNaTermine_AspNetUsers_ClanId",
                table: "PrijaveNaTermine",
                column: "ClanId",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening",
                column: "TrenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
