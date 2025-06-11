using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesMadeUserSpecific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "Id",
                keyValue: new Guid("0c8e9981-434b-4d39-a23f-26e2d1a4a49c"));

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed80c4f6-d657-4ed4-854b-5254a7448a4e"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CategoryTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypes_UserId",
                table: "CategoryTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTypes_AspNetUsers_UserId",
                table: "CategoryTypes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTypes_AspNetUsers_UserId",
                table: "CategoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTypes_UserId",
                table: "CategoryTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CategoryTypes");

            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0c8e9981-434b-4d39-a23f-26e2d1a4a49c"), "Vaisiai ir Darzoves" },
                    { new Guid("ed80c4f6-d657-4ed4-854b-5254a7448a4e"), "Duona" }
                });
        }
    }
}
