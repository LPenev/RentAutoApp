using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserAddDateTimeNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 14, 15, 11, 24, 46, DateTimeKind.Utc).AddTicks(5550));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 14, 8, 53, 3, 205, DateTimeKind.Utc).AddTicks(8011));
        }
    }
}
