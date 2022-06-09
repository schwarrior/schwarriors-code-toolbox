
alter table TestTypes
add ModifyDate datetime null default getdate(),
ModifyUser sysname null default current_user,
ModifyHost sysname null default host_name();

CREATE TRIGGER dbo.Trigger_AfterUpdate_TestType_Audit
   ON  dbo.TestTypes
   AFTER UPDATE, INSERT
AS 
BEGIN

	SET NOCOUNT ON;

	update 
		tt
	set 
		tt.ModifyDate = getdate(),
		tt.ModifyUser = current_user,
		tt.ModifyHost = host_name()
	from
		inserted i 
		inner join [dbo].[TestTypes] tt
			on i.TestTypeID = tt.TestTypeID


END
GO

-- testing
update top (1) TestTypes
set [Name] = [Name]
where TestTypeID = '5FADBDBD-080F-4265-A2D3-23B747790101';
go

select * from TestTypes
go