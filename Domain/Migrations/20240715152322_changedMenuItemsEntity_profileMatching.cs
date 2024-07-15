using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class changedMenuItemsEntity_profileMatching : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpiceLevel",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dietType",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "isSweetTooth",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "regionalMealPreference",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpiceLevel",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "dietType",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "isSweetTooth",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "regionalMealPreference",
                table: "MenuItems");
        }
    }
}
