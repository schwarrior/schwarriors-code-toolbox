use RestoredDB;

Alter User SqlAuthUser With Login = SqlAuthUser;

--or

EXEC sp_change_users_login 'Report';

EXEC sp_change_users_login 'update_one', 'SqlAuthUser', 'SqlAuthUser';

--will create missing logins
EXEC sp_change_users_login 'Auto_Fix', 'SqlAuthUser', NULL, 'password';

--serverwide report
use master;
go
DECLARE @command varchar(1000) 
SELECT @command = 'select db_name(); exec sp_change_users_login @Action=''Report'';' 
EXEC sp_MSforeachdb @command