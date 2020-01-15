using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class SetPartCodeAsUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentPartSetId",
                table: "Part",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ComplexityClass",
                table: "Order",
                maxLength: 128,
                nullable: false,
                defaultValue: "InEstimation",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_Part_Code",
                table: "Part",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Part_Code",
                table: "Part");

            migrationBuilder.AlterColumn<long>(
                name: "ParentPartSetId",
                table: "Part",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComplexityClass",
                table: "Order",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldDefaultValue: "InEstimation");
        }
    }
}
