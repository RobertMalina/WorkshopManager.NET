using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
    public partial class TimeLogTraineeTablesSingularizedNameForcing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_Order_OrderId",
                table: "TimeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_Worker_WorkerId",
                table: "TimeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Worker_SupervisorId",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeLogs",
                table: "TimeLogs");

            migrationBuilder.RenameTable(
                name: "Trainees",
                newName: "Trainee");

            migrationBuilder.RenameTable(
                name: "TimeLogs",
                newName: "TimeLog");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_SupervisorId",
                table: "Trainee",
                newName: "IX_Trainee_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeLogs_WorkerId",
                table: "TimeLog",
                newName: "IX_TimeLog_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeLogs_OrderId",
                table: "TimeLog",
                newName: "IX_TimeLog_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainee",
                table: "Trainee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeLog",
                table: "TimeLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLog_Order_OrderId",
                table: "TimeLog",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLog_Worker_WorkerId",
                table: "TimeLog",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainee_Worker_SupervisorId",
                table: "Trainee",
                column: "SupervisorId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLog_Order_OrderId",
                table: "TimeLog");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLog_Worker_WorkerId",
                table: "TimeLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainee_Worker_SupervisorId",
                table: "Trainee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainee",
                table: "Trainee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeLog",
                table: "TimeLog");

            migrationBuilder.RenameTable(
                name: "Trainee",
                newName: "Trainees");

            migrationBuilder.RenameTable(
                name: "TimeLog",
                newName: "TimeLogs");

            migrationBuilder.RenameIndex(
                name: "IX_Trainee_SupervisorId",
                table: "Trainees",
                newName: "IX_Trainees_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeLog_WorkerId",
                table: "TimeLogs",
                newName: "IX_TimeLogs_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeLog_OrderId",
                table: "TimeLogs",
                newName: "IX_TimeLogs_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeLogs",
                table: "TimeLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_Order_OrderId",
                table: "TimeLogs",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_Worker_WorkerId",
                table: "TimeLogs",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Worker_SupervisorId",
                table: "Trainees",
                column: "SupervisorId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
