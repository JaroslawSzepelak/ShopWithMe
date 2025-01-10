using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWithMe.Migrations
{
    public partial class AddProductImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StorageFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => new { x.ProductId, x.StorageFileId });
                    table.ForeignKey(
                        name: "FK_ProductImages_Files_StorageFileId",
                        column: x => x.StorageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_StorageFileId",
                table: "ProductImages",
                column: "StorageFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");
        }
    }
}
