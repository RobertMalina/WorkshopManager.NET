using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class AppUserRemovalFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserToAppRole_Roles_RoleId",
                table: "AppUserToAppRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserToAppRole_Users_UserId",
                table: "AppUserToAppRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Worker_WorkerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WorkerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AppUser");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "AppRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserToAppRole_AppRole_RoleId",
                table: "AppUserToAppRole",
                column: "RoleId",
                principalTable: "AppRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserToAppRole_AppUser_UserId",
                table: "AppUserToAppRole",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserToAppRole_AppRole_RoleId",
                table: "AppUserToAppRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserToAppRole_AppUser_UserId",
                table: "AppUserToAppRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "AppRole",
                newName: "Roles");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Worker",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WorkerId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkerId",
                table: "Users",
                column: "WorkerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserToAppRole_Roles_RoleId",
                table: "AppUserToAppRole",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserToAppRole_Users_UserId",
                table: "AppUserToAppRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Worker_WorkerId",
                table: "Users",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
