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
                name: "Users",
                columns: table => new
                {
                    UID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    RegTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    User2UID = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Users_Users_User2UID",
                        column: x => x.User2UID,
                        principalTable: "Users",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_User2UID",
                table: "Users",
                column: "User2UID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
