using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopManagerNET.Migrations
{
  public partial class functionsAndProceduresAdd_v1 : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var workerConstraintsAdd_sp = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkerConstraintsAdd]'))
	DROP PROCEDURE [dbo].[WorkerConstraintsAdd]
GO
CREATE PROCEDURE [dbo].[WorkerConstraintsAdd]
AS
BEGIN
	--Order to Worker (Supervisor)
	BEGIN TRY
		ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Worker_SupervisorId] FOREIGN KEY([SupervisorId])
		REFERENCES [dbo].[Worker] ([Id])
		ON DELETE SET NULL
	END TRY
	BEGIN CATCH
		--SELECT '[FK_Order_Worker_SupervisorId] already exists.' AS CollisionDetectedInfo;
	END CATCH

	--OrderToWorker (junction table) to Worker
	ALTER TABLE [dbo].[OrderToWorker]
	WITH CHECK ADD  CONSTRAINT [FK_OrderToWorker_Worker_WorkerId] FOREIGN KEY([WorkerId])
	REFERENCES [dbo].[Worker] ([Id])
	ON DELETE CASCADE;

	--TimeLog to Worker
	ALTER TABLE [dbo].[TimeLog]  
	WITH CHECK ADD  CONSTRAINT [FK_TimeLog_Worker_WorkerId] FOREIGN KEY([WorkerId])
	REFERENCES [dbo].[Worker] ([Id])
	ON DELETE CASCADE

	--Trainee to Worker
	ALTER TABLE [dbo].[Trainee]  
	WITH CHECK ADD  CONSTRAINT [FK_Trainee_Worker_SupervisorId] FOREIGN KEY([SupervisorId])
	REFERENCES [dbo].[Worker] ([Id])
	ON DELETE CASCADE

END";
      var workerConstraintsRemove_sp = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkerConstraintsRemove]'))
	DROP PROCEDURE [dbo].[WorkerConstraintsRemove]
GO
CREATE PROCEDURE [dbo].[WorkerConstraintsRemove]
AS
BEGIN
	--Order to Worker (Supervisor)
	BEGIN TRY
		ALTER TABLE [dbo].[Order]
		DROP CONSTRAINT [FK_Order_Worker_SupervisorId];
	END TRY
	BEGIN CATCH
		--SELECT '[FK_Order_Worker_SupervisorId] was already deleted.' AS CollisionDetectedInfo;
	END CATCH

	--OrderToWorker (junction table) to Worker
	BEGIN TRY
		ALTER TABLE [OrderToWorker]
		DROP CONSTRAINT [FK_OrderToWorker_Worker_WorkerId];
	END TRY
	BEGIN CATCH
		--SELECT '[FK_OrderToWorker_Worker_WorkerId] was already deleted.' AS CollisionDetectedInfo;
	END CATCH

	
	--TimeLog to Worker
	BEGIN TRY
		ALTER TABLE [TimeLog]
		DROP CONSTRAINT [FK_TimeLog_Worker_WorkerId];
	END TRY
	BEGIN CATCH
		--SELECT '[FK_TimeLog_Worker_WorkerId] was already deleted.' AS CollisionDetectedInfo;
	END CATCH

	--Trainee to Worker
	BEGIN TRY
		ALTER TABLE [Trainee]
		DROP CONSTRAINT [FK_Trainee_Worker_SupervisorId];
	END TRY
	BEGIN CATCH
		--SELECT '[FK_Trainee_Worker_SupervisorId] was already deleted.' AS CollisionDetectedInfo;
	END CATCH
END";
      var orderConstraintsAdd_sp = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderConstraintsAdd]'))
	DROP PROCEDURE [dbo].[OrderConstraintsAdd]
GO
CREATE PROCEDURE [dbo].[OrderConstraintsAdd]
AS
BEGIN
	--Order to Client FK key restore
	ALTER TABLE [Order]
	WITH CHECK ADD CONSTRAINT [FK_Order_Client_ClientId] FOREIGN KEY ([ClientId]) 
	REFERENCES [dbo].[Client] ([Id])
	ON DELETE CASCADE;

	--Order to Worker (Supervisor) FK key restore
	ALTER TABLE [Order]
	WITH CHECK ADD CONSTRAINT [FK_Order_Worker_SupervisorId] FOREIGN KEY ([SupervisorId]) 	
	REFERENCES [dbo].[Worker] ([Id])
	ON DELETE SET NULL;

	--Part to Order FK key restore
	ALTER TABLE [dbo].[Part]  
	WITH CHECK ADD CONSTRAINT [FK_Part_Order_OrderId] FOREIGN KEY([OrderId])
	REFERENCES [dbo].[Order] ([Id])
	ON DELETE CASCADE

	--TimeLog to Order FK key restore
	ALTER TABLE [dbo].[TimeLog] 
	WITH CHECK ADD  CONSTRAINT [FK_TimeLog_Order_OrderId] FOREIGN KEY([WorkerId])
	REFERENCES [dbo].[Order] ([Id])
	ON DELETE CASCADE;

	--OrderToWorker to Order FK key removal
	ALTER TABLE [dbo].[OrderToWorker]  
	WITH CHECK ADD  CONSTRAINT [FK_OrderToWorker_Order_OrderId] FOREIGN KEY([OrderId])
	REFERENCES [dbo].[Order] ([Id])
	ON DELETE CASCADE
