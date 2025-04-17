using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "ShoppingCarts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "ShoppingCarts");
        }
    }
}
