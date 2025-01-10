using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWithMe.Migrations
{
    public partial class AddMainImageToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MainImageId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MainImageId",
                table: "Products",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Files_MainImageId",
                table: "Products",
                column: "MainImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Files_MainImageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MainImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Products");
        }
    }
}
