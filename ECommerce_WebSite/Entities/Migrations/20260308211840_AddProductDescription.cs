using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Price" },
                values: new object[] { "Apple MacBook Pro 16-inch with M3 Max chip, 32GB RAM, and 1TB SSD. Built for Apple Intelligence.", 125000.00m });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Price" },
                values: new object[] { "Dell XPS 15 OLED with Intel Core i9, 32GB RAM, 1TB SSD, and NVIDIA RTX 4070. Perfect for creators.", 95000.00m });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Price" },
                values: new object[] { "Apple iPhone 15 Pro Max 256GB, Natural Titanium. Features the A17 Pro chip and a 5x Telephoto camera.", 60000.00m });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Price" },
                values: new object[] { "Samsung Galaxy S24 Ultra 512GB, Titanium Gray. Equipped with Galaxy AI and built-in S Pen.", 55000.00m });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Price" },
                values: new object[] { "100% premium cotton classic black t-shirt for men. Comfortable, breathable, and perfect for everyday wear.", 750.00m });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Price" },
                values: new object[] { "High-quality denim jeans, relaxed fit, classic blue wash. Durable material with standard 5-pocket styling.", 1500.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 2499.99m);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 1899.50m);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 1199.00m);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 1299.99m);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 19.99m);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 49.99m);
        }
    }
}