END";
      var orderConstraintsRemove_sp = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderConstraintsRemove]'))
	DROP PROCEDURE [dbo].[OrderConstraintsRemove]
GO
CREATE PROCEDURE [dbo].[OrderConstraintsRemove]
AS
BEGIN
	--Order to Client FK key removal
	BEGIN TRY
		ALTER TABLE [Order]
		DROP CONSTRAINT [FK_Order_Client_ClientId];		
	END TRY
	BEGIN CATCH
	END CATCH

	--Order to Worker FK key removal
	BEGIN TRY
			ALTER TABLE [dbo].[Order]
		DROP CONSTRAINT [FK_Order_Worker_SupervisorId];
	END TRY
	BEGIN CATCH
	END CATCH

	--Part to Order FK key removal
	BEGIN TRY
		ALTER TABLE [dbo].[Part]
		DROP CONSTRAINT [FK_Part_Order_OrderId];
	END TRY
	BEGIN CATCH
	END CATCH

	--TimeLog to Order FK key removal
	BEGIN TRY
		ALTER TABLE [dbo].[TimeLog]
		DROP CONSTRAINT [FK_TimeLog_Order_OrderId];		
	END TRY
	BEGIN CATCH
	END CATCH

	--OrderToWorker to Order FK key removal
	BEGIN TRY
		ALTER TABLE [dbo].[OrderToWorker]
		DROP CONSTRAINT [FK_OrderToWorker_Order_OrderId];		
	END TRY
	BEGIN CATCH
	END CATCH
END";
      var dbClear_sp = @"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClearDatabase]'))
	DROP PROCEDURE [dbo].[ClearDatabase]
GO
CREATE PROCEDURE [dbo].[ClearDatabase]
AS
BEGIN
	BEGIN TRY
		EXEC [dbo].[OrderConstraintsRemove];
		EXEC [dbo].[WorkerConstraintsRemove];
		TRUNCATE TABLE [dbo].[Order];
		TRUNCATE TABLE [dbo].[Worker];
		TRUNCATE TABLE [dbo].[Part];

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
GO";

			var getOrdersRegisteredSince_tab_func = @"IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrdersRegisteredSince]'))
	DROP FUNCTION [dbo].[GetOrdersRegisteredSince];
GO
CREATE FUNCTION [dbo].[GetOrdersRegisteredSince](
	@days INT
)
RETURNS TABLE
AS
RETURN
    SELECT 
       [Id]
      ,[ClientId]
      ,[Title]
      ,[VehicleDescription]
      ,[Description]
      ,[DateStart]
      ,[DateEnd]
      ,[Cost]
      ,[Archived]
      ,[DateRegister]
      ,[EstimatedTimeInHours]
      ,[Status]
      ,[SupervisorId]
      ,[ComplexityClass]
    FROM
        [dbo].[Order] O
    WHERE
		DATEDIFF(DAY, O.DateRegister, GETDATE() ) < @days;";

			var getSummarizedPartsCostsOfOrder_sc_func = @"IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSummarizedPartsCostsOfOrder]'))
	DROP FUNCTION [dbo].[GetSummarizedPartsCostsOfOrder];
GO
CREATE FUNCTION [dbo].[GetSummarizedPartsCostsOfOrder](
	@orderId BIGINT
)
RETURNS decimal(9, 2)
AS
BEGIN
DECLARE @sum decimal(9, 2);
SET @sum = 4;
SET @sum = (SELECT SUM(P.Price) FROM [dbo].[Part] P WHERE P.OrderId = @orderId);
	RETURN @sum;
END";

			migrationBuilder.Sql(workerConstraintsAdd_sp);
			migrationBuilder.Sql(workerConstraintsRemove_sp);
			migrationBuilder.Sql(orderConstraintsAdd_sp);
			migrationBuilder.Sql(orderConstraintsRemove_sp);
			migrationBuilder.Sql(dbClear_sp);

			migrationBuilder.Sql(getOrdersRegisteredSince_tab_func);
			migrationBuilder.Sql(getSummarizedPartsCostsOfOrder_sc_func);
		}

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
