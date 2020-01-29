using Microsoft.EntityFrameworkCore.Migrations;

namespace OSRSCharts.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 500, nullable: true),
                    ItemMembers = table.Column<bool>(nullable: false),
                    ItemTradeable = table.Column<bool>(nullable: false),
                    ItemHighalch = table.Column<int>(nullable: true),
                    ItemBuyLimit = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
