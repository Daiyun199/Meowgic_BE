using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meowgic.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderDetail2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "ScheduleReaderId",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ScheduleReaderId",
                table: "OrderDetails",
                column: "ScheduleReaderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ScheduleReaders_ScheduleReaderId",
                table: "OrderDetails",
                column: "ScheduleReaderId",
                principalTable: "ScheduleReaders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ScheduleReaders_ScheduleReaderId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ScheduleReaderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ScheduleReaderId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "OrderDetails",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "OrderDetails",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "OrderDetails",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
