------------------------------------------------------------------------------
-- SQL Deployment ------------------------------------------------------------
-- Sample Sproc Change -------------------------------------------------------
-- Production Target: ALphaSql.SampleDb --------------------------------------
-- QA Target: BravoSql.SampleDb ----------------------------------------------
------------------------------------------------------------------------------


------------------------------------------------------------------------------
-- ROLLBACK ------------------------------------------------------------------
------------------------------------------------------------------------------

if exists(select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'SampleSproc')
	DROP PROCEDURE SampleSproc
GO

CREATE proc [dbo].[SampleSproc] as
	select 'Legacy version of Sproc'
GO

------------------------------------------------------------------------------
-- DEPLOY --------------------------------------------------------------------
------------------------------------------------------------------------------

if exists(select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'SampleSproc')
	DROP PROCEDURE SampleSproc
GO

CREATE proc [dbo].[SampleSproc] as
	select 'New version of Sproc'
GO