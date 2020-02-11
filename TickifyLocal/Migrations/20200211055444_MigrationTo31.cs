using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tickify.Migrations
{
    public partial class MigrationTo31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(maxLength: 25, nullable: false),
                    NewTicketMessage = table.Column<string>(maxLength: 1500, nullable: false, defaultValue: ", this is your chat to voice your complaint to the support team. When everything is finished between you and the support team, please do !close!"),
                    TicketPrefix = table.Column<string>(maxLength: 20, nullable: true, defaultValue: "ticket"),
                    TicketCategory = table.Column<string>(maxLength: 20, nullable: false, defaultValue: "tickets"),
                    SupportTeam = table.Column<string>(maxLength: 20, nullable: false, defaultValue: "Support Team"),
                    RandomTicketNames = table.Column<bool>(nullable: false),
                    CommandPrefix = table.Column<string>(maxLength: 3, nullable: false, defaultValue: "!")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    UserId = table.Column<string>(unicode: false, maxLength: 25, nullable: false),
                    Subject = table.Column<string>(maxLength: 500, nullable: true),
                    GuildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_GuildId",
                table: "Guilds",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GuildId",
                table: "Tickets",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketId",
                table: "Tickets",
                column: "TicketId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
