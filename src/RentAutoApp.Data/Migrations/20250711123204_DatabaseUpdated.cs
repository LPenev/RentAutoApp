using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAutoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DamageReport_AspNetUsers_UserId",
                table: "DamageReport");

            migrationBuilder.DropForeignKey(
                name: "FK_DamageReport_Reservations_ReservationId",
                table: "DamageReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_AspNetUsers_UserId",
                table: "Discount");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Reservations_ReservationId",
                table: "Discount");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Vehicles_VehicleId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImage_Vehicles_VehicleId",
                table: "VehicleImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleImage",
                table: "VehicleImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DamageReport",
                table: "DamageReport");

            migrationBuilder.RenameTable(
                name: "VehicleImage",
                newName: "VehicleImages");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "Discounts");

            migrationBuilder.RenameTable(
                name: "DamageReport",
                newName: "DamageReports");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleImage_VehicleId",
                table: "VehicleImages",
                newName: "IX_VehicleImages_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Service_VehicleId",
                table: "Services",
                newName: "IX_Services_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_UserId",
                table: "Discounts",
                newName: "IX_Discounts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_ReservationId",
                table: "Discounts",
                newName: "IX_Discounts_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_DamageReport_UserId",
                table: "DamageReports",
                newName: "IX_DamageReports_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DamageReport_ReservationId",
                table: "DamageReports",
                newName: "IX_DamageReports_ReservationId");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUrl",
                table: "VehicleDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleImages",
                table: "VehicleImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DamageReports",
                table: "DamageReports",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_DiscountId",
                table: "Reservations",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DocumentId",
                table: "Notifications",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReservationId",
                table: "Notifications",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ServiceId",
                table: "Notifications",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VehicleId",
                table: "Notifications",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DamageReports_AspNetUsers_UserId",
                table: "DamageReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DamageReports_Reservations_ReservationId",
                table: "DamageReports",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_AspNetUsers_UserId",
                table: "Discounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Reservations_ReservationId",
                table: "Discounts",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Reservations_ReservationId",
                table: "Notifications",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Services_ServiceId",
                table: "Notifications",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_VehicleDocuments_DocumentId",
                table: "Notifications",
                column: "DocumentId",
                principalTable: "VehicleDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Vehicles_VehicleId",
                table: "Notifications",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Discounts_DiscountId",
                table: "Reservations",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Vehicles_VehicleId",
                table: "Services",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImages_Vehicles_VehicleId",
                table: "VehicleImages",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DamageReports_AspNetUsers_UserId",
                table: "DamageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DamageReports_Reservations_ReservationId",
                table: "DamageReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_AspNetUsers_UserId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Reservations_ReservationId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Reservations_ReservationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Services_ServiceId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_VehicleDocuments_DocumentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Vehicles_VehicleId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Discounts_DiscountId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Vehicles_VehicleId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImages_Vehicles_VehicleId",
                table: "VehicleImages");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_DiscountId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleImages",
                table: "VehicleImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DocumentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ReservationId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ServiceId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_VehicleId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DamageReports",
                table: "DamageReports");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Reservations");

            migrationBuilder.RenameTable(
                name: "VehicleImages",
                newName: "VehicleImage");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "Discount");

            migrationBuilder.RenameTable(
                name: "DamageReports",
                newName: "DamageReport");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleImages_VehicleId",
                table: "VehicleImage",
                newName: "IX_VehicleImage_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_VehicleId",
                table: "Service",
                newName: "IX_Service_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_UserId",
                table: "Discount",
                newName: "IX_Discount_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_ReservationId",
                table: "Discount",
                newName: "IX_Discount_ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_DamageReports_UserId",
                table: "DamageReport",
                newName: "IX_DamageReport_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DamageReports_ReservationId",
                table: "DamageReport",
                newName: "IX_DamageReport_ReservationId");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUrl",
                table: "VehicleDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notification",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notification",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleImage",
                table: "VehicleImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DamageReport",
                table: "DamageReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DamageReport_AspNetUsers_UserId",
                table: "DamageReport",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DamageReport_Reservations_ReservationId",
                table: "DamageReport",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_AspNetUsers_UserId",
                table: "Discount",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Reservations_ReservationId",
                table: "Discount",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Vehicles_VehicleId",
                table: "Service",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImage_Vehicles_VehicleId",
                table: "VehicleImage",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
