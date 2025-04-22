using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialManager.API.Migrations
{
    /// <inheritdoc />
    public partial class SalesDiscountIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "SalesDetails",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount",
                table: "SalesDetails");
        }
    }
}
