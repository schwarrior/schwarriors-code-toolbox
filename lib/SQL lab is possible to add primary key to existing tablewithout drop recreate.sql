--Is it possible to add a primary to an exisitng table with data
--without using the drop / create trick the SSMS designer uses?

--It seems the answer is no, at least so far. 
--Currently the below lab SQL fails when trying to add identity to a null column
--Tried many many variant patterns for defining then changing the column default, nullability and identity 
--Alwyays got blocked by SQL Server on some crucial maneuver

--Long ago, I remember navigating a way through this quagmire 
--but it no longer seems to work on latest version of SQL Server

set xact_abort on;

if exists(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Setting' and COLUMN_NAME='SettingId')
THROW 51009, 'The Setting table already has a key.', 1; 

alter table Setting
	add SettingId int null identity (1000, 1);
GO

if not exists(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='Setting' and COLUMN_NAME='SettingId')
THROW 51012, 'The Setting table doesnot have the SettingId column.', 1; 

if exists(select * from sys.default_constraints where name = 'PK_Setting_SettingId')
THROW 51010, 'The Setting table SettingId column is already primary.', 1; 

with newKeys(SettingId, Category, [Name]) as
(
	select 
		ROW_NUMBER() OVER (ORDER BY Category, [Name]) as SettingId,
		Category,
		[Name]
	from 
		Setting
)
Update
	s
Set
	s.SettingId = nk.SettingId
From
	Setting s
	inner join newKeys nk
		on s.Category = nk.Category
		and s.[Name] = nk.[Name];

alter table Setting
	alter column SettingId int not null; -- cant add identity in an alter statement
GO

if exists(select * from sys.default_constraints where name = 'PK_Setting_SettingId')
THROW 51010, 'The Setting table SettingId column is already primary.', 1; 

alter table Setting
	-- ADD CONSTRAINT PK_Setting_SettingId PRIMARY KEY CLUSTERED (SettingId)	
	ADD CONSTRAINT PK_Setting_SettingId PRIMARY KEY NONCLUSTERED (SettingId);
GO
