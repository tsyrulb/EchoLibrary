using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations.MariaDb
{
    public partial class MariaDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_UserDB_Username",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Contact_Contactid",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "MessageDB");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "ContactDB");

            migrationBuilder.RenameIndex(
                name: "IX_Message_Contactid",
                table: "MessageDB",
                newName: "IX_MessageDB_Contactid");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_Username",
                table: "ContactDB",
                newName: "IX_ContactDB_Username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageDB",
                table: "MessageDB",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactDB",
                table: "ContactDB",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactDB_UserDB_Username",
                table: "ContactDB",
                column: "Username",
                principalTable: "UserDB",
                principalColumn: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageDB_ContactDB_Contactid",
                table: "MessageDB",
                column: "Contactid",
                principalTable: "ContactDB",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactDB_UserDB_Username",
                table: "ContactDB");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageDB_ContactDB_Contactid",
                table: "MessageDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageDB",
                table: "MessageDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactDB",
                table: "ContactDB");

            migrationBuilder.RenameTable(
                name: "MessageDB",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "ContactDB",
                newName: "Contact");

            migrationBuilder.RenameIndex(
                name: "IX_MessageDB_Contactid",
                table: "Message",
                newName: "IX_Message_Contactid");

            migrationBuilder.RenameIndex(
                name: "IX_ContactDB_Username",
                table: "Contact",
                newName: "IX_Contact_Username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_UserDB_Username",
                table: "Contact",
                column: "Username",
                principalTable: "UserDB",
                principalColumn: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Contact_Contactid",
                table: "Message",
                column: "Contactid",
                principalTable: "Contact",
                principalColumn: "id");
        }
    }
}
