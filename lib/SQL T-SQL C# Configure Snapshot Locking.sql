use master
go

ALTER DATABASE TargetDatabase
SET ALLOW_SNAPSHOT_ISOLATION ON

--Might require TargetDatabase to be in Single User mode
ALTER DATABASE TargetDatabase
SET READ_COMMITTED_SNAPSHOT ON


--use in c#
--SqlTransaction sqlTran = connection.BeginTransaction(IsolationLevel.Snapshot);
