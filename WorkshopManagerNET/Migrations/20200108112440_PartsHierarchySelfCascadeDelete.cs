using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class PartsHierarchySelfCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part");

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part",
                column: "ParentPartSetId",
                principalTable: "Part",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part");

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part",
                column: "ParentPartSetId",
                principalTable: "Part",
                principalColumn: "Id");
        }
    }
}
