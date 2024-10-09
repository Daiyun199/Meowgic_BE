using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meowgic.Data.Migrations
{
    /// <inheritdoc />
    public partial class accountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "otpResetPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otpResetPassword",
                table: "AspNetUsers");
        }
    }
}
