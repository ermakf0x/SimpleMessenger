using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class newChatModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hash",
                table: "ChunkChat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hash",
                table: "Chats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "ChunkChat");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Chats");
        }
    }
}
