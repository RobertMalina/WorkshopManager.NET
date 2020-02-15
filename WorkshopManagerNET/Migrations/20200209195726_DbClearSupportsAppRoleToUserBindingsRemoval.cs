using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
  public partial class DbClearSupportsAppRoleToUserBindingsRemoval : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var dbClearUpdate = @"USE [Maq]
GO
/****** Object:  StoredProcedure [dbo].[ClearDatabase]    Script Date: 09/02/2020 20:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ClearDatabase]
AS
BEGIN
	BEGIN TRY
		EXEC [dbo].[OrderConstraintsRemove];
		EXEC [dbo].[WorkerConstraintsRemove];
		TRUNCATE TABLE [dbo].[Order];
		TRUNCATE TABLE [dbo].[Worker];
		TRUNCATE TABLE [dbo].[Part];

		DELETE FROM [dbo].[AppUserToAppRole];
		DBCC CHECKIDENT ('[AppUserToAppRole]', RESEED, 0);
		DELETE FROM [dbo].[AppUser];
		DBCC CHECKIDENT ('[AppUser]', RESEED, 0);

		TRUNCATE TABLE [dbo].[TimeLog];
		TRUNCATE TABLE [dbo].[OrderToWorker];	
		TRUNCATE TABLE [dbo].[Client];
		EXEC [dbo].[OrderConstraintsAdd];
		EXEC [dbo].[WorkerConstraintsAdd];
	END TRY		
	BEGIN CATCH
		SELECT 
		'failure' as [Status],
		ERROR_PROCEDURE() AS [Procedure], 
		ERROR_MESSAGE() AS ErrorMessage;
	END CATCH;
END;
";
			migrationBuilder.Sql(dbClearUpdate);

    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
