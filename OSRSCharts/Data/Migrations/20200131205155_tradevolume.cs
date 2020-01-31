using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSRSCharts.Data.Migrations
{
    public partial class tradevolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeVolume",
                columns: table => new
                {
                    TradeVolumeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    NumberOfTrades = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeVolume", x => x.TradeVolumeID);
                    table.ForeignKey(
                        name: "FK_TradeVolume_Item_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeVolume_ItemID",
                table: "TradeVolume",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeVolume");
        }
    }
}
