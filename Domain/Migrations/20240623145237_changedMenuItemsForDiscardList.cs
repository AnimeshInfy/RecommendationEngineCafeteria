using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    public partial class changedMenuItemsForDiscardList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isItemUnderDiscardList",
                table: "MenuItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SentimentScore",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isItemUnderDiscardList",
                table: "MenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "SentimentScore",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
