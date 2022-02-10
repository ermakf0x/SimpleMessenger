using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class newChatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ChatId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Message");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeModified",
                table: "Chats",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChunkChat",
                columns: table => new
                {
                    CreationTime = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ChatId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTimeModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChunkChat", x => x.CreationTime);
                    table.ForeignKey(
                        name: "FK_ChunkChat_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChunkChat_ChatId",
                table: "ChunkChat",
                column: "ChatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChunkChat");

            migrationBuilder.DropColumn(
                name: "LastTimeModified",
                table: "Chats");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Message",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
