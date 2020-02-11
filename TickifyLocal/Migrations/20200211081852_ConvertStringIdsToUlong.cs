using Microsoft.EntityFrameworkCore.Migrations;

namespace Tickify.Migrations
{
    public partial class ConvertStringIdsToUlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "UserId",
                table: "Tickets",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<ulong>(
                name: "ChannelId",
                table: "Tickets",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 25);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Tickets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "varchar(25) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(ulong),
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "ChannelId",
                table: "Tickets",
                type: "varchar(25) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(ulong),
                oldMaxLength: 25);
        }
    }
}
