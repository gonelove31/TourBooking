using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTour.Migrations
{
    public partial class updateDBImage5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "ImageTour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageTour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageTour_Tours_TourID",
                        column: x => x.TourID,
                        principalTable: "Tours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageTour_TourID",
                table: "ImageTour",
                column: "TourID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageTour");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourID = table.Column<int>(type: "int", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Tours_TourID",
                        column: x => x.TourID,
                        principalTable: "Tours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_TourID",
                table: "Image",
                column: "TourID");
        }
    }
}
