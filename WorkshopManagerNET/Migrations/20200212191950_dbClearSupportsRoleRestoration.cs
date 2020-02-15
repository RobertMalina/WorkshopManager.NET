using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
  public partial class dbClearSupportsRoleRestoration : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var dbClearModified = @"USE [Maq]
GO
/****** Object:  StoredProcedure [dbo].[ClearDatabase]    Script Date: 12/02/2020 20:24:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ClearDatabase] @restoreRoles BIT
AS
BEGIN
	BEGIN TRY
		EXEC [dbo].[OrderConstraintsRemove];
		EXEC [dbo].[WorkerConstraintsRemove];
		TRUNCATE TABLE [dbo].[Order];
		TRUNCATE TABLE [dbo].[Worker];
		TRUNCATE TABLE [dbo].[Part];

		DELETE FROM [dbo].[AppUserToAppRole];
		-- DBCC CHECKIDENT ('[AppUserToAppRole]', RESEED, 0); -- TODO weryfikacja: 'AppUserToAppRole' does not contain an identity colum
		DELETE FROM [dbo].[AppUser];
		DBCC CHECKIDENT ('[AppUser]', RESEED, 0);
		DELETE FROM [dbo].[AppRole];

		TRUNCATE TABLE [dbo].[TimeLog];
		TRUNCATE TABLE [dbo].[OrderToWorker];	
		TRUNCATE TABLE [dbo].[Client];
		EXEC [dbo].[OrderConstraintsAdd];
		EXEC [dbo].[WorkerConstraintsAdd];

		IF @restoreRoles = 1
		BEGIN
			EXEC SetAppRoles
		END

	END TRY		
	BEGIN CATCH
		SELECT 
		'failure' as [Status],
		ERROR_PROCEDURE() AS [Procedure], 
		ERROR_MESSAGE() AS ErrorMessage;
	END CATCH;
END;
";
			migrationBuilder.Sql(dbClearModified);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
