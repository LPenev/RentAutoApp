using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesIntoVehicleAndReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 16, 23, 12, 4, 204, DateTimeKind.Utc).AddTicks(3228));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_IsArchived_IsAvailable_SubCategoryId_LocationId",
                table: "Vehicles",
                columns: new[] { "IsArchived", "IsAvailable", "SubCategoryId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_IsArchived_LocationId_PricePerDay",
                table: "Vehicles",
                columns: new[] { "IsArchived", "LocationId", "PricePerDay" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IsPaid",
                table: "Reservations",
                column: "IsPaid");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Status_StartDate",
                table: "Reservations",
                columns: new[] { "Status", "StartDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_IsArchived_IsAvailable_SubCategoryId_LocationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_IsArchived_LocationId_PricePerDay",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_IsPaid",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Status_StartDate",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "SiteSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedOnUtc",
                value: new DateTime(2025, 8, 16, 21, 57, 17, 863, DateTimeKind.Utc).AddTicks(8340));
        }
    }
}
