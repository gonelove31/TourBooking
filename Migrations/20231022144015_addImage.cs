using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTour.Migrations
{
    public partial class addImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Location_LocationID",
                table: "Tours");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Tours",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Location_LocationID",
                table: "Tours",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Location_LocationID",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Location_LocationID",
                table: "Tours",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
