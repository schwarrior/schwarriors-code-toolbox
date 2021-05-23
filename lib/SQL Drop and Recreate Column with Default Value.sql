--if a column is created with an unnamed default constraint / implicitly named constraint like so
alter table report.User add CreateDate datetime2 not null User default getdate();

--the default gets an automatically assigned unique name
--drop auto generated constraint
declare @constName varchar(500);
select @constName = name from sys.default_constraints where name like 'DF__User__CreateDate__%';
declare @sql varchar(1000) =  'alter table User drop constraint ' + @constName;
print @sql
exec(@sql)
