using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class UpdateChatModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_User_FirstUserID",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_User_SecondUserID",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_FirstUserID",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_SecondUserID",
                table: "Chats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Chats_FirstUserID",
                table: "Chats",
                column: "FirstUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SecondUserID",
                table: "Chats",
                column: "SecondUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_User_FirstUserID",
                table: "Chats",
                column: "FirstUserID",
                principalTable: "User",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_User_SecondUserID",
                table: "Chats",
                column: "SecondUserID",
                principalTable: "User",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
