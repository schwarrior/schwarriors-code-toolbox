--disable foreign key constraints

--single table
ALTER TABLE Users NOCHECK CONSTRAINT all

--all tables (often required because of cross refs)
-- disable all constraints
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- enable all constraints
exec sp_msforeachtable @command1="print '?'", @command2="ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"