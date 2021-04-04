create Proc [dbo].[SampleProc]
(
	@ID bigint
)
as
begin

SET XACT_ABORT ON

--log start
EXECUTE dbo.[LogThis] @StatusCode = 100, @ID = @ID

DECLARE @ProcResult Table
(
	ID int not null identity primary key, 
	CompleteDate datetime default getdate(), 
	InsertCount int not null default 0, 
	UpdateCount int not null default 0, 
	DeleteCount int not null default 0
)


BEGIN TRY

BEGIN TRANSACTION

-- do work here eg insert, update, delete

Insert @ProcResult(InsertCount,UpdateCount,DeleteCount)
Select 0 as InsertCount, 0 as UpdateCount, 0 as DeleteCount

COMMIT TRANSACTION

select * from @ProcResult
EXECUTE dbo.[LogThis] @StatusCode = 202, @ID = @ID

--Report Import Success
EXECUTE dbo.[LogThis] 
	@StatusCode = 200, @ID = @ID;

--Return Success Code to Client (SSIS understands 0 fail or 1 sucess)
SELECT cast(1 as bit) as Result, '' as ErrorTable, 'Success' as SQLError

END TRY

BEGIN CATCH
	
	IF XACT_STATE() <> 0 -- -1 = uncommittable, 0 = no transaction to rollback, 1 = committable
		ROLLBACK TRANSACTION
	
	DECLARE @SQLError varchar(1024)
	Select @SQLError = Error_Message() + '' (Line: '' + cast(ERROR_LINE() as varchar(5)) + '')''
	
	--Report Import Failure with error
	EXECUTE dbo.LogThis
		@StatusCode = 400, @ID = @ID, @Detail = @SQLError

	--RETURN failure code to Client (SSIS understands 0 fail or 1 sucess)
	Select cast(0 as bit) as Result, 
	@SQLError as SQLError
	
END CATCH
		
	
end --proc