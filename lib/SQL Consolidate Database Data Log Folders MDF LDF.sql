--Move databases to one central location

--old default location
--C:\Program Files\Microsoft SQL Server\MSSQL11.SQL2012DEV\MSSQL\DATA

--new default location and target for consolidation
--D:\SQLData

--CHANGE DEFAULT DIRECTORIES FOR NEW DBS
--With SSMS, go to Server Properties : Database Setttings 
--and change default folders

--user dbs

use master
go

SELECT 
'ALTER DATABASE ' +
d.name +
' MODIFY FILE ( NAME = ' +
mf.name +
' , FILENAME = ''D:\SQLData\' +
mf.name +
case mf.type_desc when 'ROWS' then '.mdf' else '.ldf' end +
''' );'
--select d.name, mf.name, physical_name AS CurrentLocation, mf.state_desc
FROM sys.master_files mf
inner join sys.databases d
	on mf.database_id = d.database_id
WHERE physical_name not like 'D:\SQLData%'


--msdb

SELECT name, physical_name AS CurrentLocation, state_desc
FROM sys.master_files
WHERE database_id = DB_ID(N'msdb');

ALTER DATABASE msdb MODIFY FILE ( NAME = MSDBData , FILENAME = 
'D:\SQLData\MSDBData.mdf' )

ALTER DATABASE msdb MODIFY FILE ( NAME = MSDBLog , FILENAME = 
'D:\SQLData\MSDBLog.ldf' )

--tempdb

SELECT name, physical_name AS CurrentLocation, state_desc
FROM sys.master_files
WHERE database_id = DB_ID(N'tempdb');

ALTER DATABASE tempdb MODIFY FILE ( NAME = tempdev , FILENAME = 
'D:\SQLData\tempdev.mdf' )

ALTER DATABASE tempdb MODIFY FILE ( NAME = templog , FILENAME = 
'D:\SQLData\templog.ldf' )