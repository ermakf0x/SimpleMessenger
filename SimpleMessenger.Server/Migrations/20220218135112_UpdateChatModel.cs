using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleMessenger.Server.Migrations
{
    public partial class UpdateChatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondUser",
                table: "Chats",
                newName: "SecondUserID");

            migrationBuilder.RenameColumn(
                name: "FirstUser",
                table: "Chats",
                newName: "FirstUserID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "SecondUserID",
                table: "Chats",
                newName: "SecondUser");

            migrationBuilder.RenameColumn(
                name: "FirstUserID",
                table: "Chats",
                newName: "FirstUser");
        }
    }
}
