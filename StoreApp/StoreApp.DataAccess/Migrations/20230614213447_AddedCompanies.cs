using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.DataAccess.Migrations
{
    public partial class AddedCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "County", "Name", "PhoneNumber", "PostalCode" },
                values: new object[] { 1, "Bulevardul General Paul Teodorescu 4", "București", "București", "Carturesti SRL", "0723451234", "061344" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "County", "Name", "PhoneNumber", "PostalCode" },
                values: new object[] { 2, "Str. Independentei 20", "București", "București", "SC Library", "0723321234", "061324" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "County", "Name", "PhoneNumber", "PostalCode" },
                values: new object[] { 3, "Bulevardul Vasile Milea 32", "București", "București", "Book Comp", "0723481234", "061321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Companies",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
