set xact_abort on;
go
begin tran
ALTER TABLE users
NOCHECK CONSTRAINT [FK_users_applicationid_to_applications_applicationid]; 
update users set ApplicationID = 'e2104e4d-aacc-4b76-b851-3f67fa98b181'
update applications set ApplicationID = 'e2104e4d-aacc-4b76-b851-3f67fa98b181'
ALTER TABLE users
CHECK CONSTRAINT [FK_users_applicationid_to_applications_applicationid];
commit
go
