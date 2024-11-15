using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meowgic.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSpecial",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSpecial",
                table: "Services");
        }
    }
}
