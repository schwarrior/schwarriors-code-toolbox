--down

if exists(select * from INFORMATION_SCHEMA.columns where TABLE_NAME='User' and COLUMN_NAME='CreateDate')

begin

alter table report.User drop constraint DFLT_User_CreateDate;

alter table report.User drop column CreateDate;

end;

--up

alter table report.User add CreateDate datetime2 not null User default getdate();



--if the default was created without specifying a name and the default gets a unique name
--drop auto generated constraint
declare @constName varchar(500);
select @constName = name from sys.default_constraints where name like 'DF__User__CreateDate__%';
declare @sql varchar(1000) =  'alter table User drop constraint ' + @constName;
print @sql
exec(@sql)
