using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.DataAccess.Migrations
{
    public partial class AddedStripeIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderHeader");
        }
    }
}
