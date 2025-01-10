using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWithMe.Migrations
{
    public partial class AddLeadToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lead",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lead",
                table: "Products");
        }
    }
}
