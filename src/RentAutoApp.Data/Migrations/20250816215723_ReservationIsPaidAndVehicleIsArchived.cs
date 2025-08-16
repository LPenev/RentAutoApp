using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReservationIsPaidAndVehicleIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 16, 21, 57, 17, 863, DateTimeKind.Utc).AddTicks(8340));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 14, 15, 11, 24, 46, DateTimeKind.Utc).AddTicks(5550));
        }
    }
}
