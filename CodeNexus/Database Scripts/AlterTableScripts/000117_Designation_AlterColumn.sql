USE [PMSNexus_Employees]
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Designation' AND COLUMN_NAME = 'ModifiedBy')
BEGIN
    ALTER TABLE [dbo].[Designation] add ModifiedBy int null
	ALTER TABLE [dbo].[Designation] add ModifiedOn Datetime
END
