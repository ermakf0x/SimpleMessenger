using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    RegDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    User2UID = table.Column<int>(type: "INTEGER", nullable: true),
                    Token = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UID);
                    table.ForeignKey(
                        name: "FK_User_User_User2UID",
                        column: x => x.User2UID,
                        principalTable: "User",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hash = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTimeModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstUser = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondUser = table.Column<int>(type: "INTEGER", nullable: false),
                    User2UID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_User_User2UID",
                        column: x => x.User2UID,
                        principalTable: "User",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateTable(
                name: "ChunkChat",
                columns: table => new
                {
                    CreationTime = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTimeModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hash = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChunkChat", x => new { x.OwnerId, x.CreationTime });
                    table.ForeignKey(
                        name: "FK_ChunkChat_Chats_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChunkChatCreationTime = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ChunkChatOwnerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_ChunkChat_ChunkChatOwnerId_ChunkChatCreationTime",
                        columns: x => new { x.ChunkChatOwnerId, x.ChunkChatCreationTime },
                        principalTable: "ChunkChat",
                        principalColumns: new[] { "OwnerId", "CreationTime" });
                    table.ForeignKey(
                        name: "FK_Message_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User2UID",
                table: "Chats",
                column: "User2UID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChunkChatOwnerId_ChunkChatCreationTime",
                table: "Message",
                columns: new[] { "ChunkChatOwnerId", "ChunkChatCreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_User2UID",
                table: "User",
                column: "User2UID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "ChunkChat");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
