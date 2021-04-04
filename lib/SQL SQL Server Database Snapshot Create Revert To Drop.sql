--DATABASE SNAPSHOTING

--CREATE

--use object explorer to script database create to get "on" section then add suffix to file name

use master;
go

create database SampleDb_snapshot_before_orig 
 ON  PRIMARY 
( NAME = N'SampleDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012DEV\MSSQL\DATA\SampleDb_snapshot_before_orig.mdf')
as snapshot of SampleDb
GO

--REVERT DB TO SNAPSHOT

use master;
go

ALTER DATABASE SampleDb
SET SINGLE_USER WITH
ROLLBACK AFTER 10 
RESTORE DATABASE SampleDb from 
DATABASE_SNAPSHOT = 'SampleDb_snapshot_before_orig';
ALTER DATABASE SampleDb SET MULTI_USER
GO

--DROP A SNAPSHOT

use master;
go

DROP DATABASE SampleDb_snapshot_before_orig;
GO

-- SNAPSHOT Test

use SampleDb;
go

--Relies on a existing table MoreExamples
--Create table MoreExamples
--(
--	Id int not null primary key,
--	Example nvarchar(200) null
--);

drop table MoreExamples;

-- Now REVERT DB to SNAPSHOT
-- MoreExamples table should once again be present