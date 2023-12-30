using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTour.Migrations
{
    public partial class updateDB_29122023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FocusDay",
                table: "Tours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FocusPlace",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HDVName",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "doubleBed",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "singleBed",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FocusDay",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "FocusPlace",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "HDVName",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "doubleBed",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "singleBed",
                table: "Hotel");
        }
    }
}
