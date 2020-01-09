using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class AuthSystemMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AppUserId",
                table: "Worker",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Worker_AppUserId",
                table: "Worker",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_AppUser_AppUserId",
                table: "Worker",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worker_AppUser_AppUserId",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_Worker_AppUserId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Worker");
        }
    }
}
