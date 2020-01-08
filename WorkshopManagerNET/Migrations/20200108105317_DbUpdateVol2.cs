using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class DbUpdateVol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartsCount",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "VehicleModel",
                table: "Order",
                newName: "VehicleDescription");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Order",
                newName: "Cost");

            migrationBuilder.AddColumn<long>(
                name: "ParentPartSetId",
                table: "Part",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Order",
                type: "datetime2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)");

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Order",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegister",
                table: "Order",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedTimeInHours",
                table: "Order",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Order",
                maxLength: 128,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<long>(
                name: "SupervisorId",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(128)", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Hours = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    WorkerId = table.Column<long>(nullable: false),
                    OrderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeLogs_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeLogs_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserToAppRole",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserToAppRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserToAppRole_AppRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserToAppRole_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Part_ParentPartSetId",
                table: "Part",
                column: "ParentPartSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SupervisorId",
                table: "Order",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserToAppRole_RoleId",
                table: "AppUserToAppRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLogs_OrderId",
                table: "TimeLogs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLogs_WorkerId",
                table: "TimeLogs",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Worker_SupervisorId",
                table: "Order",
                column: "SupervisorId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part",
                column: "ParentPartSetId",
                principalTable: "Part",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Worker_SupervisorId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Part_Part_ParentPartSetId",
                table: "Part");

            migrationBuilder.DropTable(
                name: "AppUserToAppRole");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "TimeLogs");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_Part_ParentPartSetId",
                table: "Part");

            migrationBuilder.DropIndex(
                name: "IX_Order_SupervisorId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ParentPartSetId",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DateRegister",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EstimatedTimeInHours",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "VehicleDescription",
                table: "Order",
                newName: "VehicleModel");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Order",
                newName: "Price");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Order",
                type: "datetime2(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PartsCount",
                table: "Order",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
