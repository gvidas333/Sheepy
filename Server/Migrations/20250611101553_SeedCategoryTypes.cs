using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0c8e9981-434b-4d39-a23f-26e2d1a4a49c"), "Vaisiai ir Darzoves" },
                    { new Guid("ed80c4f6-d657-4ed4-854b-5254a7448a4e"), "Duona" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "Id",
                keyValue: new Guid("0c8e9981-434b-4d39-a23f-26e2d1a4a49c"));

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed80c4f6-d657-4ed4-854b-5254a7448a4e"));
        }
    }
}
