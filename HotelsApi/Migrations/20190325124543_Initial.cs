using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelsApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    HotelName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 2048, nullable: true),
                    Country = table.Column<string>(maxLength: 64, nullable: false),
                    City = table.Column<string>(maxLength: 64, nullable: false),
                    Address = table.Column<string>(maxLength: 128, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 64, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
