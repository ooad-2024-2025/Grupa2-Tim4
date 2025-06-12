using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class _12taMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Obrok",
                columns: table => new
                {
                    IdObrok = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BrojKalorija = table.Column<int>(type: "int", nullable: false),
                    PlanIshraneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obrok", x => x.IdObrok);
                    table.ForeignKey(
                        name: "FK_Obrok_PlanIshrane_PlanIshraneId",
                        column: x => x.PlanIshraneId,
                        principalTable: "PlanIshrane",
                        principalColumn: "IdPlanishrane",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Obrok_PlanIshraneId",
                table: "Obrok",
                column: "PlanIshraneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obrok");
        }
    }
}
