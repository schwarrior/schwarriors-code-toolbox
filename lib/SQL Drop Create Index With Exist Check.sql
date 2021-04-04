if exists(
	SELECT * 
	FROM sys.indexes 
	WHERE name='IDX_User_LastName_FirstName' AND object_id = OBJECT_ID('dbo.User')
)
DROP INDEX [IDX_User_LastName_FirstName] ON dbo.User
go

CREATE NONCLUSTERED INDEX [IDX_User_LastName_FirstName] ON dbo.User
(
	[LastName] ASC,
	[FirstName] ASC
)
INCLUDE 
( 	
	[CreateDate],
	[ModifyDate]
) 
WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go