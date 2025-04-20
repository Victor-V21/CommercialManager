using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductNameToSalesDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "SalesDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_name",
                table: "SalesDetails");
        }
    }
}
