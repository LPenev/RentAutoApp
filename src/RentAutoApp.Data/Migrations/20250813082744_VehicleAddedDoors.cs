using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class VehicleAddedDoors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Doors",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Vehicles_Doors",
                table: "Vehicles",
                sql: "[Doors] >= 2 AND [Doors] <= 6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Vehicles_Doors",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Doors",
                table: "Vehicles");
        }
    }
}
