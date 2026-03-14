using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Electronics", null },
                    { 4, "Clothing", null },
                    { 2, "Laptops", 1 },
                    { 3, "Smartphones", 1 },
                    { 5, "Men's Fashion", 4 }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "IsActive", "Name", "Price", "SKU", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "MacBook Pro 16-inch", 2499.99m, "MAC-PRO-16-2024", 10 },
                    { 2, 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Dell XPS 15", 1899.50m, "DELL-XPS-15-OLED", 15 },
                    { 3, 3, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "iPhone 15 Pro Max", 1199.00m, "IPH-15-PM-256", 25 },
                    { 4, 3, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Samsung Galaxy S24 Ultra", 1299.99m, "SAM-S24-ULT-512", 20 },
                    { 5, 5, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Classic Cotton T-Shirt", 19.99m, "TSHIRT-MEN-BLK-M", 100 },
                    { 6, 5, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Denim Jeans Relaxed Fit", 49.99m, "JEANS-MEN-BLU-32", 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
