using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Data.Migrations
{
    /// <inheritdoc />
    public partial class _15TAMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_ClanId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening");

            // migrationBuilder.RenameColumn(
            //     name: "TipCilja",
            //     table: "PlanIshrane",
            //     newName: "Visina");


            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "Termin",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Plan",
                table: "PlanIshrane",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_AspNetUsers_ClanId",
                table: "Trening",
                column: "ClanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening",
                column: "TrenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_ClanId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_AspNetUsers_TrenerId",
                table: "Trening");

            migrationBuilder.RenameColumn(
                name: "Visina",
                table: "PlanIshrane",
                newName: "TipCilja");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Datum",
                table: "Termin",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Plan",
                table: "PlanIshrane",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
    }
}
