using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChunkChat_Chats_ChatId",
                table: "ChunkChat");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "ChunkChat",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ChunkChat_Chats_ChatId",
                table: "ChunkChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChunkChat_Chats_ChatId",
                table: "ChunkChat");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "ChunkChat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChunkChat_Chats_ChatId",
                table: "ChunkChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
